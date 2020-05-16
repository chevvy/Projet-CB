using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealthPoint = 100;
    public bool isDead = false;
    private bool _hasTakenDamage = false;
    
    private MeshRenderer _enemyMesh;
    private int _currentHealth;

    public Material damageMaterial;
    private Material _enemyMaterial;
    

    void Start()
    {
        _enemyMesh = GetComponent<MeshRenderer>();
        _enemyMaterial = _enemyMesh.material;
        _currentHealth = maxHealthPoint;
    }

    public void TakeDamage(int damage)
    {
        _enemyMesh.material = damageMaterial;
        _currentHealth -= damage;
        _hasTakenDamage = true;
        StartCoroutine(ApplyDamageMaterial());
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (_currentHealth <= 0)
        {
            Die();    
        }
        Debug.Log("debug");
    }

    private void Die()
    {
        Debug.Log("enemy dieeed");
        GetComponent<Collider>().enabled = false;
        enabled = false;
    }

    IEnumerator ApplyDamageMaterial()
    {
        _enemyMesh.material = damageMaterial;
        yield return new WaitForSeconds(.1f);
        _enemyMesh.material = _enemyMaterial;
    }
}
