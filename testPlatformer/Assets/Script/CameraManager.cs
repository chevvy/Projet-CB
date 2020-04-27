using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera playerCamera;
    CameraFollow cameraFollowScript;


    public float delayTime = 5f;
    public float _delayTimer;
    void Start()
    {
        cameraFollowScript = playerCamera.GetComponent<CameraFollow>();

        _delayTimer = delayTime;
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
        CameraActivator();
    }

    void Timer()
    {
        if (_delayTimer > 0) {_delayTimer -= Time.deltaTime;}

    }

    void CameraActivator()
    {
        if (_delayTimer <= 0)
        {
            cameraFollowScript.enabled = true;
        }
        else
        {
            cameraFollowScript.enabled = false;
        }
    }
}
