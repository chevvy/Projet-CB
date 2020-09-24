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
        public float jumpForceResult;
        public AnimationCurve jumpCurve;
        public float jumpIndexTimer=1f;
        public float groundDistance = 0.2f;
        [FormerlySerializedAs("Ground")] public LayerMask groundLayerMask; // on vient indiquer ce qu'est le ground
        [SerializeField] private PlayerCombat playerCombat = null;
        public float attackRotationDelay = 0.5f;
        public float jumpDelay = 0.1f; 

        private float timerAttackRotation;
        private CharacterController _characterController;
        private Animator _playerAnimator;
        private static readonly int Horizontal = Animator.StringToHash("horizontal");
        private static readonly int Jumping = Animator.StringToHash("jumping");
        private static readonly int Attacking = Animator.StringToHash("attacking");
        private static readonly int Landing = Animator.StringToHash("landing");
        private static readonly int AttackRotation = Animator.StringToHash("attackRotation");
        private static readonly int Air = Animator.StringToHash("air");
        private Vector3 _moveDirection = Vector3.zero;
        private bool _isJumping;
        private Transform _groundChecker;
        private bool _isGrounded = true;
        private bool _lastFrameGrounded = true;
        private bool _isAttackSwitched = false;
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
            if (!_isGrounded) { CharacterFall(); }
            MoveCharacter();
            JumpCurveIndex();
            timerAttackRotation =+ Time.deltaTime;
            
        }

        private void CheckIfLanded()
        {
            // check if lastFrameGrounded est pas null 
            // si !lastFrameGrounded et currentFrameGrounded -> on start l'anim de landing
            if ((!_lastFrameGrounded &&  _isGrounded))
            {
                OnLanding();
            } 
            else if((!_lastFrameGrounded &&  _isGrounded)|| (_lastFrameGrounded && _isGrounded))
            {
                _playerAnimator.SetBool(Landing, true);
                _playerAnimator.SetBool(Air, false);
            }
            else
            {
                 _playerAnimator.SetBool(Landing, false);
                 _playerAnimator.SetBool(Air, true);
            }
            _lastFrameGrounded = _isGrounded;
        }

        private void OnLanding()
        {
            _playerAnimator.SetBool(Landing, true);
            _playerAnimator.SetBool(Air, false);
            if(_moveDirection.y < 0) AudioManager.Instance.PlaySound("Landing_metal", true);
            _moveDirection.y = 0; // comme ça, il n'y a pas de force qui accumulé si on saute pas avant la prochaine chute
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
                 jumpIndexTimer = 0f;
            }

            if (!context.canceled) return;
            _playerAnimator.SetBool(Jumping, false);
            _isJumping = false;
        }

        private void JumpCurveIndex()
        {
            jumpForceResult = jumpForce*jumpCurve.Evaluate(jumpIndexTimer);
            jumpIndexTimer += Time.deltaTime ;
        
        }
        private void Jump()
        {
            if (!_isGrounded || _isJumping) return;
            _playerAnimator.SetBool(Jumping, true);
            _moveDirection.y = jumpForceResult;
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
            AudioManager.Instance.PlaySound("Strike", true);
            playerCombat.StartAttackTimer();
            if (_isAttackSwitched)
            {
                _playerAnimator.SetBool(AttackRotation, true);
                _isAttackSwitched = false;
                
            }
            else 
            {
                _playerAnimator.SetBool(AttackRotation, false);
                _isAttackSwitched = true;
            }
            

            _playerAnimator.SetBool(Attacking, true);
           

            

        }

        private void CancelAttack()
        {
            _playerAnimator.SetBool(Attacking, false);
        }
    }
}
    


