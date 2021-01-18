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

        private bool _isCheckingForGround = false;

        private float WAIT_TIME_BEFORE_CHECKING_GROUND = 0.5f;

        public TakesDamage( BasicRobotBehavior robot, Animator animator)
        {
            _animator = animator;
            _robot = robot;
        }
        
        //test jira

        public void Tick()
        {
            if (!_isCheckingForGround) return; // on veut pas qu'il commence a checker le ground trop vite
            
            _robot.isGrounded = Physics.CheckSphere(_robot.transform.position, _robot.groundCheckerRadius, _robot.groundLayerMask,
                QueryTriggerInteraction.Ignore);
            if (_robot.isGrounded) { _robot.isGettingAttacked = false; }
        }

        public void OnEnter()
        {
            // Debug.Log("Enter Takes damage");
            _robot.EnterTakesDamageState();
            _robot.StartCoroutine(WaitBeforeCheckingForGround());
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
            yield return new WaitForSeconds(WAIT_TIME_BEFORE_CHECKING_GROUND);
            _isCheckingForGround = true;
        }

        public void OnExit()
        {
            // Debug.Log("exit takes damage");
            _robot.ExitTakesDamageState();
            _isCheckingForGround = false;
        }
    }
}