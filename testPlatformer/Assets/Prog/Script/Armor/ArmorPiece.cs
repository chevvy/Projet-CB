using System;
using System.Collections;
using System.Collections.Generic;
using NSubstitute.ExceptionExtensions;
using Prog.Script.Armor;
using Prog.Script.RigidbodyInteraction;
using UnityEngine;
using UnityEngine.Assertions;

public interface IArmorPieceLogic
{
    Rigidbody Rigidbody { get; set; }
    IApplyForce ApplyForce { get; set; }
    /// <summary>
    /// On va venir débarquer la pièce d'armure en désactivant "kinematic"
    /// et en lui appliquant de la force en fonction de la position du joueur
    /// </summary>
    /// <param name="xAttackOriginPosition">Position en x du player</param>
    void RemoveArmorPiece(float xAttackOriginPosition);
}

public class ArmorPiece : MonoBehaviour
{
    public bool isArmorPieceInTheBack = false;
    public void RemoveArmorPiece(float xAttackOrigin) => ArmorPieceLogic.RemoveArmorPiece(xAttackOrigin);

    public IArmorPieceLogic ArmorPieceLogic { get; set; }

    private void Awake()
    {
        ArmorPieceLogic = new ArmorPieceLogic { ApplyForce = new ApplyForce(), Rigidbody = GetComponent<Rigidbody>() };
        if (!ArmorPieceLogic.Rigidbody.isKinematic)
        {
            Debug.LogError("The armorPiece '" + name + "' rigidBody must be kinematic");
        }
    }
}

public class ArmorPieceLogic : IArmorPieceLogic
{
    public Rigidbody Rigidbody { get; set; }
    public IApplyForce ApplyForce { get; set; }
    /// <summary>
    /// On va venir débarquer la pièce d'armure en désactivant "kinematic"
    /// et en lui appliquant de la force en fonction de la position du joueur
    /// </summary>
    /// <param name="xAttackOriginPosition">Position en x du player</param>
    public void RemoveArmorPiece(float xAttackOriginPosition)
    {
        Rigidbody.isKinematic = false;
        ApplyForce.OnAttack(Rigidbody, null, xAttackOriginPosition, 5, 5);
    }
}
