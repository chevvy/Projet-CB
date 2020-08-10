using System;
using System.Collections;
using System.Collections.Generic;
using NSubstitute.ExceptionExtensions;
using Prog.Script;
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
    public AudioClip armorPieceBreakingSound;

    private void Awake()
    {
        ArmorPieceLogic = new ArmorPieceLogic(armorPieceBreakingSound) { ApplyForce = new ApplyForce(), Rigidbody = GetComponent<Rigidbody>() };
        if (armorPieceBreakingSound != null)
        {
            
        }
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
    private AudioClip ArmorPieceBreakingSound;

    public ArmorPieceLogic(AudioClip sound = null)
    {
        ArmorPieceBreakingSound = sound;
    }

    /// <summary>
    /// On va venir débarquer la pièce d'armure en désactivant "kinematic"
    /// et en lui appliquant de la force en fonction de la position du joueur
    /// </summary>
    /// <param name="xAttackOriginPosition">Position en x du player</param>
    public void RemoveArmorPiece(float xAttackOriginPosition)
    {
        Rigidbody.isKinematic = false;
        PlayArmorPieceBreakingSound();
        ApplyForce.OnAttack(Rigidbody, null, xAttackOriginPosition, 5, 5);
    }

    private void PlayArmorPieceBreakingSound()
    {
        AudioManager.Instance.PlaySound("armor_piece_breaking", true);
    }
}
