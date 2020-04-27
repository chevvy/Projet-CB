using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerANimationV4 : MonoBehaviour
{
    Animator playerAnimator;
    CharacterController playerCC;
    PlayerBaseMovementV4 playerBaseMovementV4;


    bool axisIsRight;
    bool axisIsLeft;
    bool isUp;

    void Start()
    {
        playerBaseMovementV4 = GetComponent<PlayerBaseMovementV4>();
        playerCC = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        ReadInput();
    }

    void ReadInput()
    {
        if (isUp == true)
        {
            ResetGroundBool();
        }

        if (playerCC.isGrounded)
        { 

            if (Input.GetButtonDown("Jump") && isUp == false)
            {
                isUp = true;

                print("jump");
            }

            if (Input.GetButtonUp("Jump") && isUp == true)
            {
                isUp = false;

                print("fall");
            }


            if (Input.GetAxis("Horizontal") > 0 && axisIsRight == false)
            {
                axisIsLeft = false;
                axisIsRight = true;
                print("right");
            }
            if (Input.GetAxis("Horizontal") < 0 && axisIsLeft == false)
            {
                axisIsRight = false;
                axisIsLeft = true;
                print("left");
            }
            if ((Input.GetAxis("Horizontal") == 0 && axisIsLeft == true) || (Input.GetAxis("Horizontal") == 0 && axisIsRight == true))
            {
                axisIsRight = false;
                axisIsLeft = false;
                print("AxisReset");
            }
        }
    }

    void CharacterState()
    { 
    
    }

    void ResetGroundBool()
    {
        isUp = false;
        axisIsLeft = false;
        axisIsRight = false;
    }




}
