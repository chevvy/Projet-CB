using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Prog.Script.Environnement;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ActivatorManager
    {
        private readonly GameObject _activatorManagerGameObject = new GameObject();

        private global::ActivatorManager _activatorManager;

        private readonly GameObject _activator1GameObject = new GameObject();
        private readonly GameObject _activator2GameObject = new GameObject();
        private Activator _activator1;
        private Activator _activator2;

        private readonly GameObject _doorObject = new GameObject();
        
        [SetUp]
        public void Setup()
        {
            _activatorManagerGameObject.AddComponent<global::ActivatorManager>();
            _activatorManagerGameObject.AddComponent<Animator>(); // on vient les ajouter comme ça car pas capable de mock animator
            
            _activatorManager = _activatorManagerGameObject.GetComponent<global::ActivatorManager>();

            _activatorManager.animationParam = "test";
            _activatorManager.SetAnimator(_activatorManagerGameObject.GetComponent<Animator>());

            _doorObject.AddComponent<Door>();
            _activatorManager.SetDoor(_doorObject.GetComponent<Door>());
            _doorObject.GetComponent<Door>().DoorTransform = _doorObject.transform;

            _activator1GameObject.AddComponent<Activator>();
            _activator1 = _activator1GameObject.GetComponent<Activator>();
            _activator1.name = "pressure1";
            
            _activator2GameObject.AddComponent<Activator>();
            _activator2 = _activator2GameObject.GetComponent<Activator>();
            _activator2.name = "pressure2";
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_activatorManager);
            Object.DestroyImmediate(_activator1);
            Object.DestroyImmediate(_activator2);
        }

        [Test]
        public void check_that_proper_init_when_no_Activators_are_assigned()
        {
            Assert.True(_activatorManager.GetNbOfActivator() == 0);
        }
        
        [Test]
        public void add_activator_and_check_it_is_added() // Check added pressure plate is there
        {
            Assert.True(_activatorManager.GetNbOfActivator() == 0);
            _activatorManager.AddActivator(_activator1);
            Assert.True(_activatorManager.GetNbOfActivator() == 1);
        }

        [Test] 
        public void enable_activator_and_check_it_is_pressed() // Check Pressed pressure plate is activated
        {
            _activatorManager.AddActivator(_activator1);
            _activatorManager.EnableActivator(_activator1.name);
            Assert.True(_activatorManager.CheckActivatorState(_activator1.name));
        }

        [Test]
        public void disable_activator_and_check_it_is_disabled() // Check Depressed pressure plate is desactivated
        {
            _activatorManager.AddActivator(_activator1);
            _activatorManager.DisableActivator(_activator1.name);
            Assert.False(_activatorManager.CheckActivatorState(_activator1.name));
        }

        [Test] // Check that when not all when pressure plate are pressed, door not opening
        public void check_that_when_not_all_activators_are_pressed_Door_not_opening()
        {
            _activatorManager.AddActivator(_activator1);
            _activatorManager.AddActivator(_activator2);
            _activatorManager.EnableActivator(_activator1.name);
            Assert.False(_activatorManager.GetActivationStateOfManager());
        }

        [Test] // Check that when all plates are activated, door is opening 
        public void check_that_when_all_activators_are_enabled_Door_is_opening()
        {
            _activatorManager.AddActivator(_activator1);
            _activatorManager.AddActivator(_activator2);
            _activatorManager.EnableActivator(_activator1.name);
            _activatorManager.EnableActivator(_activator2.name);
            Assert.True(_activatorManager.GetActivationStateOfManager());
        }
    }
}
