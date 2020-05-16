using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int healthPoint = 100;
    public bool isDead = false;
    private MeshRenderer _enemyMesh;
    // Start is called before the first frame update
    void Start()
    {
        _enemyMesh = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (healthPoint <= 0)
        {
            _enemyMesh.enabled = false;
            isDead = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerWeapon") && !isDead)
        {
            Debug.Log("ayoye");
            healthPoint -= other.GetComponent<PlayerWeapon>().dps;
        }
    }
}
