using System;
using System.Collections;
using System.Collections.Generic;
using Prog.Script;
using UnityEngine;

public class WeaponImpactDetector : MonoBehaviour
{
    public PlayerCombat playerCombat;
    
    // TODO Mettre des vérifications pour être certains d'avoir un rigidbody et un collider sur le mesh

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            playerCombat.Attack(other.GetComponent<Enemy>());
        }
    }
}
