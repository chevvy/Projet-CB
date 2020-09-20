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

        AddTransition(idleSearch, searchForTarget, HasFinishedSearching());
        AddTransition(searchForTarget, moveTowardTarget, HasTarget());
        AddTransition(moveTowardTarget, idleSearch, HasReachedDestination());

        AddTransition(takesDamage, searchForTarget, HasLanded());
        AddAnyTransition(takesDamage, HasTakenDamage());

        _stateMachine.SetState(searchForTarget);
        
        void AddTransition(IState to, IState from, Func<bool> conditon) =>
            _stateMachine.AddTransition(to, from, conditon);

        void AddAnyTransition(IState to, Func<bool> condition) =>
            _stateMachine.AddAnyTransition(to, condition);

        Func<bool> HasTarget() => () => Target != null;
        Func<bool> HasReachedDestination() => () => !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f;
        Func<bool> HasFinishedSearching() => () => !isSearching;
        Func<bool> HasTakenDamage() => () => isGettingAttacked;
        Func<bool> HasLanded() => () => isGrounded;
    }
    
    private void Update() => _stateMachine.Tick();
}
