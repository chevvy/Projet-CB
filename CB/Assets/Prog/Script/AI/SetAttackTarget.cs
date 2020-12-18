using UnityEngine;
using UnityEngine.AI;

namespace Prog.Script.AI
{
    public class SetAttackTarget : IState
    {
        private readonly BasicRobotBehavior _robot;
        private readonly NavMeshAgent _navMeshAgent;

        public SetAttackTarget(BasicRobotBehavior robot, NavMeshAgent navMeshAgent)
        {
            _robot = robot;
            _navMeshAgent = navMeshAgent;
        }
        public void Tick() {}

        public void OnEnter()
        {
            Debug.Log("entering SetAttackTarget");
            _robot.isInAttackState = true;
            // Set anim surprise 
            _navMeshAgent.SetDestination(_robot.playerTransform.position);
            _robot.isMovingTowardEnemy = true;
        }

        public void OnExit()
        {
            _robot.isInAttackState = false;
            _navMeshAgent.enabled = false;
            Debug.Log("exiting SetAttackTarget");
        }
    }
}