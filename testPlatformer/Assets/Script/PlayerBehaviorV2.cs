using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviorV2 : MonoBehaviour
{
    public CharacterController playerCC;
    public Animator playerAnimator;
    public float horizontalSpeed = 1;
    public float fallForce = 1f;
    public float gravity = 2f;
    private static readonly int Horizontal = Animator.StringToHash("horizontal");

    private Rigidbody Rigidbody => GetComponent<UnityEngine.Rigidbody>();
    private Vector3 m_LastMovement;

    private bool m_isMoving;
    // Start is called before the first frame update
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isMoving)
        {
            playerCC.Move(m_LastMovement);
            Debug.Log("Player moved " + m_LastMovement);
        }
        
        if (!playerCC.isGrounded)
        {
            Vector3 fall = new Vector3(0, -fallForce, 0);
            playerCC.Move(fall);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // faut trouver un moyen d'utiliser le On HOLD du input manager pour permettre de 
        // tenir le stick et de continuer à avancer 
        var joystickMovement = context.ReadValue<Vector2>();
        playerAnimator.SetFloat(Horizontal, joystickMovement.x);
        if (context.performed)
        {
            m_LastMovement = new Vector3(joystickMovement.x * Time.deltaTime * horizontalSpeed, 0, 0f);
            m_isMoving = true;
            //Debug.Log("Performed of x  = " + joystickMovement.x);
        }

        if (context.canceled)
        {
            m_isMoving = false;
        }
    }
    
}
