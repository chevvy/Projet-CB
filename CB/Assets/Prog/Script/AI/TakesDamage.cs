using System;
using System.Collections;
using Prog.Script.RigidbodyInteraction;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

namespace Prog.Script.AI
{
    public class TakesDamage : IState
    {
        private readonly Animator _animator;
        private static readonly int HitStunLeft = Animator.StringToHash("HitStun_L");
        private static readonly int HitStunRight = Animator.StringToHash("HitStun_R");

        private readonly BasicRobotBehavior _robot;
        private readonly CheckDirection _direction = new CheckDirection();
        private readonly NavMeshAgent _navMeshAgent;

        private bool _isCheckingForGround = false;

        public TakesDamage( BasicRobotBehavior robot, Animator animator, NavMeshAgent agent)
        {
            _animator = animator;
            _robot = robot;
            _navMeshAgent = agent;
        }

        public void Tick()
        {
            if (!_isCheckingForGround) return; // on veut pas qu'il commence a checker le ground trop vite
            
            _robot.isGrounded = Physics.CheckSphere(_robot.transform.position, _robot.groundCheckerRadius, _robot.groundLayerMask,
                QueryTriggerInteraction.Ignore);
            if (_robot.isGrounded) _robot.isGettingAttacked = false;
        }

        public void OnEnter()
        {
            _navMeshAgent.enabled = false;
            _robot.StartCoroutine(WaitBeforeCheckingForGround());
            _robot.isGettingAttacked = false;
            SetAnimationRelativeToDirection();
        }

        private void SetAnimationRelativeToDirection()
        {
            _animator.SetBool(
                _direction.IsGoingLeft(_robot.transform, _robot.playerPosition) ? 
                    HitStunLeft : HitStunRight, true
            );
        }

        IEnumerator WaitBeforeCheckingForGround()
        {
            yield return new WaitForSeconds(0.4f);
            _isCheckingForGround = true;
        }

        public void OnExit()
        {
            _robot.isGrounded = false;
            _robot.isGettingAttacked = false;
            _isCheckingForGround = false;
            _navMeshAgent.enabled = true;
        }
    }
}