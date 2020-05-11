using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Prog.Script
{
    public class PlayerBehaviorV2 : MonoBehaviour
    {
        public float horizontalSpeed = 4f;
        public float fallForce = 18f;
        public float gravity = 20f;
        [FormerlySerializedAs("jumpSpeed")] public float jumpForce = 9f;
        
        private CharacterController _characterController;
        private Animator _playerAnimator;
        private static readonly int Horizontal = Animator.StringToHash("horizontal");
        private static readonly int Jumping = Animator.StringToHash("jumping");
        private Vector3 _moveDirection = Vector3.zero;
        private bool _isJumping; 
        private bool IsGrounded => _characterController.isGrounded;
        public bool _isGrounded;
  
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _playerAnimator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            _isGrounded = _characterController.isGrounded;
            if (IsGrounded && !_isJumping)
            {
                _moveDirection.y = -0.1f;
            }
            if (!IsGrounded)
            {
                CharacterFall();
            }
            MoveCharacter();
        }
    
        private void CharacterFall()
        {
            _moveDirection.y -= fallForce * Time.deltaTime;
        }
    
        private void MoveCharacter()
        {
            var mouvementThisFrame = _moveDirection * Time.deltaTime;
            _characterController.Move(mouvementThisFrame);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var joystickMovement = context.ReadValue<Vector2>();
            _playerAnimator.SetFloat(Horizontal, joystickMovement.x);
            if (context.performed)
            {
                joystickMovement = context.ReadValue<Vector2>();
                _moveDirection.x = joystickMovement.x;
                _moveDirection.x *= horizontalSpeed;
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
                _playerAnimator.SetBool(Jumping, false);
                _isJumping = false;
            }
        }

        private void Jump()
        {
            if (IsGrounded && !_isJumping)
            {
                _playerAnimator.SetBool(Jumping, true);
                _moveDirection.y = jumpForce;
                _moveDirection.y -= gravity * Time.deltaTime;
                _isJumping = true;
            }
        }
    }
}
    


