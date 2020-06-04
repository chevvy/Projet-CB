using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private bool _allPlatesAreActivated = false;


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

    public bool CheckPressurePlateState(string pressurePlateName)
    {
        return _listOfPlates[pressurePlateName];
    }

    public int GetNbOfPressurePlates()
    {
        return _listOfPlates.Count;
    }

    public bool GetActivationStateOfManager()
    {
        return _allPlatesAreActivated;
    }

    public void AddPressurePlate(PressurePlate pressurePlate)
    {
        _listOfPlates.Add(pressurePlate.name, false);
        pressurePlate.PressurePlateManager = GetComponent<PressurePlateManager>();
    }

    public void SetDoor(Door door)
    {
        targetedDoor = door;
    }
}
