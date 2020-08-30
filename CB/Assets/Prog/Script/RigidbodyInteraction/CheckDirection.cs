using UnityEngine;

namespace Prog.Script.RigidbodyInteraction
{
    public class CheckDirection
    {
        private Transform _currentTranform;
        private Transform _oldTransform;

        public void SetTransform(Transform current, Transform old)
        {
            _currentTranform = current;
            _oldTransform = old;
        }
        
        public bool IsGoingLeft()
        {
            return !(_currentTranform.position.x > _oldTransform.position.x);
        }

        public bool IsGoingRight()
        {
            return (_currentTranform.position.x > _oldTransform.position.x);
        }
    }
}