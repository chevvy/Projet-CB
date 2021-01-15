using Prog.Script.RigidbodyInteraction;
using UnityEngine;
using UnityEngine.AI;

namespace Prog.Script.AI
{
    public class PlayerDetected : IState
    {
        private Animator _robotAnimator;
        private NavMeshAgent _robotNavMeshAgent;
        private BasicRobotBehavior _robot;
        private static readonly int SupriseLeft = Animator.StringToHash("Surprise_L");
        private static readonly int SupriseRight = Animator.StringToHash("Surprise_R");
        private readonly CheckDirection _direction = new CheckDirection();
        
        public PlayerDetected(Animator animator, NavMeshAgent navMeshAgent, BasicRobotBehavior robot)
        {
            _robotAnimator = animator;
            _robotNavMeshAgent = navMeshAgent;
            _robot = robot;
        }
        public void Tick() { }

        public void OnEnter()
        {
            _robot.isPlayerDetected = false;
            // Debug.Log("Enter player detected");
            _robotNavMeshAgent.enabled = false; // stop agent 
            _robot.Target = _robot.playerTarget;  // remove target & set target player
            SetAnimationRelativeToDirection();// trigger anim de surprise
            // _robot.detectionEnded = true;
        }
        
        private void SetAnimationRelativeToDirection()
        {
            _robotAnimator.SetTrigger(_direction.IsGoingLeft(_robot.transform, _robot.Target.transform) ? SupriseLeft : SupriseRight);
        }

        public void OnExit()
        {
            // Debug.Log("exit player detected");
            _robotNavMeshAgent.enabled = true;
            _robot.isPlayerDetected = false;
            _robot.detectionEnded = false;
            _robot.canAttackPlayer = true;
            _robotNavMeshAgent.speed = _robot.playerDetectedAgentSpeed;
        }
    }
}