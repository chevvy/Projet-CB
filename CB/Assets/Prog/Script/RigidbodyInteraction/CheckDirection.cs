using UnityEngine;

namespace Prog.Script.RigidbodyInteraction
{
    public class CheckDirection
    {
        public bool IsGoingLeft(Transform current, Transform target)
        {
            var movementDirection = target.transform.position.x - current.transform.position.x;
            return (movementDirection < 0);
        }
        
        public bool IsGoingLeft(Transform current, float target)
        {
            var movementDirection = target - current.transform.position.x;
            return (movementDirection < 0);
        }

        public bool IsGoingRight(Transform current, Transform target)
        {
            var movementDirection = target.transform.position.x - current.transform.position.x;
            return (movementDirection > 0);
        }

        public bool IsBetweenTargets(Transform target1, float currentXPosition, Transform target2)
        {
            var target1Position = target1.transform.position.x;
            var target2Position = target2.transform.position.x;
            return ((target1Position < currentXPosition && currentXPosition < target2Position) ||
                    (target2Position < currentXPosition && currentXPosition < target1Position));
        }
    }
}