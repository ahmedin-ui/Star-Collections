using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerSpeed = 5f;
    public float JumpHieght = 2f;
    public float HorizontalInput;
    private Rigidbody playerRb;
    private Animator playerAnim;
    

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
    
    }
}
