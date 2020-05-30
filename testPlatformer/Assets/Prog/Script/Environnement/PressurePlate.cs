using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    private bool _isPressed = false; // FOR TESTING PURPOSE 
    public PressurePlateManager PressurePlateManager { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerCharacter"))
        {
            ActivatePressurePlate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerCharacter"))
        {
            DisablePressurePlate();
        }
    }

    private void ActivatePressurePlate()
    {
        _isPressed = true;
        PressurePlateManager.PressurePlateIsPressed(this.name);
    }

    private void DisablePressurePlate()
    {
        _isPressed = false;
        PressurePlateManager.PresurePlateIsReleased(this.name);
    }
}
