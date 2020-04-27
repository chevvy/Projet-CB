using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseMovementV2 : MonoBehaviour
{
    Vector3 playerDirection = new Vector3(0f, 0f, 0f);
    public CharacterController playerCC;
    public float horizontalSpeed;
    
    public float horiztonalFormula;
    public float verticalFormula;
    public float jumpFormula;
    public float gravityFormula;

    public float gravity;
   
    public float jumpHeight;
    public float jumpSpeed;
    public float jumpDecay;
    
    public float jumpMath;
    public float height;

    public float _jumpDelayTimer;
    public float _jumpRefreshTimer;
    public float jumpDelay;
    public float jumpRefreshDelay;
    bool jumpRefreshed;
    bool initialiseJumping;
    bool isJumping;
    bool isAbleToJump;
    

    void Start()
    {
        _jumpRefreshTimer = jumpRefreshDelay;
        _jumpDelayTimer = jumpDelay;
        playerCC = GetComponent<CharacterController>();
    }

    void Update()
    {
        playerDirection.Set(horiztonalFormula, jumpFormula -gravityFormula  , 0f);
        horiztonalFormula = horizontalSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;

        gravityFormula = gravity * Time.deltaTime;


        Jump();
        
        
        
     

        playerCC.Move(playerDirection);
    }



    void Jump()
    { 
        if (playerCC.isGrounded)
        {
            
            
            if (_jumpRefreshTimer < 0)
            {
                
                print("is refreshed");
                height = jumpHeight;
                isAbleToJump = true;
                isJumping = false;
                initialiseJumping = false;
                jumpRefreshed = false;
                
            }
            if (_jumpRefreshTimer > 0)
            {
               _jumpRefreshTimer -= Time.deltaTime;
            }
            
        }

        if (Input.GetButtonDown("Jump") && isAbleToJump ==true)
        {
            _jumpDelayTimer = jumpDelay;
            _jumpRefreshTimer = jumpRefreshDelay;
            
            isAbleToJump = false;
            initialiseJumping = true;
            
        }

        if (initialiseJumping == true && _jumpDelayTimer > 0)
        {
            print("is timed");
            _jumpDelayTimer -= Time.deltaTime;
            
            isJumping = true;
        }
        if (_jumpDelayTimer < 0)
        {
            print("i'm out");
        }

        if (initialiseJumping == true && Input.GetButton("Jump") && jumpFormula > 0)
        {
            print("is 'hold'");
            isJumping = true;
        }

        if (isJumping ==true && (jumpFormula-gravityFormula)<0 && _jumpDelayTimer<0)
        {
            print(" appex reached");
            isJumping = false;
        }

        if (!Input.GetButton("Jump") && _jumpDelayTimer <0)
        {
          
            print("is released");
            isJumping = false;
        }


        if (isJumping == true)
        {
            print("is jumping");
            jumpFormula = height * Time.deltaTime;
            height = height * jumpDecay;
        }
        if (isJumping == false)
        {
            
            jumpFormula = 0;
        }
    }

  /*  void Jump()
    {
        if (Input.GetButton("Jump") && height>1)
        {
            print("jump pressed");
            jumpFormula = height * Time.deltaTime;
            height = height * jumpDecay;
        }
        if (Input.GetButton("Jump") && verticalFormula < 0)
        {
            print("anti float");
            jumpFormula = 0;
        }
        if (!Input.GetButton("Jump"))
        {

            jumpFormula = 0;
            height = jumpHeight;
        }
      

        verticalFormula = jumpFormula - gravityFormula;
    }

*/

}
