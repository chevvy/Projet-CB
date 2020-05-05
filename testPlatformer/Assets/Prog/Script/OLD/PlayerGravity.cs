using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    public CharacterController playerCC;
    public float gravity;

    // Start is called before the first frame update
    void Start()
    {
        playerCC = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCC.isGrounded)
        {
            playerCC.Move(-transform.up * gravity*Time.deltaTime);
        }
    }
}
