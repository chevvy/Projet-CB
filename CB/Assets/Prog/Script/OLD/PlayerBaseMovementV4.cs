using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseMovementV4 : MonoBehaviour
{
    public CharacterController playerCC;
    public AnimationCurve jumpCurve;
    public AnimationCurve fallCurve;


    
    public Vector3 movementOutput;

    static float movementHorizontalOutput;
    public float speedHorizontal = 5f;
    static float movementVerticalOutput;

    bool movementFreeze;

    float jumpOutput;
    float gravityOutput;

    public float jumpForce = 1.0f;
    public float fallForce = 1.0f;


    int jumpState;
    bool isJumping;
    bool reachedGround;
    public float jumpDelayForAnim = 0.5f;
    public float nearJumpDelay = 0.5f;
    public float minimalJumpTime = 0.5f;
    public float freezedDelay=0.1f;
    float _freezedHorizontalTimer;
    float _jumpDelayForAnimTimer;
    float _jumpInputTimer;
    float _minimalJumpTimer;
    float _animationJumpTimer;
    float _animationFallTimer;

    float fixedMovementVerticalOuput;
    float fixedAxis;

    void Start()
    {
        movementFreeze = false;
        playerCC = GetComponent<CharacterController>();
        isJumping = false;
    }


    void Update()
    {
        Timer();
        JumpState();
        MovementOutput();
        MovementHorizontal();

    }

    private void FixedUpdate()
    {
        FixedJump();
        FixedMovementHorizontal();
    }




   
    public  void MovementOutput()
    {
        if (movementFreeze == false)
        { 
            movementOutput = new Vector3(movementHorizontalOutput, movementVerticalOutput, 0);
            playerCC.Move(movementOutput);
        }
        if (movementFreeze == true)
        {
            
        }
    }

    void FixedMovementHorizontal()
    {
        
        movementHorizontalOutput = fixedAxis*speedHorizontal * 0.01f ;
    }

    void MovementHorizontal()
    {
        fixedAxis = Input.GetAxis("Horizontal");
        
    }


    void JumpState()
    {
        

        if (Input.GetButtonDown("Jump"))
        {
           
            _jumpDelayForAnimTimer = jumpDelayForAnim;
            _jumpInputTimer = nearJumpDelay + jumpDelayForAnim;
        }

        if (playerCC.isGrounded && _jumpInputTimer > 0)
        {
            movementFreeze = true;

            if (_jumpDelayForAnimTimer <= 0)
            {
                jumpState = 1;
                movementFreeze = false;
                isJumping = true;
            }
        }

        if (!playerCC.isGrounded && !Input.GetButton("Jump") && _minimalJumpTimer <= 0)
        {
            isJumping = false;
        }
        
        if (!playerCC.isGrounded && jumpCurve.Evaluate(_animationJumpTimer) >= 0.95)
        {
            isJumping = false;
        }

        if (isJumping == true )
        {
            _animationFallTimer = 0;
            Jump();
        }

        if (isJumping == false)
        {
            _minimalJumpTimer = minimalJumpTime;
            _animationJumpTimer = 0;
            Fall();
        }

        if (playerCC.isGrounded && reachedGround == false && jumpState == 1)
        {
            jumpState = 0;
            _freezedHorizontalTimer = freezedDelay;
            reachedGround = true;
            movementFreeze = true;
        }

        if (playerCC.isGrounded && reachedGround == true && _freezedHorizontalTimer <= 0) 
        {
            movementFreeze = false;
        }



    }

    void FixedJump()
    {
        fixedMovementVerticalOuput = (1 - jumpCurve.Evaluate(_animationJumpTimer)) * jumpForce * Time.deltaTime;
    }
    void Jump()
    {
        
        movementVerticalOutput = fixedMovementVerticalOuput;
        
    }

    void Fall()
    {
        movementVerticalOutput = -fallCurve.Evaluate(_animationFallTimer)* fallForce * Time.deltaTime;
    }

    void Timer()
    {
        _jumpDelayForAnimTimer -= Time.deltaTime;
        _jumpInputTimer -= Time.deltaTime;
        _minimalJumpTimer -= Time.deltaTime;
        _animationJumpTimer += Time.deltaTime;
        _animationFallTimer += Time.deltaTime;
    }


}
