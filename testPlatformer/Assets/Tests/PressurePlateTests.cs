using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NSubstitute;

namespace Tests
{
    public class PressurePlateTests
    {
        private IPressurePlateManager _pressurePlateManager;
        private GameObject pressurePlateObject = new GameObject();
        private PressurePlate pressurePlate; 
        
        [SetUp]
        public void SetUp()
        {
            _pressurePlateManager = Substitute.For<IPressurePlateManager>();
            pressurePlateObject.AddComponent<PressurePlate>();

            pressurePlate = pressurePlateObject.GetComponent<PressurePlate>();
            pressurePlate.name = "test";
            pressurePlate.PressurePlateManager = _pressurePlateManager;
        }

        [Test]
        public void check_that_pressurePlate_is_disabled_by_default()
        {
            Assert.False(pressurePlate.isPressed);
        }

        [Test]
        public void check_that_when_Activate_activates_pressurePlate()
        {
            pressurePlate.ActivatePressurePlate();
            
            Assert.True(pressurePlate.isPressed);
        }
        
        [Test]
        public void check_that_when_Disable_disables_pressurePlate()
        {
            pressurePlate.DisablePressurePlate();
            
            Assert.False(pressurePlate.isPressed);
        }
    }
}
