using System;
using System.Collections;
using UnityEngine;

namespace Prog.Script.Environnement
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private Transform door;
        [SerializeField] private float maxOpeningHeight = 20;
        [SerializeField] private float openingSpeed = 1;

        private float _initialPositionY;
        
        // à tester : si transform pas assigné -> throw, si modif transform, nouvelle position bonne
    
        void Start()
        {
            _initialPositionY = door.position.y;
        }

        public void OpenDoor()
        {
            StartCoroutine(OpenDoorCoroutine());
        }

        private IEnumerator OpenDoorCoroutine()
        {
            // stop coroutine closeDoor 
            while (door.position.y <= maxOpeningHeight + _initialPositionY)
            {
                door.position += new Vector3(0, openingSpeed, 0);
                yield return null;
            }
        }

        public void CloseDoor()
        {
         StopCoroutine(OpenDoorCoroutine());
         StartCoroutine(CloseDoorCoroutine());
        }

        private IEnumerator CloseDoorCoroutine()
        {
            while (door.position.y > _initialPositionY)
            {
                door.position -= new Vector3(0, openingSpeed, 0);
                yield return null;
            }
        }
    }
}
