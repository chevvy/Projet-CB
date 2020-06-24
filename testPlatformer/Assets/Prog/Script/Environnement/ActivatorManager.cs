using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Prog.Script.Environnement;
using UnityEngine;

public class ActivatorManager : MonoBehaviour, IActivatorManager
{
    [SerializeField] private Door targetedDoor; //  la porte qui sera ouverte suite à l'activation
    [SerializeField] private Activator[] _pressurePlates;
    [SerializeField] private bool _allPlatesMustBeActivated = false;
    
    private Dictionary<string, bool> _listOfPlates = new Dictionary<string, bool>();
    private bool _isDoorActivated = false;
    private bool _allPlatesAreActivated = false;


    private void Start()
    {
        addActivatorsOnLoad();
    }

    public void addActivatorsOnLoad()
    {
        if (_pressurePlates == null) { return; }
        foreach (var pressurePlate in _pressurePlates)
        {
            _listOfPlates.Add(pressurePlate.name, false);
            pressurePlate.ActivatorManager = this;
        }
    }

    public void ActivatorsStateVerification()
    {
        _allPlatesAreActivated = true;
        foreach (var plate in _listOfPlates)
        {
            if (!plate.Value) // si la plate n'est pas activé
            {
                _allPlatesAreActivated = false;
                targetedDoor.CloseDoor();
            }
        }

        if (!_allPlatesAreActivated) return;
        targetedDoor.OpenDoor();
    }

    public void EnableActivator(string pressurePlateName)
    {
        _listOfPlates[pressurePlateName] = true;
        ActivatorsStateVerification();
    }

    public void DisableActivator(string pressurePlateName)
    {
        _listOfPlates[pressurePlateName] = false;
        ActivatorsStateVerification();
    }

    public bool CheckActivatorState(string pressurePlateName)
    {
        return _listOfPlates[pressurePlateName];
    }

    public int GetNbOfActivator()
    {
        return _listOfPlates.Count;
    }

    public bool GetActivationStateOfManager()
    {
        return _allPlatesAreActivated;
    }

    public void AddActivator(Activator activator)
    {
        _listOfPlates.Add(activator.name, false);
        activator.ActivatorManager = GetComponent<ActivatorManager>();
    }

    public void SetDoor(Door door)
    {
        targetedDoor = door;
    }
}
