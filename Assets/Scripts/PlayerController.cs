using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    private float jumpForce = 60.0f;
    private float movementDistance = 1.5f;
    private int playerPosition = 0;

    public bool isGrounded = true;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    void Start()
    {

    }

    void Jump()
    {
        animator.SetBool("isJumping", true);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 
        isGrounded = false;
    }

    void MoveSideways(int direction)
    {
        if (direction == -1 && playerPosition == -1 || direction == 1 && playerPosition == 1)
        {
            return;
        }
        else
        {
            transform.position = new Vector3(transform.position.x + (movementDistance * direction), transform.position.y, transform.position.z);
            playerPosition += direction;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        if (Input.GetKeyDown("a"))
        {
            MoveSideways(-1);
        }

        if (Input.GetKeyDown("d"))
        {
            MoveSideways(1);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("singleBox"))
        {
            Debug.Log("Game Over");
        }
    }
}
