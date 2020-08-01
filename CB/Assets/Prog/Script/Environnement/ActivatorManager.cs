using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Prog.Script.Environnement;
using UnityEngine;
using UnityEngine.Serialization;

public class ActivatorManager : MonoBehaviour, IActivatorManager
{
    //[SerializeField] private Door targetedDoor; //  la porte qui sera ouverte suite à l'activation
    
    [SerializeField] private Animator targetAnimator; // l'animator a qui on va envoyer les changements d'anim
    [SerializeField] public string animationParam; //Le param qui sera toggled selon le state d'activation
    
    [FormerlySerializedAs("_pressurePlates")] [SerializeField] private Activator[] activators;
    [FormerlySerializedAs("_allPlatesMustBeActivated")] [SerializeField] private bool allActivatorsMustStayEnabled = false;
    
    private Dictionary<string, bool> _listOfPlates = new Dictionary<string, bool>();
    private bool _isDoorActivated = false;
    private bool _allPlatesAreActivated = false;
    private int EnableAnimation => Animator.StringToHash(animationParam);



    private void Start()
    {
        addActivatorsOnLoad();
    }

    public void addActivatorsOnLoad()
    {
        if (activators == null) { return; }
        foreach (var pressurePlate in activators)
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
                targetAnimator.SetBool(EnableAnimation, false);
            }
        }

        if (!_allPlatesAreActivated) return;
        targetAnimator.SetBool(EnableAnimation, true);
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
        //targetedDoor = door;
    }

    public void SetAnimator(Animator animator)
    {
        targetAnimator = animator;
    }
}
