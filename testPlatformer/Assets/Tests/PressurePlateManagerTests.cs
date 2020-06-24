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

        private ActivatorManager _activatorManager;
        
        GameObject PressurePlate1GameObject = new GameObject();
        GameObject PressurePlate2GameObject = new GameObject();
        private Activator _pressurePlate1;
        private Activator _pressurePlate2;

        private GameObject DoorObject = new GameObject();
        
        [SetUp]
        public void Setup()
        {
            PressurePlateManagerGameObject.AddComponent<ActivatorManager>();
            _activatorManager = PressurePlateManagerGameObject.GetComponent<ActivatorManager>();

            DoorObject.AddComponent<Door>();
            _activatorManager.SetDoor(DoorObject.GetComponent<Door>());
            DoorObject.GetComponent<Door>().DoorTransform = DoorObject.transform;

            PressurePlate1GameObject.AddComponent<Activator>();
            _pressurePlate1 = PressurePlate1GameObject.GetComponent<Activator>();
            _pressurePlate1.name = "pressure1";
            
            PressurePlate2GameObject.AddComponent<Activator>();
            _pressurePlate2 = PressurePlate2GameObject.GetComponent<Activator>();
            _pressurePlate2.name = "pressure2";
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_activatorManager);
            Object.DestroyImmediate(_pressurePlate1);
            Object.DestroyImmediate(_pressurePlate2);
        }

        [Test]
        public void check_that_proper_init_when_no_pressurePlate_are_assigned()
        {
            Assert.True(_activatorManager.GetNbOfActivator() == 0);
        }
        
        [Test]
        public void add_pressurePlate_and_check_it_is_added() // Check added pressure plate is there
        {
            Assert.True(_activatorManager.GetNbOfActivator() == 0);
            _activatorManager.AddActivator(_pressurePlate1);
            Assert.True(_activatorManager.GetNbOfActivator() == 1);
        }

        [Test] 
        public void press_on_plate_and_check_it_is_pressed() // Check Pressed pressure plate is activated
        {
            _activatorManager.AddActivator(_pressurePlate1);
            _activatorManager.EnableActivator(_pressurePlate1.name);
            Assert.True(_activatorManager.CheckActivatorState(_pressurePlate1.name));
        }

        [Test]
        public void depress_plate_and_check_it_is_released() // Check Depressed pressure plate is desactivated
        {
            _activatorManager.AddActivator(_pressurePlate1);
            _activatorManager.DisableActivator(_pressurePlate1.name);
            Assert.False(_activatorManager.CheckActivatorState(_pressurePlate1.name));
        }

        [Test] // Check that when not all when pressure plate are pressed, door not opening
        public void check_that_when_not_all_pressurePlate_are_pressed_Door_not_opening()
        {
            _activatorManager.AddActivator(_pressurePlate1);
            _activatorManager.AddActivator(_pressurePlate2);
            _activatorManager.EnableActivator(_pressurePlate1.name);
            Assert.False(_activatorManager.GetActivationStateOfManager());
        }

        [Test] // Check that when all plates are activated, door is opening 
        public void check_that_when_all_pressurePlate_are_pressed_Door_is_opening()
        {
            _activatorManager.AddActivator(_pressurePlate1);
            _activatorManager.AddActivator(_pressurePlate2);
            _activatorManager.EnableActivator(_pressurePlate1.name);
            _activatorManager.EnableActivator(_pressurePlate2.name);
            Assert.True(_activatorManager.GetActivationStateOfManager());
        }
    }
}
