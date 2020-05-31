using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Prog.Script.Environnement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PressurePlateManagerTests
    {
        GameObject PressurePlateManagerGameObject = new GameObject();

        private PressurePlateManager _pressurePlateManager;
        
        GameObject PressurePlate1GameObject = new GameObject();
        GameObject PressurePlate2GameObject = new GameObject();
        private PressurePlate _pressurePlate1;
        private PressurePlate _pressurePlate2;

        private GameObject DoorObject = new GameObject();
        
        [SetUp]
        public void Setup()
        {
            PressurePlateManagerGameObject.AddComponent<PressurePlateManager>();
            _pressurePlateManager = PressurePlateManagerGameObject.GetComponent<PressurePlateManager>();

            DoorObject.AddComponent<Door>();
            _pressurePlateManager.SetDoor(DoorObject.GetComponent<Door>());
            DoorObject.GetComponent<Door>().DoorTransform = DoorObject.transform;

            PressurePlate1GameObject.AddComponent<PressurePlate>();
            _pressurePlate1 = PressurePlate1GameObject.GetComponent<PressurePlate>();
            _pressurePlate1.name = "pressure1";
            
            PressurePlate2GameObject.AddComponent<PressurePlate>();
            _pressurePlate2 = PressurePlate2GameObject.GetComponent<PressurePlate>();
            _pressurePlate2.name = "pressure2";
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_pressurePlateManager);
            Object.DestroyImmediate(_pressurePlate1);
            Object.DestroyImmediate(_pressurePlate2);
        }

        [Test]
        public void check_that_proper_init_when_no_pressurePlate_are_assigned()
        {
            Assert.True(_pressurePlateManager.GetNbOfPressurePlates() == 0);
        }
        
        [Test]
        public void add_pressurePlate_and_check_it_is_added() // Check added pressure plate is there
        {
            Assert.True(_pressurePlateManager.GetNbOfPressurePlates() == 0);
            _pressurePlateManager.AddPressurePlate(_pressurePlate1);
            Assert.True(_pressurePlateManager.GetNbOfPressurePlates() == 1);
        }

        [Test] 
        public void press_on_plate_and_check_it_is_pressed() // Check Pressed pressure plate is activated
        {
            _pressurePlateManager.AddPressurePlate(_pressurePlate1);
            _pressurePlateManager.PressurePlateIsPressed(_pressurePlate1.name);
            Assert.True(_pressurePlateManager.CheckPressurePlateState(_pressurePlate1.name));
        }

        [Test]
        public void depress_plate_and_check_it_is_released() // Check Depressed pressure plate is desactivated
        {
            _pressurePlateManager.AddPressurePlate(_pressurePlate1);
            _pressurePlateManager.PresurePlateIsReleased(_pressurePlate1.name);
            Assert.False(_pressurePlateManager.CheckPressurePlateState(_pressurePlate1.name));
        }

        [Test] // Check that when not all when pressure plate are pressed, door not opening
        public void check_that_when_not_all_pressurePlate_are_pressed_Door_not_opening()
        {
            _pressurePlateManager.AddPressurePlate(_pressurePlate1);
            _pressurePlateManager.AddPressurePlate(_pressurePlate2);
            _pressurePlateManager.PressurePlateIsPressed(_pressurePlate1.name);
            Assert.False(_pressurePlateManager.GetActivationStateOfManager());
        }

        [Test] // Check that when all plates are activated, door is opening 
        public void check_that_when_all_pressurePlate_are_pressed_Door_is_opening()
        {
            _pressurePlateManager.AddPressurePlate(_pressurePlate1);
            _pressurePlateManager.AddPressurePlate(_pressurePlate2);
            _pressurePlateManager.PressurePlateIsPressed(_pressurePlate1.name);
            _pressurePlateManager.PressurePlateIsPressed(_pressurePlate2.name);
            Assert.True(_pressurePlateManager.GetActivationStateOfManager());
        }
    }
}
