using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationV3 : MonoBehaviour
{

    public string debugString;
    // Debug

    public Animator playerAnimator;
    public CharacterController playerCC;
    PlayerBaseMovementV4 playerBaseMovementV4;

    bool isPlayerRight;
    bool canLand;
    public string animState;

    bool lockJumpAnim;
    bool lockFallAnim;
    bool lockLandAnim;
    bool lockRunAnim;
    bool lockIdleAnim;


    
    void Start()
    {
        AnimUnlocker();
        canLand = true;
        playerCC = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    
    void Update()
    {
        
        PlayerOrientation();
        JumpInputManager();
    }

    void PlayerOrientation()
    {
        if (Input.GetAxisRaw("Horizontal")>0) { isPlayerRight = true; }
        if (Input.GetAxisRaw("Horizontal") > 0) { isPlayerRight = false; }
    }

    void JumpInputManager()
    {
        if (Input.GetButton("Jump"))
        {
            if (playerCC.isGrounded)
            {
                if(canLand == true)
                {
                    animState = "Land";
                    canLand = false;
                  
                    TriggerLand();
                }

                if (canLand == false)
                {
                    animState = "Jumping";
                    
                    TriggerJump();

                }
                
            }
            if (playerCC.velocity.y<0)
            {
                animState = "Falling With Input";
             
                TriggerFall();
            }
        }

        if (!Input.GetButton("Jump"))
        {
            if (playerCC.isGrounded)
            {
                if (canLand == true)
                {
                    animState = "Land";
                    canLand = false;
                    
                    TriggerLand();
                }
                
                if (canLand == false)
                {
                    animState = "Idle";
          
                    TriggerIdle();
                }
            }
            if (!playerCC.isGrounded && playerCC.velocity.y < 0)
            {
                animState = "Falling No Input";

                TriggerFall();
            }
        }

        if (!playerCC.isGrounded)
        {
            if (animState == "Jumping" || animState == "Falling")
            {
                canLand = true;
            }
        }    
    }


    void TriggerJump()
    {
        if (lockJumpAnim == false)
        {
            debugString = "TriggerJump";
            if (isPlayerRight == true) { playerAnimator.SetTrigger("trigJumpRight"); }
            if (isPlayerRight == false) { playerAnimator.SetTrigger("trigJumpLeft"); }
            AnimUnlocker();
            lockJumpAnim = true;
        }
    }
    void TriggerFall()
    {
        if (lockFallAnim == false)
        {
            debugString = "TriggerFall";
            if (isPlayerRight == true) { playerAnimator.SetTrigger("trigFallRight"); }
            if (isPlayerRight == false) { playerAnimator.SetTrigger("trigFallLeft"); }
            AnimUnlocker();
            lockJumpAnim = true;
        }
    }

    void TriggerLand()
    {
        if (lockLandAnim == false)
        {
            debugString = "TriggerLand";
            if (isPlayerRight == true) { playerAnimator.SetTrigger("trigLandRight"); }
            if (isPlayerRight == false) { playerAnimator.SetTrigger("trigLandLeft"); }
            AnimUnlocker();
            lockJumpAnim = true;
        }
    }

    void TriggerRun()
    {
        if (lockRunAnim == false)
        {
            if (isPlayerRight == true) { playerAnimator.SetTrigger("trigRunRight"); }
            if (isPlayerRight == false) { playerAnimator.SetTrigger("trigRunLeft"); }
            AnimUnlocker();
            lockJumpAnim = true;
        }
    }
    void TriggerIdle()
    {
        if (lockIdleAnim == false)
        {
            debugString = "TriggerIdle";
            if (isPlayerRight == true) { playerAnimator.SetTrigger("trigRunRight"); }
            if (isPlayerRight == false) { playerAnimator.SetTrigger("trigRunLeft"); }
            AnimUnlocker();
            lockJumpAnim = true;
        }
    }

    void AnimUnlocker()
    {
        lockJumpAnim = false;
        lockFallAnim = false;
        lockLandAnim = false;
        lockIdleAnim = false;
        lockRunAnim = false;
    }



}
