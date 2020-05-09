using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviorV2 : MonoBehaviour
{
    private CharacterController m_PlayerCc;
    private Animator m_PlayerAnimator;
    
    public float horizontalSpeed = 1;
    public float fallForce = 1f;
    public float gravity = 20f;
    public float jumpSpeed = 8f;
    private static readonly int Horizontal = Animator.StringToHash("horizontal");
    private static readonly int jumping = Animator.StringToHash("jumping");

    private Rigidbody Rigidbody => GetComponent<UnityEngine.Rigidbody>();
    private Vector3 m_MoveDirection = Vector3.zero;

    //private bool m_IsMoving;
    private bool m_IsJumping;
    // Start is called before the first frame update
    void Start()
    {
        m_PlayerCc = GetComponent<CharacterController>();
        m_PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_PlayerCc.isGrounded && !m_IsJumping)
        {
            CharacterFall();
        }
        MoveCharacter();
    }
    
    private void MoveCharacter()
    {
        m_PlayerCc.Move(m_MoveDirection * Time.deltaTime);
    }

    private void CharacterFall()
    {
        m_MoveDirection.y -= fallForce;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var joystickMovement = context.ReadValue<Vector2>();
        m_PlayerAnimator.SetFloat(Horizontal, joystickMovement.x);
        if (context.performed)
        {
            joystickMovement = context.ReadValue<Vector2>();
            m_MoveDirection.x = joystickMovement.x;
            m_MoveDirection *= horizontalSpeed;
            //m_IsMoving = true;
            //Debug.Log("Current movement = " + m_MoveDirection);
        }
        if (context.canceled)
        {
            m_MoveDirection.x = 0;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump();
        }

        if (context.canceled)
        {
            m_PlayerAnimator.SetBool(jumping, false);
            m_IsJumping = false;
        }
    }
   

    private void Jump()
    {
        if (m_PlayerCc.isGrounded && !m_IsJumping)
        {
            m_PlayerAnimator.SetBool(jumping, true);
            m_MoveDirection.y = jumpSpeed;
            m_MoveDirection.y -= gravity * Time.deltaTime;
            Debug.Log("Jumping : " + m_MoveDirection);
            m_IsJumping = true;
            //MoveCharacter();
            // m_IsJumping = false;
        }
    }
}
    


