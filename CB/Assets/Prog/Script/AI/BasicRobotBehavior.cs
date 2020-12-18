using System;
using Prog.Script;
using Prog.Script.AI;
using UnityEngine;
using UnityEngine.AI;

public class BasicRobotBehavior : MonoBehaviour
{
    private StateMachine _stateMachine;
    
    public Target Target { get; set; }
    public Target[] patrolPoints;
    public LayerMask groundLayerMask;
    public bool isSearching = false;
    public bool isGrounded;
    public bool isGettingAttacked;
    public float groundCheckerRadius = 1f;
    public float playerPosition;
    
    public bool playerDetected = false;
    public bool isInAttackState = false;
    public bool isMovingTowardEnemy = false;
    public Transform playerTransform;
    private void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.autoBraking = false;
        
        var animator = GetComponent<Animator>();
        var enemyDetector = gameObject.AddComponent<EnemyDetector>();
        
        _stateMachine = new StateMachine();

        var searchForTarget = new SearchForTarget(this); // State qui cherche le player avec un raycast
        var moveTowardTarget = new MoveTowardTarget(this, navMeshAgent, animator);
        var idleSearch = new IdleSearch(this, animator);
        var takesDamage = new TakesDamage(this, animator, navMeshAgent);
        var setAttackTarget = new SetAttackTarget(this, navMeshAgent);
        var moveTowardEnemy = new MoveTowardEnemy(this, navMeshAgent, animator);
        var attackEnemy = new AttackEnemy();

        AddTransition(idleSearch, searchForTarget, HasFinishedSearching());
        AddTransition(searchForTarget, moveTowardTarget, HasTarget());
        AddTransition(moveTowardTarget, idleSearch, HasReachedDestination());
        
        AddTransition(takesDamage, searchForTarget, HasLanded());
        AddAnyTransition(takesDamage, HasTakenDamage());
        
        // en tout temps, on détecte pour voir si on trouve un joueur. Si c'est le cas, on va en SetAttackTarget
        // Dans l'attack state, on set le navmeshAgent pour qu'il se déplace vers le joueur
        // et on met l'anim de surprise ? 
        // on set à "movingTowardtarget"
        AddAnyTransition(setAttackTarget, HasDetectedPlayer());
        // lorsqu'on commence à se déplacer, on se déplace vers l'ennemi et on va dans le state
        // Dans le state, on fait animer dans la bonne direction l'ennemi (et on le fait aller plus vite)
        AddTransition(setAttackTarget, moveTowardEnemy, IsMovingTowardEnemy());
        // Si on est en marche d'attaque et qu'on atteint la target, on passe en attackEnemy
        AddTransition(moveTowardEnemy, attackEnemy, HasReachedAttackTarget());
        // Si le player est rendu trop loin (<3f), on move toward enemy
        AddTransition(attackEnemy, moveTowardEnemy, AttackTargetIsTooFarButStillInReach());
        // Si le player est vraiment trop loin, on passe à la recherche de target
        AddTransition(attackEnemy, searchForTarget, AttackTargetIsWayTooFar());
        // mais si c'est à cause des dommages reçu, ça devrait passer searchForTarget par la transition du takesDamage
        
        
        
        

        _stateMachine.SetState(searchForTarget);
        
        void AddTransition(IState to, IState from, Func<bool> conditon) =>
            _stateMachine.AddTransition(to, from, conditon);

        void AddAnyTransition(IState to, Func<bool> condition) =>
            _stateMachine.AddAnyTransition(to, condition);

        Func<bool> HasTarget() => () => Target != null;
        Func<bool> HasReachedDestination() => () => !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f && !isInAttackState;
        Func<bool> HasReachedAttackTarget() => () => (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f) && isInAttackState;
        Func<bool> AttackTargetIsTooFarButStillInReach() => () => (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= 3f);
        Func<bool> AttackTargetIsWayTooFar() => () => (!navMeshAgent.pathPending && navMeshAgent.remainingDistance > 3f);
        Func<bool> HasFinishedSearching() => () => !isSearching;
        Func<bool> HasTakenDamage() => () => isGettingAttacked;
        Func<bool> HasLanded() => () => isGrounded;
        Func<bool> HasDetectedPlayer() => () => playerDetected && !isInAttackState;
        Func<bool> HasAttackTarget() => () => playerTransform != null;
        Func<bool> IsMovingTowardEnemy() => () => isMovingTowardEnemy;
    }
    
    private void Update() => _stateMachine.Tick();

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, groundCheckerRadius);
    }
    void FixedUpdate()
    {
        // Si le player est déjà detecté et on est en attack, pas besoin de faire des checkups, ça veut dire qu'on est déjà en attack state
        // if (playerDetected && isInAttackState)
        // {
        //     return;
        // }
        
        var newTransform = transform.position;
        newTransform.y = transform.position.y + 0.25f;
        RaycastHit hit;
        // If the ray intersects with a wall ou autre, on return car on ne peut détecter le joueur
        // if(Physics.Raycast(newTransform, transform.TransformDirection(Vector3.forward), out hit, 10, 12))
        // {
        //     return;
        // }
        
        // If on detecte un joueur, on enclenche la routine d'attaque
        if (Physics.Raycast(newTransform, transform.TransformDirection(Vector3.forward), out hit, 10, 9))
        {
            if (hit.collider.CompareTag("Player"))
            {
                playerTransform = hit.collider.transform;
                playerDetected = true;
            }
        }
        else
        {
            // Si on détecte pas le player, on s'assure de retourner en mode recherche, si c'est pas déjà le cas
            playerDetected = false;
            isInAttackState = false;
        }
        
      
        
        
        // DEBUG DEBUG 
        // TODO à enlever
        if (Physics.Raycast(newTransform, transform.TransformDirection(Vector3.right), out hit))
        {
            Debug.DrawRay(newTransform, transform.TransformDirection(Vector3.right) * hit.distance, Color.yellow);
            Debug.Log("Did Hit" + hit.collider.name);
        }
        else
        {
            Debug.DrawRay(newTransform, transform.TransformDirection(Vector3.right) * 1000, Color.white);
        }
    }
}
