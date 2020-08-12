using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Prog.Script.Armor;
using Prog.Script.RigidbodyInteraction;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ArmorPiecesTests
    {
        // TODO À RÉPARER SUITE AU REFACTOR QUE J'AVAIS FAIT
        GameObject ArmorPieceInstance = new GameObject();
        private ArmorPiece _armorPiece;
        private Rigidbody _armorPieceRigidbody;
        private IApplyForce _applyForce = Substitute.For<IApplyForce>();
        // private ArmorPieceLogic _armorPieceLogic = new ArmorPieceLogic();

        [SetUp]
        public void Setup()
        {
            ArmorPieceInstance.AddComponent<ArmorPiece>();
            _armorPiece = ArmorPieceInstance.GetComponent<ArmorPiece>();
            // _armorPiece.ArmorPieceLogic = new ArmorPieceLogic();

            ArmorPieceInstance.AddComponent<Rigidbody>();
            _armorPieceRigidbody = ArmorPieceInstance.GetComponent<Rigidbody>();
            _armorPieceRigidbody.isKinematic = true;
            // _armorPiece.ArmorPieceLogic.Rigidbody = _armorPieceRigidbody;
            
            // _armorPiece.ArmorPieceLogic.ApplyForce = _applyForce;

        }


        [Test]
        public void When_removing_armorPiece_sets_kinematic_to_false()
        {
            _armorPiece.RemoveArmorPiece(1);

            Assert.IsFalse(_armorPieceRigidbody.isKinematic);
        }

        [Test]
        public void when_removing_armorPiece_calls_applyForce()
        {
            _armorPiece.RemoveArmorPiece(1);
            _applyForce.Received().OnAttack(_armorPieceRigidbody, null, 1 , 5, 5);
        }
    }
}
