using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Prog.Script
{
    public class PlayerBehavior : MonoBehaviour
    {
        public float horizontalSpeed = 4f;
        public float fallForce = 18f;
        public float gravity = 20f;
        [FormerlySerializedAs("jumpSpeed")] public float jumpForce = 9f;
        public float groundDistance = 0.2f;
        [FormerlySerializedAs("Ground")] public LayerMask groundLayerMask; // on vient indiquer ce qu'est le ground
        [SerializeField]PlayerCombat playerCombat;

        private CharacterController _characterController;
        private Animator _playerAnimator;
        private static readonly int Horizontal = Animator.StringToHash("horizontal");
        private static readonly int Jumping = Animator.StringToHash("jumping");
        private static readonly int Attacking = Animator.StringToHash("attacking");
        private static readonly int Landing = Animator.StringToHash("landing");
        private Vector3 _moveDirection = Vector3.zero;
        private bool _isJumping;
        private Transform _groundChecker;
        private bool _isGrounded = true;
        private bool _lastFrameGrounded = true;
        
        void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _playerAnimator = GetComponent<Animator>();
            _groundChecker = transform.GetChild(0);
        }
        
        void Update()
        {
            _isGrounded = Physics.CheckSphere(_groundChecker.position, groundDistance, groundLayerMask, QueryTriggerInteraction.Ignore);
            CheckIfLanded();
            if (!_isGrounded)
            {
                CharacterFall();
            }
            MoveCharacter();
        }

        private void CheckIfLanded()
        {
            // check if lastFrameGrounded est pas null 
            // si !lastFrameGrounded et currentFrameGrounded -> on start l'anim de landing
            if ((!_lastFrameGrounded &&  _isGrounded) || (_lastFrameGrounded && _isGrounded))
            {
                _playerAnimator.SetBool(Landing, true);
            }
            else
            {
                 _playerAnimator.SetBool(Landing, false);    
            }
            _lastFrameGrounded = _isGrounded;
        }
    
        private void CharacterFall()
        {
            _moveDirection.y -= fallForce * Time.deltaTime;
        }
    
        private void MoveCharacter()
        {
            var zPosition = 0;
            var mouvementThisFrame = _moveDirection * Time.deltaTime;
            if(Math.Abs(transform.position.z - zPosition) > 0.01)
            {
                mouvementThisFrame.z = (float)((zPosition - transform.position.z) * 0.05f);
            }
            
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

            if (!context.canceled) return;
            _playerAnimator.SetBool(Jumping, false);
            _isJumping = false;
        }

        private void Jump()
        {
            if (!_isGrounded || _isJumping) return;
            _playerAnimator.SetBool(Jumping, true);
            _moveDirection.y = jumpForce;
            _moveDirection.y -= gravity * Time.deltaTime;
            _isJumping = true;
        }
        
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PerformAttack();
            }

            if (context.canceled)
            {
                CancelAttack();
            }
        }

        public void OnChargeAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                CancelAttack();
            }
        }

        private void PerformAttack()
        {
            playerCombat.StartAttackTimer();
            _playerAnimator.SetBool(Attacking, true);
        }

        private void CancelAttack()
        {
            _playerAnimator.SetBool(Attacking, false);
        }
    }
}
    


