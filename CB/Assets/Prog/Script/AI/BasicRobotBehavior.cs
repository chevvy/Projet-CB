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
    private CheckDirection _direction = new CheckDirection();

    private Rigidbody _robotRigidBody;
    private NavMeshAgent _robotNavMeshAgent;
    
    private Animator _robotAnimator;
    private static readonly int Attack = Animator.StringToHash("Attack");
    private void Awake()
    {
        _robotNavMeshAgent = GetComponent<NavMeshAgent>();
        _robotNavMeshAgent.autoBraking = false;

        _robotRigidBody = GetComponent<Rigidbody>();
        _robotAnimator = GetComponent<Animator>();
        var enemyDetector = gameObject.AddComponent<EnemyDetector>();
        
        _stateMachine = new StateMachine();

        var searchForTarget = new SearchForTarget(this); // State qui cherche le player avec un raycast
        var moveTowardTarget = new MoveTowardTarget(this, _robotNavMeshAgent, _robotAnimator);
        var idleSearch = new IdleSearch(this, _robotAnimator);
        var takesDamage = new TakesDamage(this, _robotAnimator);
        var attackPlayer = new AttackPlayer(_robotAnimator, _robotNavMeshAgent);

        AddTransition(idleSearch, searchForTarget, HasFinishedSearching());
        AddTransition(searchForTarget, moveTowardTarget, HasTarget());
        AddTransition(moveTowardTarget, idleSearch, HasReachedDestination());

        AddTransition(takesDamage, searchForTarget, HasLanded());
        AddAnyTransition(takesDamage, HasTakenDamage());
        
        AddAnyTransition(attackPlayer, IsAttackingPlayer());
        AddTransition(attackPlayer, idleSearch, HasFinishedAttacking());

        _stateMachine.SetState(searchForTarget);
        
        void AddTransition(IState to, IState from, Func<bool> conditon) =>
            _stateMachine.AddTransition(to, from, conditon);

        void AddAnyTransition(IState to, Func<bool> condition) =>
            _stateMachine.AddAnyTransition(to, condition);

        Func<bool> HasTarget() => () => Target != null;
        Func<bool> HasReachedDestination() => () => !_robotNavMeshAgent.pathPending && _robotNavMeshAgent.remainingDistance < 0.5f;
        Func<bool> HasFinishedSearching() => () => !isSearching;
        Func<bool> HasTakenDamage() => () => isGettingAttacked;
        Func<bool> HasLanded() => () => isGrounded;
        Func<bool> IsAttackingPlayer() => () => isAttacking;
        Func<bool> HasFinishedAttacking() => () => !isAttacking;
    }
    
    private void Update() => _stateMachine.Tick();

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, groundCheckerRadius);
    }

    public void EnterTakesDamageState()
    {
        _robotRigidBody.isKinematic = false;
        _robotNavMeshAgent.enabled = false;
        isGettingAttacked = false;
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
        isAttacking = true;
        Debug.Log("Is attacking");
    }

    public void ExitAttackState() // est callé par l'animator à la fin de l'Attaque
    {
        isAttacking = false;
    }
}
