using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationV5 : MonoBehaviour
{
    CharacterController playerCC;
    Animator playerAnimator;

    bool isRightTrigged;
    bool isLeftTrigged;
    bool isAirRightTrigged;
    bool isAirLeftTrigged;
    bool isJumpTrigged;
    bool isFallTrigged;
    bool isLandTrigged;
    bool isNeutralTrigged;

    bool isPlayerRight;
    bool isJumpRight;

    float _noInputTimer;
    public float noInputDelay = 0.2f;

    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        RefreshTrigger();
    }


    void Update()
    {
        InputManager();
        PlayerOrientation();
        Timer();
    }


    void InputManager()
    {
        InputTriggerFall();
        InputTriggerJump();
        InputTriggerLand();
        InputTriggerLeft();
        InputTriggerRight();
        InputTriggerAirLeft();
        InputTriggerAirRight();
        InputTriggerNeutral();
    }

    void InputTriggerRight()
    {
        if (Input.GetAxisRaw("Horizontal") > 0 && isRightTrigged == false && playerCC.isGrounded)
        {
            // ground right
            print("ground right");
            playerAnimator.SetTrigger("trigRunRight");
            ResfreshSideTrigger();
            isRightTrigged = true;
        }
    }

    void InputTriggerAirRight()
    {
        if (Input.GetAxisRaw("Horizontal") > 0 && isAirRightTrigged == false && !playerCC.isGrounded)
        {
            // air right  ** need animation
            print("air right");
            //playerAnimator.SetTrigger("");
            ResfreshSideTrigger();
            isAirRightTrigged = true;
        }
    }

    void InputTriggerLeft()
    {
        if (Input.GetAxisRaw("Horizontal") < 0 && isLeftTrigged == false && playerCC.isGrounded)
        {
            // ground left
            print("ground left");
            playerAnimator.SetTrigger("trigRunLeft");
            ResfreshSideTrigger();
            isLeftTrigged = true;
        }
    }
    void InputTriggerAirLeft()
    {
        if (Input.GetAxisRaw("Horizontal") < 0 && isAirLeftTrigged == false && !playerCC.isGrounded)
        {
            // left air  ** need animation
            print("air left");
            //playerAnimator.SetTrigger("");
            ResfreshSideTrigger();
            isAirLeftTrigged = true;
        }
    }

    void InputTriggerJump()
    {
        if (Input.GetButtonDown("Jump") && playerCC.isGrounded && isJumpTrigged == false)
        {
            isJumpRight = isPlayerRight;
            // up air
            print("jump");
            if (isJumpRight == true) { playerAnimator.SetTrigger("trigJumpRight"); }
            if (isJumpRight == false) { playerAnimator.SetTrigger("trigJumpLeft"); }
            ResfreshVerticalTrigger();
            isJumpTrigged = true;
        }
    }
    void InputTriggerFall()
    {
        if (!Input.GetButton("Jump") && !playerCC.isGrounded && isFallTrigged == false && isJumpTrigged == true)
        {
            // down air
            print("fall");
            if (isJumpRight == true) { playerAnimator.SetTrigger("trigFallRight"); }
            if (isJumpRight == false) { playerAnimator.SetTrigger("trigFallLeft"); }
            ResfreshVerticalTrigger();
            isFallTrigged = true;
        }
        if (!Input.GetButton("Jump") && !playerCC.isGrounded && isFallTrigged == false && isJumpTrigged == false)
        {
            // down air
            print("fall");
            if (isPlayerRight == true) { playerAnimator.SetTrigger("trigFallRight"); }
            if (isPlayerRight == false) { playerAnimator.SetTrigger("trigFallLeft"); }
            ResfreshVerticalTrigger();
            isFallTrigged = true;
        }
    }
    void InputTriggerLand()
    {
        if (playerCC.isGrounded && isFallTrigged == true && isLandTrigged == false)
        {
            print("land");
            if (isJumpRight == true) { playerAnimator.SetTrigger("trigLandRight"); }
            if (isJumpRight == false) { playerAnimator.SetTrigger("trigLandLeft"); }
            ResfreshVerticalTrigger();
            ResfreshSideTrigger();
            isLandTrigged = true;
           
        }
    }
    void InputTriggerNeutral()
    {
        if (!Input.GetButton("Jump") && Input.GetAxisRaw("Horizontal") == 0 && isNeutralTrigged == false && _noInputTimer < 0)
        {
            if (playerCC.isGrounded)
            {
                
                print("ground neutral");
                if (isPlayerRight == true) { playerAnimator.SetTrigger("trigIdleRight"); }
                if (isPlayerRight == false) { playerAnimator.SetTrigger("trigIdleLeft"); }
                RefreshTrigger();
                isNeutralTrigged = true;
                
            }
        }


    }

    void RefreshTrigger()
    {
        ResfreshSideTrigger();
        ResfreshVerticalTrigger();

    }

    void ResfreshSideTrigger()
    {
        _noInputTimer = noInputDelay;
        isNeutralTrigged = false;
        isRightTrigged = false;
        isLeftTrigged = false;
        isAirLeftTrigged = false;
        isAirRightTrigged = false;
    }

    void ResfreshVerticalTrigger()
    {
        _noInputTimer = noInputDelay;
        isJumpTrigged = false;
        isFallTrigged = false;
        isLandTrigged = false;
    }

    void RefreshNeutralTrigger()
    {
        isNeutralTrigged = false;
    }


    void PlayerOrientation()
    {

        if (Input.GetAxisRaw("Horizontal") > 0) { isPlayerRight = true; }
        if (Input.GetAxisRaw("Horizontal") < 0) { isPlayerRight = false; }
    }

    void Timer()
    {
        _noInputTimer -= Time.deltaTime;
    }

}
