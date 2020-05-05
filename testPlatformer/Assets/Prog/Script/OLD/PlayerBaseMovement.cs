using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseMovement : MonoBehaviour
{
    public float moveSpeed = 1.0f;
    public CharacterController playerCC;
    bool rightInput;
    bool leftInput;
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
    }

   
    void Update()
    {   //Stop

        if (Input.GetAxisRaw("Horizontal") == 0)
        {
            
        }
        //Right
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            playerCC.SimpleMove(transform.right*moveSpeed);
        }
        //Left
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            playerCC.SimpleMove(-transform.right*moveSpeed );
        }
    }


}
