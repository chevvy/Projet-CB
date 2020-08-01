using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    bool animCrouchLock;
    bool animIdleLock;
    bool animRunningLeftLock;
    bool animRunningRightLock;
    float horizontalAxis;
    float minimalJumpTime;

    bool isJumpingRight;
    bool isPlayerFacingRight;

    public float idleDelayTime = 1f;
    

    bool animLandLock;
    bool animFallLock;
    bool animJumpLock;
    bool asLanded;
    int animStateJump;

    public PlayerBaseMovementV4 scriptMovement;
    public Animator playerAnimator;
    public CharacterController playerCC;

    float _minimalJumpTimer;
    float _idleTimer;

    // Start is called before the first frame update
    void Start()
    {
        AnimUnlock();
        isPlayerFacingRight = true;
        bool asLanded = true;
        playerCC = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        animStateJump = 0;
        scriptMovement = GetComponent<PlayerBaseMovementV4>();
        minimalJumpTime = scriptMovement.minimalJumpTime;

    }

    // Update is called once per frame
    void Update()
    {
        IdleAnim();
        SideMovementAnim();
        JumpAnimation();
        Timer();
        PlayerOrientation();
    }

    // Je me question sur l'Ordre d'arrivé de la fonction, elle ne fonctionne malheureusement plus.
    void AnimUnlock()
    {
        animCrouchLock = false;
        animIdleLock = false;
        animRunningLeftLock = false;
        animRunningRightLock = false;
        animJumpLock = false;
        animLandLock = false;
        animFallLock = false;

    }
    void JumpAnimation()
    {   
        

        if (playerCC.isGrounded && Input.GetButtonDown("Jump") && animJumpLock == false && animStateJump == 0) 
        {
            _minimalJumpTimer = minimalJumpTime;
            animStateJump = 1;
            AnimUnlock();
            animJumpLock = true;
            asLanded = false;

            if (isPlayerFacingRight == true) { isJumpingRight = true; playerAnimator.SetTrigger("trigJumpRight"); }
            if (isPlayerFacingRight == false) { isJumpingRight = false; playerAnimator.SetTrigger("trigJumpLeft"); }
        }
        if (!playerCC.isGrounded && !Input.GetButton("Jump") && animFallLock == false && animStateJump == 1 && _minimalJumpTimer <= 0 )
        {
            animStateJump = 2;
            AnimUnlock();
            animFallLock = true;

            if (isJumpingRight == true) { playerAnimator.SetTrigger("trigFallRight"); }
            if (isJumpingRight == false) { playerAnimator.SetTrigger("trigFallLeft"); }
        }

        if (playerCC.isGrounded && asLanded == false && animLandLock == false && animStateJump == 2)
        {
            animStateJump = 0;
            AnimUnlock();
            animLandLock = true;
            asLanded = true;

            _idleTimer = 0f;
            
            if (isJumpingRight == true) { playerAnimator.SetTrigger("trigLandRight"); }
            if (isJumpingRight == false) { playerAnimator.SetTrigger("trigLandLeft"); }

        }

    }

    void IdleAnim()
    {
        if (horizontalAxis !=0 || !playerCC.isGrounded)
        {
            _idleTimer = 0f;
        }

        

        if ( animIdleLock == false && _idleTimer >= idleDelayTime && playerCC.isGrounded && animStateJump == 0)
        {
            AnimUnlock();
            animIdleLock = true;
            if (isPlayerFacingRight == true) { playerAnimator.SetTrigger("trigIdleRight"); }
            if (isPlayerFacingRight == false) { playerAnimator.SetTrigger("trigIdleLeft"); }
            
        }
       

    }
   

    void SideMovementAnim()
    {
        if (playerCC.isGrounded && animStateJump == 0)
        {
            horizontalAxis = Input.GetAxisRaw("Horizontal");

            if (horizontalAxis>0 && animRunningRightLock == false)
            {
                AnimUnlock();
                animRunningRightLock = true;
                playerAnimator.SetTrigger("trigRunRight");
            }
            if (horizontalAxis < 0 && animRunningLeftLock == false)
            {
                AnimUnlock();
                animRunningLeftLock = true;
                playerAnimator.SetTrigger("trigRunLeft");
            }
           
            
        }

    }

    void PlayerOrientation()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            isPlayerFacingRight = true;
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            isPlayerFacingRight = false;
        }
    }

    void AnimLock()
    { 
    }

    

    void Timer ()
    {
        _minimalJumpTimer -= Time.deltaTime;
        _idleTimer += Time.deltaTime;
    }

  






}
