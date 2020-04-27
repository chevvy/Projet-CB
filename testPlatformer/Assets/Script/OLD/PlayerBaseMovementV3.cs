using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseMovementV3 : MonoBehaviour
{
    public CharacterController playerCC;
    public AnimationCurve jumpCurve;

    public Vector3 movementOutput = new Vector3(0f, 0f, 0f);

    public float movementSpeed = 5f;
    float horizontalMovement;

    public float jumpOutput ;
    public float jumpHeight = 2;

    public bool jumpOn;
   float jumpOrigin;
   public float jumpTop;
   public float jumpSpeed = 10f;
   public float jumpDecayFactor;
    public float jumpMinimalValueFactor ;
   public float jumpPercentage;
   public float debugPositionY;
    public float lerpTarget;
    public float curveVelocity = 0.0f;
    public float curveTime;
    public float debugDampCurve;

    float _jumpCurveTimer;

    public float earlyJumpDelay = 0.1f;
    float _earlyJumpTimer;
    float _nearFloorTimer;
    public float nearFloorDelay = 0.1f;

    public float gravity = 5;
    float deltaGravity;


    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        jumpOn = false;
        _earlyJumpTimer = 0;

    }


    void Update()
    {
        MovementHorizontal();
        JumpStateMachine();
        MovementOutput();

        Timer();
    }
    private void FixedUpdate()
    {
        
        JumpCurve();
    }

    void MovementHorizontal()
    {
        horizontalMovement = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;


    }

    void Timer()
    {
        // Early Timer
        if (_earlyJumpTimer >= 0)
        {
            _earlyJumpTimer -= Time.deltaTime;
        }
        // Near Floor Timer
        if (_nearFloorTimer >= 0)
        {
            _nearFloorTimer -= Time.deltaTime;
        }
        // Jump Curve Timer
        if (_jumpCurveTimer  < 10)
        {
            _jumpCurveTimer += Time.deltaTime;
        }
    }
    
    void  JumpStateMachine()
    {
       

        // Swapped the Input.Getbutton "Jump" to InputTimer
        if (Input.GetButtonDown("Jump"))
        {
            
            _nearFloorTimer = nearFloorDelay;
        }
        // Input Read, Grounded, Ready To Jump , to add 
        if (playerCC.isGrounded && _nearFloorTimer>=0)
        {
            _jumpCurveTimer = 0;
            jumpOrigin = playerCC.transform.position.y;
            jumpTop = jumpOrigin + jumpHeight;
            jumpOn = true;
            _earlyJumpTimer = earlyJumpDelay;
        }
        // Falling Condition  
        if (!playerCC.isGrounded && _earlyJumpTimer <= 0)
        {
            Falling();
        }
        // Input Read Neutral Mid-air 
        if (!playerCC.isGrounded && !Input.GetButton("Jump"))
        {
            jumpOn = false;
        }
        // Jumping
        if (jumpOn == true)
        {
            if (playerCC.transform.position.y > jumpTop)
            {
                jumpOn = false;
            }
            Jump();
        }
        
        
         

    }
    void JumpCurve()
    {
        jumpPercentage = 1-jumpCurve.Evaluate(_jumpCurveTimer);
    }
    void JumpReset()
    {
    }
    void Jump()
    {
        jumpOutput = jumpCurve.Evaluate(_jumpCurveTimer);
            //jumpOutput = jumpSpeed * Time.deltaTime;
             
    }


    void Falling()
    {
            jumpOutput = -gravity * Time.deltaTime;
        
    }

    // ici on a la fourchette des 4 axes
    void MovementOutput()
    {
        movementOutput.Set(horizontalMovement, jumpOutput, 0f);
        playerCC.Move(movementOutput) ;
    }

}
