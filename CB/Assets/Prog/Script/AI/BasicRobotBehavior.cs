using System;
using Bolt;
using Prog.Script.AI;
using Prog.Script.RigidbodyInteraction;
using UnityEngine;
using UnityEngine.AI;
using IState = Prog.Script.IState;
using StateMachine = Prog.Script.StateMachine;

public class BasicRobotBehavior : MonoBehaviour
{
    private StateMachine _stateMachine;
    
    public Target Target { get; set; }
    public Target[] patrolPoints;
    public LayerMask groundLayerMask;
    public bool isSearching = false;
    public bool isGrounded;
    public bool isAttacking;
    public bool isGettingAttacked;
    public float groundCheckerRadius = 1f;
    public float playerPosition;
    public float viewDistance = 3f;
    public float agentSpeed = 1.5f;
    public float playerDetectedAgentSpeed = 2f;
    public bool isPlayerDetected = false;
    public bool detectionEnded = false;
    public bool canAttackPlayer = false;
    
    private CheckDirection _direction = new CheckDirection();
    public Target playerTarget;

    private Rigidbody _robotRigidBody;
    private NavMeshAgent _robotNavMeshAgent;
    
    private Animator _robotAnimator;
    private void Awake()
    {
        _robotNavMeshAgent = GetComponent<NavMeshAgent>();
        _robotNavMeshAgent.autoBraking = false;
        _robotNavMeshAgent.speed = agentSpeed;

        _robotRigidBody = GetComponent<Rigidbody>();
        _robotAnimator = GetComponent<Animator>();
        var enemyDetector = gameObject.AddComponent<EnemyDetector>();
        
        _stateMachine = new StateMachine();

        var searchForTarget = new SearchForTarget(this); // State qui cherche le player avec un raycast
        var moveTowardTarget = new MoveTowardTarget(this, _robotNavMeshAgent, _robotAnimator);
        var idleSearch = new IdleSearch(this, _robotAnimator);
        var takesDamage = new TakesDamage(this, _robotAnimator);
        var attackPlayer = new AttackPlayer(_robotAnimator, _robotNavMeshAgent);
        var playerDetected = new PlayerDetected(_robotAnimator, _robotNavMeshAgent, this);

        AddTransition(idleSearch, searchForTarget, HasFinishedSearching());
        AddTransition(searchForTarget, moveTowardTarget, HasTarget());
        AddTransition(moveTowardTarget, idleSearch, HasReachedDestination());

        AddTransition(takesDamage, searchForTarget, HasLanded());
        
        AddTransition(attackPlayer, idleSearch, HasFinishedAttacking());
        AddTransition(attackPlayer, takesDamage, HasTakenDamage());
        
        AddAnyTransition(moveTowardTarget, HasDetectionEnded());
        AddAnyTransition(takesDamage, HasTakenDamage());
        AddAnyTransition(attackPlayer, IsAttackingPlayer());
        AddAnyTransition(playerDetected, IsPlayerDetected());
        
        _stateMachine.SetState(searchForTarget);
        
        void AddTransition(IState to, IState from, Func<bool> conditon) =>
            _stateMachine.AddTransition(to, from, conditon);

        void AddAnyTransition(IState to, Func<bool> condition) =>
            _stateMachine.AddAnyTransition(to, condition);

        Func<bool> HasTarget() => () => Target != null;
        Func<bool> HasDetectionEnded() => () => detectionEnded;
        Func<bool> HasReachedDestination() => () => !_robotNavMeshAgent.pathPending && _robotNavMeshAgent.remainingDistance < 0.5f;
        Func<bool> HasFinishedSearching() => () => !isSearching;
        Func<bool> HasTakenDamage() => () => isGettingAttacked;
        Func<bool> HasLanded() => () => isGrounded;
        Func<bool> IsAttackingPlayer() => () => isAttacking;
        Func<bool> HasFinishedAttacking() => () => !isAttacking;
        Func<bool> IsPlayerDetected() => () => isPlayerDetected;
    }
    
    private void Update() => _stateMachine.Tick();

    public void EnterTakesDamageState()
    {
        _robotRigidBody.isKinematic = false;
        _robotNavMeshAgent.enabled = false;
        isGettingAttacked = true;
    }
    public void ExitTakesDamageState()
    {
        _robotRigidBody.isKinematic = true;
        isGrounded = false;
        isGettingAttacked = false;
        _robotNavMeshAgent.enabled = true;
    }

    public void EnterAttackState(float xPlayerPosition)
    {
        playerPosition = xPlayerPosition; // pourra être utilisé depuis les autres components
        if (_direction.IsBetweenTargets(Target.transform, playerPosition, transform) || canAttackPlayer)
        {
            isAttacking = true;
        }

    }

    public void ExitAttackState() // est callé par l'animator à la fin de l'Attaque
    {
        isAttacking = false;
    }

    public void DetectionHasEnded()
    {
        detectionEnded = true;
    }

    void FixedUpdate()
    {
        if (isPlayerDetected || (Target != null && Target.gameObject.CompareTag("Player"))) { return; }
        int playerLayerMask = LayerMask.GetMask("PlayerCharacter");
        int environmentMask = LayerMask.GetMask("Default");

        Vector3 startingPosition = transform.position;
        startingPosition.y += 0.5f;
        
        RaycastHit hit;
        // si on pogne l'envionnement mais pas de joueur, cho bye
        if (Physics.Raycast(
                startingPosition,
                transform.TransformDirection(Vector3.left),
                out hit,
                viewDistance,
                environmentMask))
        {
            isPlayerDetected = false;
            return;
        }
        
        // Does the ray intersect with the player layer
        if (Physics.Raycast(
                startingPosition, 
                transform.TransformDirection(Vector3.left), 
                out hit, viewDistance, 
                playerLayerMask))
        {
            if (!_direction.IsGoingRight(transform, Target.transform)) return;
            playerTarget = hit.collider.GetComponent<Target>();
            isPlayerDetected = true;
            Debug.DrawRay(startingPosition, transform.TransformDirection(Vector3.left) * hit.distance, Color.yellow);
            return;
        }
        if (Physics.Raycast(
            startingPosition, 
            transform.TransformDirection(Vector3.right), 
            out hit, viewDistance, 
            playerLayerMask))
        {
            if (!_direction.IsGoingLeft(transform, Target.transform)) return;
            playerTarget = hit.collider.GetComponent<Target>();
            isPlayerDetected = true;
            Debug.DrawRay(startingPosition, transform.TransformDirection(Vector3.right) * hit.distance, Color.red);
            return;
        }
    }
}
