using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviorV1 : MonoBehaviour
{
    public Vector3 debugPlayerVelocity;
    public float XRawAxis;
    public float YRawAxis;
    public float XAxis;
    public float YAxis;

    //Dubug Zone


    public CharacterController playerCC;
    public Animator playerAnimator;

    public AnimationCurve jumpingCurve;
    public AnimationCurve fallingCurve;
   // TweakZone
    public float horizontalSpeed = 5f;
    public float jumpForce = 5f;
    public float fallForce = 1f;
    public float gravity = 2f;
    public float runCooldownDelay = 0.1f;
    public float noXInputDelay = 0.1f;
    public float minimalJumpDelay= 0.2f;
    public float bigFallDelay = 0.2f;
    
   // SHHHHHH Secret Zone
    Vector3 movementOutput = new Vector3(0, 0, 0);

    bool isInputedHorizontalRight;
    bool isInputedHorizontalLeft;
    //bool isInputedHorizontal;
    bool isInputedVerticalUp;
    bool isInputedVerticalDown;
    bool isInputedJump;
    bool playerIsRight;
    bool isLastOrientationRight;
    bool isRunOnCoolDown;
    bool bigFall;
    bool dashMove;
    bool overdriveMove;

    public string desiredState;
    public string currentMoveState;
    public string currentAnimState;

    
    public float horizontalOutput;
    float verticalOutput;

    //TimerZone

    float _curveTimer;
    float _runCooldownTimer;
    float _releasedHorizontalInput;
    float _minimalJumpTimer;
    float _landTimer;


    // Start is called before the first frame update
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        XAxis = Input.GetAxis("Horizontal");
        YAxis = Input.GetAxis("Vertical");
        XRawAxis = Input.GetAxisRaw("Horizontal");
        YRawAxis = Input.GetAxisRaw("Vertical");
        debugPlayerVelocity = playerCC.velocity;
        PlayerState();
        InputManager();
        MovementOutput();
        OverDriveState();
        Timer();
    }

    void MovementOutput()
    {
        movementOutput.Set(horizontalOutput, verticalOutput-gravity, 0);
        playerCC.Move(movementOutput*Time.deltaTime);
    }

    void PlayerState()
    {
        if (!overdriveMove)
        {
            if (playerCC.isGrounded)
            {
                if (isInputedJump) { Jump(); desiredState = "Jump"; }
                else 
                { 
                if (isInputedHorizontalRight && !isInputedVerticalDown) { RunRight(); desiredState = "RunRight"; }
                if (isInputedHorizontalLeft && !isInputedVerticalDown) { RunLeft(); desiredState = "RunLeft"; }
                if (!isInputedHorizontalLeft && !isInputedHorizontalRight && !isInputedVerticalDown) { Idle(); desiredState = "Idle"; }
                if (!isInputedHorizontalLeft && !isInputedHorizontalRight && isInputedVerticalDown) { CrouchIdle(); desiredState = "CrouchIdle"; }
                if (isInputedHorizontalRight && isInputedVerticalDown) { CrouchRunRight(); desiredState = "CrouchRunRight"; }
                if (isInputedHorizontalLeft && isInputedVerticalDown) { CrouchRunLeft(); desiredState = "CrouchRunLeft"; }
                }
            }
            if (!playerCC.isGrounded)
            {
                if (isInputedJump && playerCC.velocity.y > 0) { Jump(); desiredState = "Jump"; }
                else { Fall(); desiredState = "Fall"; }
            }

        }
        else
        {
            
        }

    
    }

    void InputManager()
    {
        if (Input.GetAxisRaw("Horizontal") > 0) { isInputedHorizontalRight = true; isInputedHorizontalLeft = false; _releasedHorizontalInput = noXInputDelay; }
        if (Input.GetAxisRaw("Horizontal") < 0) { isInputedHorizontalRight = false; isInputedHorizontalLeft = true; _releasedHorizontalInput = noXInputDelay; }
        if (Input.GetAxisRaw("Horizontal") == 0 && _releasedHorizontalInput <= 0) { isInputedHorizontalRight = false; isInputedHorizontalLeft = false; }

        if (Input.GetAxisRaw("Vertical") > 0) { isInputedVerticalUp = true; isInputedVerticalDown = false; }
        if (Input.GetAxisRaw("Vertical") < 0) { isInputedVerticalDown = true; isInputedVerticalUp = false; }
        if (Input.GetAxisRaw("Vertical") == 0) { isInputedVerticalUp = false; isInputedVerticalDown = false; }


        // special line for minimal jump delay
        if (Input.GetButtonDown("Jump") && playerCC.isGrounded) { _minimalJumpTimer = minimalJumpDelay; }
        if (Input.GetButton("Jump")) { isInputedJump = true; }
        if (!Input.GetButton("Jump") && _minimalJumpTimer <= 0) { isInputedJump = false; }

        if (Input.GetAxisRaw("Horizontal") > 0) { playerIsRight = true; }
        if (Input.GetAxisRaw("Horizontal") < 0) { playerIsRight = false; }
    }
    
    // Action Function & Animation Function
    void Jump()
    {
        if (desiredState == "Jump")
        {
            
            AnimJump();
            verticalOutput = jumpingCurve.Evaluate(_curveTimer) * jumpForce;
            horizontalOutput = horizontalSpeed * Input.GetAxis("Horizontal");
        }
    }

    void AnimJump()
    {
        if (desiredState == "Jump" && currentAnimState != "animJump")
        {

            CurveReset();
            currentAnimState = "animJump";
            if (isLastOrientationRight == true && playerIsRight == true) { playerAnimator.SetTrigger("trigJumpRight"); isLastOrientationRight = true; }
            if (isLastOrientationRight == true && playerIsRight == false) { playerAnimator.SetTrigger("trigJumpLeft"); isLastOrientationRight = false; }
            if (isLastOrientationRight == false && playerIsRight == true) { playerAnimator.SetTrigger("trigJumpRight"); isLastOrientationRight = true; }
            if (isLastOrientationRight == false && playerIsRight == false) { playerAnimator.SetTrigger("trigJumpLeft"); isLastOrientationRight = false; }

        }

    }
    void Fall()
    {
        if (desiredState == "Fall")
        {
            
            AnimFall();
            verticalOutput = -fallingCurve.Evaluate(_curveTimer) *fallForce;
            horizontalOutput = horizontalSpeed * Input.GetAxis("Horizontal");
        }
    }
  

    void AnimFall()
    {
        if (desiredState == "Fall" && currentAnimState != "animFall")
        {
            CurveReset();
            currentAnimState = "animFall";
            if(isLastOrientationRight == true) { playerAnimator.SetTrigger("trigFallRight"); }
            if (isLastOrientationRight == false) { playerAnimator.SetTrigger("trigFallLeft"); }

        }

    }

    void RunCoolDown()
    {
        if (_runCooldownTimer >= 0)
        { isRunOnCoolDown = true; }
        if (_runCooldownTimer < 0)
        { isRunOnCoolDown = false; }
    }

    void RunRight()
    {
        RunCoolDown();
        if (desiredState == "RunRight"  && isRunOnCoolDown == false)
        {
            AnimRunRight();
            verticalOutput = 0;
            horizontalOutput = horizontalSpeed * Input.GetAxis("Horizontal");
        }
    }
    void RunLeft()
    {
        RunCoolDown();
        if (desiredState == "RunLeft" && isRunOnCoolDown == false)
        {
            AnimRunLeft();
            verticalOutput = 0;
            horizontalOutput = horizontalSpeed * Input.GetAxis("Horizontal");
        }
    }


  
    void AnimRunRight()
    {
        if (desiredState == "RunRight" && currentAnimState != "animRunRight" && playerIsRight == true)
        {
            _runCooldownTimer = runCooldownDelay;
            currentAnimState = "animRunRight";
            playerAnimator.SetTrigger("trigRunRight");
            isLastOrientationRight = true;
        }
    }
    void AnimRunLeft()
    {
        if (desiredState == "RunLeft" && currentAnimState != "animRunLeft" && playerIsRight == false)
        {
            _runCooldownTimer = runCooldownDelay;
            currentAnimState = "animRunLeft";
            playerAnimator.SetTrigger("trigRunLeft");
            isLastOrientationRight = false;

        }

    }

    void CrouchRunRight()
    {
        RunCoolDown();
        if (desiredState == "CrouchRunRight" && isRunOnCoolDown == false)
        {
            AnimCrouchRunRight();
            verticalOutput = 0;
            horizontalOutput = horizontalSpeed * Input.GetAxis("Horizontal");
        }
    }
    void CrouchRunLeft()
    {
        RunCoolDown();
        if (desiredState == "CrouchRunLeft" && isRunOnCoolDown == false)
        {
            AnimCrouchRunLeft();
            verticalOutput = 0;
            horizontalOutput = horizontalSpeed * Input.GetAxis("Horizontal");
        }
    }

    void AnimCrouchRunRight()
    {
        if (desiredState == "CrouchRunRight" && currentAnimState != "animCrouchRunRight" )
        {
            _runCooldownTimer = runCooldownDelay;
            currentAnimState = "animCrouchRunRight";
            playerAnimator.SetTrigger("trigCrouchRunRight");
            isLastOrientationRight = true;

        }
    }
    void AnimCrouchRunLeft()
    {
        if (desiredState == "CrouchRunLeft" && currentAnimState != "animCrouchRunLeft")
        {
            _runCooldownTimer = runCooldownDelay;
            currentAnimState = "animCrouchRunLeft";
            playerAnimator.SetTrigger("trigCrouchRunLeft");
            isLastOrientationRight = false;

        }
    }


    void Idle()
    {
        if (desiredState == "Idle" )
        {
            AnimIdle();
            horizontalOutput = horizontalSpeed * Input.GetAxis("Horizontal");
        }
    
    }
    void AnimIdle()
    {
        if (desiredState == "Idle" && currentAnimState != "animIdle")
        {
            currentAnimState = "animIdle";
            if (isLastOrientationRight == true) { playerAnimator.SetTrigger("trigIdleRight"); }
            if (isLastOrientationRight == false) { playerAnimator.SetTrigger("trigIdleLeft"); }

        }

    }

    void CrouchIdle()
    {
        if (desiredState == "CrouchIdle")
        {
            AnimCrouchIdle();
            horizontalOutput = horizontalSpeed * Input.GetAxis("Horizontal");
        }

    }
    void AnimCrouchIdle()
    {
        if (desiredState == "CrouchIdle" && currentAnimState != "animCrouchIdle")
        {
            currentAnimState = "animCrouchIdle";
            if (isLastOrientationRight == true) { playerAnimator.SetTrigger("trigCrouchIdleRight"); }
            if (isLastOrientationRight == false) { playerAnimator.SetTrigger("trigCrouchIdleLeft"); }

        }

    }

    // OverDrive Move
    void OverDriveState()
    {
        if ( bigFall == false )
        { overdriveMove = false; }
        else
        { overdriveMove = false; }
    }

    void DashMove()
    {
        if (desiredState == "Dash")
        {
            AnimLand();
            horizontalOutput = 0;
            if (_landTimer < 0)
            {
                bigFall = false;
            }


        }
    }

    void Land()
    {
        if (desiredState == "Land")
        {
            AnimLand();
            horizontalOutput = 0;
            if (_landTimer < 0)
            {
                bigFall = false;
            }
            
            
        }
    }

    void AnimLand()
    {
        if (desiredState == "Land" && currentAnimState != "animLand")
        {
            currentAnimState = "animLand";
            if (isLastOrientationRight == true) { playerAnimator.SetTrigger("trigLandRight"); }
            if (isLastOrientationRight == false) { playerAnimator.SetTrigger("trigLandLeft"); }

        }
    }

   

    // Time & Curves

    void CurveReset()
    {
        _curveTimer = 0;
    }

    void Timer()
    {
        _curveTimer += Time.deltaTime;
        _runCooldownTimer -= Time.deltaTime;
        _releasedHorizontalInput -= Time.deltaTime;
        _minimalJumpTimer -= Time.deltaTime;
        _landTimer -= Time.deltaTime;
    }

}
