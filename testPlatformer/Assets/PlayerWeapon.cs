using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public int dps = 5;
    private Collider _weaponCollider;
    // Start is called before the first frame update
    void Start()
    {
        _weaponCollider = GetComponent<Collider>();
    }
    
}
