using System.Collections;
using Prog.Script.RigidbodyInteraction;
using UnityEngine;

namespace Prog.Script.AI
{
    public class IdleSearch : IState
    {
        private readonly Animator _animator;
        private static readonly int IdleLeft = Animator.StringToHash("Idle_L");
        private static readonly int IdleRight = Animator.StringToHash("Idle_R");

        private readonly BasicRobotBehavior _robot;
        private readonly CheckDirection _direction = new CheckDirection();

        public IdleSearch( BasicRobotBehavior robot, Animator animator)
        {
            _animator = animator;
            _robot = robot;
        }
        public void Tick() {}

        public void OnEnter()
        {
            _robot.isSearching = true;
            _robot.StartCoroutine(SearchForTarget());
        }

        IEnumerator SearchForTarget()
        {
            SetAnimationRelativeToDirection();
            yield return new WaitForSeconds(2);
            _robot.isSearching = false;
        }

        private void SetAnimationRelativeToDirection()
        {
            
            _animator.SetBool(
                _direction.IsGoingLeft(_robot.transform, _robot.Target.transform) ? 
                    IdleLeft : IdleRight, true
            );
        }

        public void OnExit()
        {
            _robot.isSearching = false;
        }
    }
}