using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float nearFloorJumpInputDelay;
    public float jumpForce;
    public CharacterController playerCC;
    bool wasNearFloor;
    bool hasJumped;
    bool canJump;
    float _jumpInputTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        _jumpInputTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCC.isGrounded)
        {
            canJump = true;
            print("I'm grounded");
        }
        if (Input.GetButtonDown("Jump"))
        {
            _jumpInputTimer = nearFloorJumpInputDelay;
        }
        if (_jumpInputTimer >= 0)
        {
            wasNearFloor = true;
            _jumpInputTimer -= Time.deltaTime;
        }
        if (_jumpInputTimer <= 0)
        {
            wasNearFloor = false;
        }
        if (canJump == true && wasNearFloor && Input.GetButtonDown("Jump"))
        {
            print("I've inputed jumped");
            
            hasJumped = true;
            canJump = false;

        }
        if (hasJumped == true && Input.GetButton("Jump"))
        {
            print("I've jumped");
            playerCC.Move(transform.up * jumpForce * Time.deltaTime); 
           
        }
        if (hasJumped == true && !Input.GetButton("Jump"))
        {
            print("I've stop inputing jump");
            hasJumped = false;
        }
    }

    void Jump()
    { 
    
    
    }

}
