using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    Vector3 cameraPosition = new Vector3(0, 0, 0);


    public Vector3 cameraOffset;
    public Vector3 targetOffset;
   
    Vector3 desiredPosition;
    Vector3 smoothedPosition;
    public float lerpSmoothValue = 10f;
    Vector3 dampVelocity;
    void Start()
    {
        
       
    }

    private void Update()
    {

    }
    private void FixedUpdate()
    {

    }

    void LateUpdate()
    {

        //smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref dampVelocity, lerpSmoothValue);

        desiredPosition = target.position + cameraOffset;

       // transform.position = smoothedPosition;

        transform.position = desiredPosition;

        //transform.LookAt(target.position + targetOffset);

    }

    void CameraLimit()
    {}

    
}
