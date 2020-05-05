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
    public float gravity = 2f;
    private static readonly int Horizontal = Animator.StringToHash("horizontal");
    private static readonly int jumping = Animator.StringToHash("jumping");

    private Rigidbody Rigidbody => GetComponent<UnityEngine.Rigidbody>();
    private Vector3 m_MoveDirection;

    private bool m_IsMoving;
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
        if (m_PlayerCc.isGrounded && m_IsMoving)
        {
            MoveCharacter();
        }
      
        if (!m_PlayerCc.isGrounded)
        {
            CharacterFall();
        }
    }

    private void CharacterFall()
    {
        Vector3 fall = new Vector3(0, -fallForce, 0);
        m_PlayerCc.Move(fall);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var joystickMovement = context.ReadValue<Vector2>();
        m_PlayerAnimator.SetFloat(Horizontal, joystickMovement.x);
        if (context.performed)
        {
            joystickMovement = context.ReadValue<Vector2>();
            m_MoveDirection = new Vector3(joystickMovement.x, 0, 0);
            m_MoveDirection *= horizontalSpeed;
            m_IsMoving = true;
            //Debug.Log("Current movement = " + m_MoveDirection);
        }
        if (context.canceled)
        {
            m_IsMoving = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            m_PlayerAnimator.SetBool(jumping, true);
        }

        if (context.canceled)
        {
            m_PlayerAnimator.SetBool(jumping, false);
        }
    }
    private void MoveCharacter()
    {
        m_PlayerCc.Move(m_MoveDirection * Time.deltaTime);
    }
}
    


