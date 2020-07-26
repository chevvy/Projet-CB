using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Prog.Script.Armor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ArmorTests
    {
        GameObject ArmorInstance = new GameObject();
        private Armor _armor;

        [SetUp]
        public void Setup()
        {
            ArmorInstance.AddComponent<Armor>();
            _armor = ArmorInstance.GetComponent<Armor>();

        }

        // [Test]
        // public void if_armor_doesnt_have_pieces_doesnt_throw()
        // {
        //     _armor.armorPieces = new IArmorPiece[] { };
        //     _armor.TakeDamage(5, 5);
        // }

        [Test]
        public void if_armor_is_broken_doesnt_apply_damage_to_armorHealth()
        {

        }

        
        [TestFixture]
        public class ArmorWithPiecesTest
        {
            GameObject ArmorInstance = new GameObject();
            private Armor _armor;

            private ArmorPiece _armorPiece1;
            private ArmorPiece _armorPiece2;
            private readonly GameObject _armorPieceInstance1 = new GameObject();
            private readonly GameObject _armorPieceInstance2 = new GameObject();
            private readonly IArmorPieceLogic _armorPieceLogic1 = Substitute.For<IArmorPieceLogic>();
            private readonly IArmorPieceLogic _armorPieceLogic2 = Substitute.For<IArmorPieceLogic>();

            [SetUp]
            public void Setup()
            {
                ArmorInstance.AddComponent<Armor>();
                _armor = ArmorInstance.GetComponent<Armor>();

                // Setup complexe avec les vrai component de armor pieces car necessaires pour pouvoir Serialize
                // correctement dans unity
                _armorPieceInstance1.AddComponent<ArmorPiece>();
                _armorPieceInstance1.AddComponent<Rigidbody>();
                _armorPiece1 = _armorPieceInstance1.GetComponent<ArmorPiece>();
                _armorPieceLogic1.Rigidbody = _armorPieceInstance1.GetComponent<Rigidbody>();
                _armorPiece1.ArmorPieceLogic = _armorPieceLogic1;
                
                _armorPieceInstance2.AddComponent<ArmorPiece>();
                _armorPieceInstance2.AddComponent<Rigidbody>();
                _armorPiece2 = _armorPieceInstance2.GetComponent<ArmorPiece>();
                _armorPieceLogic2.Rigidbody = _armorPieceInstance2.GetComponent<Rigidbody>();
                _armorPiece1.ArmorPieceLogic = _armorPieceLogic2;

                _armor.armorPieces = new[] { _armorPiece1, _armorPiece2 };
            }

            [Test]
            public void if_takeDamage_and_has_armorPieces_armor_takes_damage()
            {
                _armor.TakeDamage(5, 1);
                Assert.True(_armor.armorHealth == 95);
            }
            
            // if armorPieces > 0, on enlève une pièce d'armure
            [Test]
            public void if_takeDamage_removes_armor_piece()
            {
                _armor.TakeDamage(5, 1);
                _armorPieceLogic1.Received().RemoveArmorPiece(1);
            }
            // if armorPieces > 0, on augmente l'index d'amrmor piece a enlever
            
            
        }
        
        // if armorPIeces index est plus grand que le nb de pieces, on set vie a 0 et is broken
        
    }
}
