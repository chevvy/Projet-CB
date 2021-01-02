using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Serialization;
using Random = System.Random;

namespace Prog.Script.Armor
{
    public interface IArmor
    {
        bool IsArmorBroken();

        /// <summary>
        /// Fonction qui reçoit une attaque, calcule la vie restante sur l'armure,
        /// calcule si on doit enlever une pièce d'armure et finalement indique si l'armure est brisé/morte
        /// </summary>
        /// <param name="damage">Les dommages reçus par l'attaque</param>
        /// <param name="xAttackOriginPosition">La position en x de l'origine de l'attaque</param>
        void TakeDamage(int damage, float xAttackOriginPosition);
    }

    public class Armor : MonoBehaviour, IArmor
    {
        public ArmorPiece[] armorPieces;
        public int armorHealth = 100;
        public bool IsArmorBroken() => armorHealth < 1;
        public int numberOfArmorPiecesToBeRemovedEachHit = 1;

        private int _armorPieceToBeRemovedIndex = 0;

        private void Awake()
        {
            if (armorPieces != null)
            {
                RandomizeArray();
                return;
            }
            armorPieces = new ArmorPiece[]{};
        }

        private void RandomizeArray()
        {    
            var random = new Random();
            armorPieces = armorPieces.Select(x => new
            {
                value = x,
                order = random.Next()
            }).OrderBy(x => x.order).Select(x => x.value).ToArray();
        }

        /// <summary>
        /// Fonction qui reçoit une attaque, calcule la vie restante sur l'armure,
        /// calcule si on doit enlever une pièce d'armure et finalement indique si l'armure est brisé/morte
        /// </summary>
        /// <param name="damage">Les dommages reçus par l'attaque</param>
        /// <param name="xAttackOriginPosition">La position en x de l'origine de l'attaque</param>
        public void TakeDamage(int damage, float xAttackOriginPosition)
        {
            if (armorPieces.Length - 1 < _armorPieceToBeRemovedIndex || IsArmorBroken())
            {
                armorHealth = 0;
                return;
            }
            
            AdjustNumberOfPiecesToBeRemoved();

            for (int i = 0; i < numberOfArmorPiecesToBeRemovedEachHit; i++)
            {
                ApplyDamageToArmorPiece(damage, xAttackOriginPosition);
            }
        }

        private void AdjustNumberOfPiecesToBeRemoved()
        {
            var indexAtTheEndOfAttack = numberOfArmorPiecesToBeRemovedEachHit + _armorPieceToBeRemovedIndex;
            
            if (indexAtTheEndOfAttack <= armorPieces.Length) return; // On check qu'on est pas out of bound de l'array
            numberOfArmorPiecesToBeRemovedEachHit = armorPieces.Length - _armorPieceToBeRemovedIndex;
            
            Debug.Log("Adjusted nb of pieces to be removed " + numberOfArmorPiecesToBeRemovedEachHit);
        }

        private void ApplyDamageToArmorPiece(int damage, float xAttackOriginPosition)
        {
            armorPieces[_armorPieceToBeRemovedIndex].RemoveArmorPiece(xAttackOriginPosition);
            _armorPieceToBeRemovedIndex += 1;
            armorHealth -= damage;
        }
    }
}