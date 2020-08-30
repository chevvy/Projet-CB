using System;
using System.Collections;
using System.Collections.Generic;
using Prog.Script;
using Prog.Script.AI;
using UnityEngine;
using UnityEngine.AI;

public class BasicRobotBehavior : MonoBehaviour
{
    private StateMachine _stateMachine;
    
    public Target Target { get; set; }
    public Target[] patrolPoints;

    private void Awake()
    {
        var navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.autoBraking = false;
        
        var animator = GetComponent<Animator>();
        var enemyDetector = gameObject.AddComponent<EnemyDetector>();
        
        _stateMachine = new StateMachine();

        var search = new SearchForTarget(this); // State qui cherche le player avec un raycast
        var moving = new MoveTowardTarget(this, navMeshAgent, animator);
        //var alerted = new AlertedRobot() // State qui vient de détecter le player
        //var moving = new MovingTowardEnemy() // State lorsque l'enemy va vers le player
        //var attacking = new RobotAttacking() // State lorsque le robot attaque le player
        //var attacked = new  RobotGettingAttacked() // State lorsque le robot recoit une attaque

        At(search, moving, HasTarget());
        At(moving, search, HasReachedDestination());
        
        // add anyTransitions pour des escape state
        
        _stateMachine.SetState(search);
        
        void At(IState to, IState from, Func<bool> conditon) =>
            _stateMachine.AddTransition(to, from, conditon);

        Func<bool> HasTarget() => () => Target != null;
        Func<bool> HasReachedDestination() => () => !navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f;
    }
    
    private void Update() => _stateMachine.Tick();
}
