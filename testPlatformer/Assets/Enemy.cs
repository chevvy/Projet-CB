using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealthPoint = 100;
    public bool isDead = false;
    
    private MeshRenderer _enemyMesh;
    private int _currentHealth;
    
    
    void Start()
    {
        _enemyMesh = GetComponent<MeshRenderer>();
        _currentHealth = maxHealthPoint;
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (_currentHealth <= 0)
        {
            Die();    
        }
    }

    private void Die()
    {
        Debug.Log("enemy dieeed");
        GetComponent<Collider>().enabled = false;
        enabled = false;
    }
}
