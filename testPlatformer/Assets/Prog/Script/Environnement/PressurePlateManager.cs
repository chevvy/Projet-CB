using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Prog.Script.Environnement;
using UnityEngine;

public class PressurePlateManager : MonoBehaviour
{
    [SerializeField] private Door targetedDoor; //  la porte qui sera ouverte suite à l'activation
    [SerializeField] private PressurePlate[] _pressurePlates;
    [SerializeField] private bool _allPlatesMustBeActivated = false;
    
    private Dictionary<string, bool> _listOfPlates = new Dictionary<string, bool>();
    private bool _isDoorActivated = false;


    private void Start()
    {
        addPressurePlatesOnLoad();
    }

    private void addPressurePlatesOnLoad()
    {
        if (_pressurePlates == null) { return; }
        foreach (var pressurePlate in _pressurePlates)
        {
            _listOfPlates.Add(pressurePlate.name, false);
            pressurePlate.PressurePlateManager = this;
        }
    }

    private void PressurePlatesStateVerification()
    {
        var allPlatesAreActivated = true;
        foreach (var plate in _listOfPlates)
        {
            if (!plate.Value) // si la plate n'est pas activé
            {
                allPlatesAreActivated = false;
                targetedDoor.CloseDoor();
            }
        }

        if (!allPlatesAreActivated) return;
        targetedDoor.OpenDoor();
    }

    public void PressurePlateIsPressed(string pressurePlateName)
    {
        _listOfPlates[pressurePlateName] = true;
        PressurePlatesStateVerification();
    }

    public void PresurePlateIsReleased(string pressurePlateName)
    {
        _listOfPlates[pressurePlateName] = false;
        PressurePlatesStateVerification();
    }
    
    
    
}
