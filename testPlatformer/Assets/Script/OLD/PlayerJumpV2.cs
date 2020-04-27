using UnityEngine;

public class PlayerJumpV2 : MonoBehaviour
{
    public CharacterController playerCC;
    public float maxJumpHeight;
    public float jumpDecay;
    public float jumpForce;
    public float jumpVelocity;
    float deltaJumpVelocity;
    float deltaJumpDecay;
    bool isJumping;

    void Start()
    {
        playerCC = GetComponent<CharacterController>();
    }
    void Update()
    {
        deltaJumpDecay = jumpDecay * Time.deltaTime;
       
        if (playerCC.isGrounded)
        {
            jumpVelocity = jumpForce;
        }
        if (playerCC.isGrounded && Input.GetButton("Jump"))
        {
            isJumping = true;
        }

        if (isJumping == true && Input.GetButton("Jump"))
        {
            if (jumpVelocity > 1)
            {
                deltaJumpVelocity = jumpVelocity * jumpDecay *Time.deltaTime ;
                jumpVelocity -= deltaJumpVelocity;
            }
           if (jumpVelocity <= 1)
           {
               jumpVelocity = 0;
           }
            print("isJumping");
            playerCC.Move(transform.up * jumpVelocity * Time.deltaTime);
        }
        if (isJumping == true && !Input.GetButton("Jump"))
        {
            print("toFall");
            isJumping = false;
        }



    }


}
