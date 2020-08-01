using System;
using System.Collections;
using UnityEngine;

namespace Prog.Script.Environnement
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private float maxOpeningHeight = 20;
        [SerializeField] private float openingSpeed = 1;

        public Transform DoorTransform { get; set; }
        
        private float _initialPositionY;
        
        // à tester : si transform pas assigné -> throw, si modif transform, nouvelle position bonne
    
        void Start()
        {
            DoorTransform = GetComponent<Transform>();
            _initialPositionY = DoorTransform.position.y;
        }

        public void OpenDoor()
        {
            StopCoroutine(CloseDoorCoroutine());
            StartCoroutine(OpenDoorCoroutine());
        }

        private IEnumerator OpenDoorCoroutine()
        {
            while (DoorTransform.position.y <= maxOpeningHeight + _initialPositionY)
            {
                DoorTransform.position += new Vector3(0, openingSpeed, 0);
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
            while (DoorTransform.position.y > _initialPositionY)
            {
                DoorTransform.position -= new Vector3(0, openingSpeed, 0);
                yield return null;
            }
        }
    }
}
