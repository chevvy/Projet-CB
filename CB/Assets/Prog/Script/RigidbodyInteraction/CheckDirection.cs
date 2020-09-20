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
    }
}