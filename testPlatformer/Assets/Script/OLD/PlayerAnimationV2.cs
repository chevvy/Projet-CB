using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationV2 : MonoBehaviour
{
    public string animCurrentState = "";
    public string animDesiredState = "";
    public bool grounded;
    // Ici Debug Variables

    Animator playerAnimator;
    CharacterController playerCC;
    PlayerBaseMovementV4 playerBaseMovementV4;
    

    public Vector3 movementOutput;
    
    string tempTriggerName;
    

    bool isJumping;
    bool isFalling;
    bool isLanding;
    bool isIdling;
    bool isRunningLeft;
    bool isRunningRight;

    bool isOrientationRight;
    bool isJumpRight;

    string animCanDo;
    
    int runState;

    float nearJumpDelay;
    float jumpDelayForAnim;
    public float idleDelay = 0.1f;

    float _jumpInputTimer;
    public float _idleTimer;


    // Start is called before the first frame update
    void Start()
    {
        playerBaseMovementV4 = GetComponent<PlayerBaseMovementV4>();
        nearJumpDelay = playerBaseMovementV4.nearJumpDelay;
        jumpDelayForAnim = playerBaseMovementV4.jumpDelayForAnim;

        playerCC = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
        isOrientationRight = true;
        isJumpRight = true;
        animCanDo = "ICanFall";
        AnimBoolCanceler();
        runState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        PlayerMovementOutput();
        PlayerOrientation();
        Timer();
        AnimStateChecker();
    }

    void PlayerMovementOutput()
    {
        movementOutput = playerCC.velocity;
    }

    void PlayerOrientation()
    {
        if (Input.GetAxisRaw("Horizontal")> 0)
        {
            isOrientationRight = true;
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            isOrientationRight = false;
        }

    }

    void InputManager()
    {

        if (_jumpInputTimer > 0 || Input.GetAxisRaw("Horizontal") != 0)
        {
            _idleTimer = idleDelay;
        }

        if(Input.GetButtonDown("Jump"))
        {

            _jumpInputTimer = nearJumpDelay + jumpDelayForAnim;
        }

        if (playerCC.isGrounded && animCanDo == "ICanWalk&Jump" && !Input.GetButtonDown("Jump"))
        {
            animCanDo = "ICanWalk&Jump";

            if (Input.GetAxisRaw("Horizontal") < 0 && isRunningLeft == false)
            {
                AnimBoolCanceler();
                isRunningLeft = true;
                animCurrentState = "RunningLeft";
                playerAnimator.SetTrigger("trigRunLeft");
                
            }
            if (Input.GetAxisRaw("Horizontal") > 0 && isRunningRight == false)
            {
                AnimBoolCanceler();
                isRunningRight = true;
                animCurrentState = "RunningRight";
                playerAnimator.SetTrigger("trigRunRight");
                
            }
            if ( _idleTimer <=0 && Input.GetAxisRaw("Horizontal") == 0 && isIdling == false )
            {
                AnimBoolCanceler();
                isIdling = true;
                animCurrentState = "Idle";
                if (isOrientationRight == true) { playerAnimator.SetTrigger("trigIdleRight"); }
                if (isOrientationRight == false) { playerAnimator.SetTrigger("trigIdleLeft"); }
                
            }
        }

        if (_jumpInputTimer>0 && playerCC.isGrounded && isJumping == false && animCanDo == "ICanWalk&Jump")
        {
            _idleTimer = idleDelay;
            _jumpInputTimer = 0;
            AnimBoolCanceler();
            animCanDo = "ICanFall";
            animCurrentState = "Jumping";
            isJumping = true;
            if (isOrientationRight == true) { playerAnimator.SetTrigger("trigJumpRight"); isJumpRight = true; }
            if (isOrientationRight == false) { playerAnimator.SetTrigger("trigJumpLeft"); isJumpRight = false; }
        }
       
        if (playerCC.velocity.y<0 && !playerCC.isGrounded && isFalling == false && animCanDo == "ICanFall")
        {
            AnimBoolCanceler();
            animCanDo = "ICanLand";
            animCurrentState = "Falling";
            isFalling = true;
            if (isJumpRight == true) { playerAnimator.SetTrigger("trigFallRight"); }
            if (isJumpRight == false) { playerAnimator.SetTrigger("trigFallLeft"); }
        }

        if (playerCC.isGrounded && isFalling == true && isLanding == false && animCanDo == "ICanLand")
        {
            AnimBoolCanceler();
            animCanDo = "ICanWalk&Jump";
            animCurrentState = "Landing";
            isLanding = true;
            if (isJumpRight == true) { playerAnimator.SetTrigger("trigLandRight"); }
            if (isJumpRight == false) { playerAnimator.SetTrigger("trigLandLeft"); }
        }

    }


    void AnimStateChecker()
    {
        if (Input.GetAxisRaw("Horizontal") >0)
        {
            animDesiredState = "RunningRight";
        }
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            animDesiredState = "RunningLeft";
        }
        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            animDesiredState = "Idle";
        }
        if (Input.GetButton("Jump"))
        {
            animDesiredState = "Jumpin";
        }
        if (!playerCC.isGrounded && !Input.GetButton("Jump"))
        {
            animDesiredState = "Falling";
        }


    }

    void AnimBoolCanceler()
    {
        isJumping = false;
        isFalling = false;
        isLanding = false;
        isIdling = false;
        isRunningRight = false;
        isRunningLeft = false;
    }

    void Timer()
    {
        _jumpInputTimer -= Time.deltaTime;
        _idleTimer -= Time.deltaTime;
    }


}
