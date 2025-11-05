using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerSpeed = 5f;
    public float JumpHieght = 6f;
    public float HorizontalInput;
    private Rigidbody playerRb;
    private Animator playerAnim;
    private bool isPlatform = true;
    

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.forward * HorizontalInput * Time.deltaTime * PlayerSpeed);
        playerAnim.SetFloat("Speed", Mathf.Abs(HorizontalInput));
        // jump input 
        if (Input.GetKeyDown(KeyCode.Space) && isPlatform)
        {
            isPlatform = false;
            playerAnim.SetBool("isJumping", true);
            playerRb.AddForce(Vector3.up * JumpHieght, ForceMode.Impulse);
        }
        // while falling 
    
    }
}
