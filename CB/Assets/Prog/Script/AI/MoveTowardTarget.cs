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

        public MoveTowardTarget(BasicRobotBehavior robot, NavMeshAgent navMeshAgent, Animator animator)
        {
            _robot = robot;
            _navMeshAgent = navMeshAgent;
            _animator = animator;
        }


        public void Tick()
        {
            // Move vers l'enemi
            // animator move
        }

        public void OnEnter()
        {
            _navMeshAgent.enabled = true;
            _navMeshAgent.SetDestination(_robot.Target.transform.position);
        }

        public void OnExit()
        {
            _navMeshAgent.enabled = false;
        }
    }
}