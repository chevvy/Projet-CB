using Prog.Script.RigidbodyInteraction;
using UnityEngine;
using UnityEngine.AI;

namespace Prog.Script.AI
{
    public class MoveTowardEnemy : IState
    {
        private readonly BasicRobotBehavior _robot;
        private readonly NavMeshAgent _navMeshAgent;

        private readonly Animator _animator;
        private static readonly int MoveLeft = Animator.StringToHash("Walk_L");
        private static readonly int MoveRight = Animator.StringToHash("Walk_R");
        
        private readonly CheckDirection _direction = new CheckDirection();

        public MoveTowardEnemy(BasicRobotBehavior robot, NavMeshAgent navMeshAgent, Animator animator)
        {
            _robot = robot;
            _navMeshAgent = navMeshAgent;
            _animator = animator;
        }
        
        public void Tick()
        {

        }

        public void OnEnter()
        {
            Debug.Log("Entering moveTowardEnemy");
            _navMeshAgent.enabled = true;
            SetAnimationRelativeToDirection();
            // On augmente la vitesse du navmeshagent
        }
        
        private void SetAnimationRelativeToDirection()
        {
            _animator.SetBool(_direction.IsGoingLeft(_robot.transform, _robot.playerTransform.transform) ? MoveLeft : MoveRight, true);
        }

        public void OnExit()
        {
            _robot.isMovingTowardEnemy = false;
            Debug.Log("exiting moveTowardEnemy");
        }
    }
}