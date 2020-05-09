using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationV6 : MonoBehaviour
{
    CharacterController playerCC;
    Animator playerAnimator;

    public bool isXActive;
    public bool isYActive;
   

    public string playerState;
    public string playerDesiredState;


    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }


    void Update()
    {
        PlayerStateManager();
        PlayerActiveManager();

    }


    void PlayerStateManager()
    {
        // Jump
        if (isYActive == true && !playerCC.isGrounded)
        {
            playerDesiredState = "jump";
            JumpTrigger();
        }
        // Fall
        if (isYActive == false && !playerCC.isGrounded)
        {
            playerDesiredState = "fall";
            FallTrigger();
        }
        // Run
        if (isXActive == true && playerCC.isGrounded)
        {
            playerDesiredState = "run";
            RunTrigger();
        }
        // Idle
        if (isXActive == false && playerCC.isGrounded)
        {
            playerDesiredState = "idle";
            IdleTrigger();
        }

    }

    void PlayerActiveManager()
    {
        if (playerCC.velocity.x != 0) { isXActive = true; }
        if (playerCC.velocity.x == 0) { isXActive = false; }
        if (playerCC.velocity.y > 0) { isYActive = true; }
        if (playerCC.velocity.y < 0) { isYActive = false; }

    }


    void JumpTrigger()
    {
        if ( playerDesiredState == "jump" && playerState !="jumping" )
        { 
            playerState = "jumping";
            playerAnimator.SetTrigger("trigJumpRight");
        }
        
    }
    void FallTrigger()
    {
        if (playerDesiredState == "fall" && playerState != "falling")
        {
            playerState = "falling";
            playerAnimator.SetTrigger("trigFallRight");

        }
    }
    void RunTrigger()
    {
        if (playerDesiredState == "run" && playerState != "running")
        {
            playerState = "running";
            playerAnimator.SetTrigger("trigRunRight");
        }
    }
    void IdleTrigger()
    {
        if (playerDesiredState == "idle" && playerState != "idling")
        {
            playerState = "idling";
            playerAnimator.SetTrigger("trigIdleRight");
        }
    }

}
