using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public bool isPressed = false;
    public string tagThatEnablesActivator;
    public IActivatorManager ActivatorManager { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagThatEnablesActivator))
        {
            EnableActivator();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagThatEnablesActivator))
        {
            DisableActivator();
        }
    }
    public void EnableActivator()
    {
        isPressed = true;
        ActivatorManager.EnableActivator(this.name);
    }

    public void DisableActivator()
    {
        isPressed = false;
        ActivatorManager.DisableActivator(this.name);
    }
}
