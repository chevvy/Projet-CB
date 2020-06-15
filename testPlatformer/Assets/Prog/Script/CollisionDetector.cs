using System;
using System.Collections;
using System.Collections.Generic;
using Prog.Script;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public PlayerCombat playerCombat;
    
    // TODO Mettre des vérifications pour être certains d'avoir un rigidbody et un collider sur le mesh
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
        {
            playerCombat.Attack();
        }
    }
}
