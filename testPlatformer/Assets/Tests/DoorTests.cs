using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Prog.Script.Environnement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DoorTests
    {
        GameObject doorInstance = new GameObject();
        
        [SetUp]
        public void Setup()
        {
            doorInstance.AddComponent<Door>();
            doorInstance.GetComponent<Door>().DoorTransform = doorInstance.transform;
        }

        [Test]
        public void Check_For_Valid_Instance_Of_Components()
        {
            Assert.True(doorInstance.TryGetComponent(out Door door));
        }

        [UnityTest]
        public IEnumerator Check_That_Door_Is_Opening_When_OpenDoor_Is_Called()
        {
            var doorComponent = doorInstance.GetComponent<Door>();
            var transform = doorComponent.transform;
            
            var positionBeforeOpen = transform.position.y;
            doorComponent.OpenDoor();

            yield return null;
            Assert.Less(positionBeforeOpen, transform.position.y);
        }

        [UnityTest]
        public IEnumerator Check_That_Door_Is_Closing_When_CloseDoor_Is_Called()
        {
            var doorComponent = doorInstance.GetComponent<Door>();
            var transform = doorComponent.transform;
            
            doorComponent.OpenDoor();
            
            yield return null;
            var positionAfterOpen = transform.position.y;
            doorComponent.CloseDoor();
            yield return null;
            Assert.Greater(positionAfterOpen, doorComponent.transform.position.y);
        }
    }
}
