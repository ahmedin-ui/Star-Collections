using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float PlayerSpeed;
    public float JumpHieght = 4f;
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
            playerAnim.SetTrigger("Jump");
            playerRb.AddForce(Vector3.up * JumpHieght, ForceMode.Impulse);
        }
        // while falling 
        if (!isPlatform && playerRb.velocity.y < 0)
        {
            playerAnim.SetBool("isFalling", true);
        }
        if (transform.position.y > 10f) // you can adjust 10f to your scene height
        {
            playerRb.velocity = Vector3.zero; // stop upward motion
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            isPlatform = true;
            playerAnim.SetBool("isJumping", false);
            playerAnim.SetBool("isFalling", false);
        }
    }
    void FixedUpdate()
{
    // extra gravity force to make player fall faster
    playerRb.AddForce(Vector3.down * 65f); // increase 20f → stronger gravity
}
}
