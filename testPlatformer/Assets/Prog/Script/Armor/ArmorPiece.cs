using System;
using System.Collections;
using System.Collections.Generic;
using NSubstitute.ExceptionExtensions;
using Prog.Script.RigidbodyInteraction;
using UnityEngine;
using UnityEngine.Assertions;

public interface IArmorPiece
{
    /// <summary>
    /// On va venir débarquer la pièce d'armure en désactivant "kinematic"
    /// et en lui appliquant de la force en fonction de la position du joueur
    /// </summary>
    /// <param name="xAttackOriginPosition">Position en x du player</param>
    void RemoveArmorPiece(float xAttackOriginPosition);
}

public class ArmorPiece : MonoBehaviour, IArmorPiece
{
    public bool isArmorPieceInTheBack = false;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (!_rigidbody.isKinematic)
        {
            Debug.LogError("The armorPiece '" + name + "' rigidBody must be kinematic");
        }
    }

    /// <summary>
    /// On va venir débarquer la pièce d'armure en désactivant "kinematic"
    /// et en lui appliquant de la force en fonction de la position du joueur
    /// </summary>
    /// <param name="xAttackOriginPosition">Position en x du player</param>
    public void RemoveArmorPiece(float xAttackOriginPosition)
    {
        _rigidbody.isKinematic = false;
        ApplyForce.OnAttack(_rigidbody, null, xAttackOriginPosition, 5, 5);
        Debug.Log("Removed armor piece : " + name);
    }
}
