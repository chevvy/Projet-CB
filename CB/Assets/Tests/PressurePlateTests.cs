using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PressurePlateTests
    {
        private IActivatorManager _activatorManager;
        private GameObject pressurePlateObject = new GameObject();
        private Activator _activator; 
        
        [SetUp]
        public void SetUp()
        {
            _activatorManager = Substitute.For<IActivatorManager>();
            pressurePlateObject.AddComponent<Activator>();

            _activator = pressurePlateObject.GetComponent<Activator>();
            _activator.name = "test";
            _activator.ActivatorManager = _activatorManager;
        }

        [Test]
        public void check_that_pressurePlate_is_disabled_by_default()
        {
            Assert.False(_activator.isPressed);
        }

        [Test]
        public void check_that_when_Activate_activates_pressurePlate()
        {
            _activator.EnableActivator();
            
            Assert.True(_activator.isPressed);
        }
        
        [Test]
        public void check_that_when_Disable_disables_pressurePlate()
        {
            _activator.DisableActivator();
            
            Assert.False(_activator.isPressed);
        }
    }
}
