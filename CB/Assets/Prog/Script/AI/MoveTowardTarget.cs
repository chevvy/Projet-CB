using Prog.Script.RigidbodyInteraction;
using UnityEngine;
using UnityEngine.AI;

namespace Prog.Script.AI
{
    public class MoveTowardTarget : IState
    {
        private readonly BasicRobotBehavior _robot;
        private readonly NavMeshAgent _navMeshAgent;

        private readonly Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int MoveLeft = Animator.StringToHash("Walk_L");
        private static readonly int MoveRight = Animator.StringToHash("Walk_R");
        
        private readonly CheckDirection _direction = new CheckDirection();

        public MoveTowardTarget(BasicRobotBehavior robot, NavMeshAgent navMeshAgent, Animator animator)
        {
            _robot = robot;
            _navMeshAgent = navMeshAgent;
            _animator = animator;
        }


        public void Tick() {}

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(_robot.Target.transform.position);
            SetAnimationRelativeToDirection();
        }

        private void SetAnimationRelativeToDirection()
        {
            _animator.SetBool(_direction.IsGoingLeft(_robot.transform, _robot.Target.transform) ? MoveLeft : MoveRight, true);
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = false;
        }
    }
}