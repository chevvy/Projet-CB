using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerBehaviorV2 : MonoBehaviour
{
    private CharacterController m_PlayerCc;
    private Animator m_PlayerAnimator;
    
    public float horizontalSpeed = 1;
    public float fallForce = 1f;
    public float gravity = 20f;
    [FormerlySerializedAs("jumpSpeed")] public float jumpForce = 8f;
    private static readonly int Horizontal = Animator.StringToHash("horizontal");
    private static readonly int jumping = Animator.StringToHash("jumping");

    private Rigidbody Rigidbody => GetComponent<UnityEngine.Rigidbody>();
    private Vector3 _moveDirection = Vector3.zero;

    //private bool m_IsMoving;
    private bool _isJumping;

    private bool _mPlayerCcIsGrounded;
    // Start is called before the first frame update
    void Start()
    {
        m_PlayerCc = GetComponent<CharacterController>();
        m_PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        _mPlayerCcIsGrounded = m_PlayerCc.isGrounded;
        //if (!m_PlayerCc.isGrounded && !m_IsJumping)
        //{
        if (!m_PlayerCc.isGrounded)
        {
            CharacterFall();
        }
        
        //}
        MoveCharacter();
    }
    
    private void MoveCharacter()
    {
        var testMove = _moveDirection * Time.deltaTime;
        m_PlayerCc.Move(testMove);
        
    }

    private void CharacterFall()
    {
        //m_MoveDirection.y -= fallForce;
        _moveDirection.y -= fallForce * Time.deltaTime;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var joystickMovement = context.ReadValue<Vector2>();
        m_PlayerAnimator.SetFloat(Horizontal, joystickMovement.x);
        if (context.performed)
        {
            joystickMovement = context.ReadValue<Vector2>();
            _moveDirection.x = joystickMovement.x;
            _moveDirection.x *= horizontalSpeed;
            //m_IsMoving = true;
            //Debug.Log("Current movement = " + m_MoveDirection);
        }
        if (context.canceled)
        {
            _moveDirection.x = 0;
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
            _isJumping = false;
        }
    }
   

    private void Jump()
    {
        if (m_PlayerCc.isGrounded && !_isJumping)
        {
            m_PlayerAnimator.SetBool(jumping, true);
            _moveDirection.y = jumpForce;
            _moveDirection.y -= gravity * Time.deltaTime;
            //Debug.Log("Jumping : " + _moveDirection);
            _isJumping = true;
        }
    }
}
    


