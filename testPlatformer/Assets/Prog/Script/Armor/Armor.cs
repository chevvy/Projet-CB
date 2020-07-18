using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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

    private int _armorPieceToBeRemovedIndex = 0;

    /// <summary>
    /// Fonction qui reçoit une attaque, calcule la vie restante sur l'armure,
    /// calcule si on doit enlever une pièce d'armure et finalement indique si l'armure est brisé/morte
    /// </summary>
    /// <param name="damage">Les dommages reçus par l'attaque</param>
    /// <param name="xAttackOriginPosition">La position en x de l'origine de l'attaque</param>
    public void TakeDamage(int damage, float xAttackOriginPosition)
    {
        if (armorPieces.Length < _armorPieceToBeRemovedIndex || IsArmorBroken()) { return; }

        armorPieces[_armorPieceToBeRemovedIndex].RemoveArmorPiece(xAttackOriginPosition);
        _armorPieceToBeRemovedIndex += 1;
    }

    



}
