using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encounter : MonoBehaviour
{
    //TODO faire une interface de ça (begin, end)

    public int nbOfEnemiesToDestroy = 2;

    private int _nbOfEnemiesDestroyed = 0;
    private Animator _exitPod;
    private static readonly int Close = Animator.StringToHash("Close");

    private void Start()
    {
        _exitPod = GetComponent<Animator>();
    }
    
    

    public void EnemyKilled()
    {
        _nbOfEnemiesDestroyed++;
        if (_nbOfEnemiesDestroyed == nbOfEnemiesToDestroy)
        {
            _exitPod.SetBool(Close, true);
        } ;
    }

}
