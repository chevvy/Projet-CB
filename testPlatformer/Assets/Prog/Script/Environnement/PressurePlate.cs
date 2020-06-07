using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool isPressed = false; // FOR TESTING PURPOSE 
    public IPressurePlateManager PressurePlateManager { get; set; }

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

    public void ActivatePressurePlate()
    {
        isPressed = true;
        PressurePlateManager.PressurePlateIsPressed(this.name);
    }

    public void DisablePressurePlate()
    {
        isPressed = false;
        PressurePlateManager.PresurePlateIsReleased(this.name);
    }
    
    
}
