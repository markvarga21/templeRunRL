using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlayerController : Agent
{
    [SerializeField]
    private Rigidbody rb;
    private float jumpForce = 6.0f;
    private float movementDistance = 1.5f;
    private int playerPosition = 0;

    public bool isGrounded = false;

    private Animator animator;
    private Vector3 playerInitPosition;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerInitPosition = transform.position;
    }

    override public void OnEpisodeBegin()
    {
        playerPosition = 0;
        transform.position = playerInitPosition;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
    }

    private void FixedUpdate()
    {
        if (isGrounded)
        {
           RequestDecision();
        }
        AddReward(0.01f);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.DiscreteActions;
        int actionJump = Mathf.FloorToInt(actionTaken[0]);
        int actionMove = (actionTaken[1]*2)-1;

        if (actionJump == 1)
        {
            Jump();
        }
        //MoveSideways(actionMove);
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        
    }

    void Jump()
    {
        if (isGrounded)
        {
            animator.SetBool("isJumping", true);
            rb.AddForce(Vector3.up*jumpForce, ForceMode.VelocityChange);
            isGrounded = false;
        }   
    }

    void MoveSideways(int direction)
    {
        if (direction == -1 && playerPosition >= 0)
        {
            //Debug.Log("Action move: " + direction + ", playerPosition = " + playerPosition);
            transform.position = new Vector3(transform.position.x + (movementDistance * direction), transform.position.y, transform.position.z);
            playerPosition += direction;
        } else if (direction == 1 && playerPosition <= 0)
        {
            //Debug.Log("Action move: " + direction + ", playerPosition = " + playerPosition);
            transform.position = new Vector3(transform.position.x + (movementDistance * direction), transform.position.y, transform.position.z);
            playerPosition += direction;
        }
    }

    //void Update()
    //{
    //    if (Input.GetButtonDown("Jump") && isGrounded)
    //    {
    //        Jump();
    //    }

    //    if (Input.GetKeyDown("a"))
    //    {
    //        MoveSideways(-1);
    //    }

    //    if (Input.GetKeyDown("d"))
    //    {
    //        MoveSideways(1);
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    private void ClearObstacles()
    {
        GameObject[] singleObstacles = GameObject.FindGameObjectsWithTag("singleBox");
        foreach (GameObject obstacle in singleObstacles)
        {
            Destroy(obstacle);
        }
        GameObject[] doubleObstacles = GameObject.FindGameObjectsWithTag("doubleBox");
        foreach (GameObject obstacle in doubleObstacles)
        {
            Destroy(obstacle);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("singleBox") || other.gameObject.CompareTag("doubleBox") || other.gameObject.CompareTag("Barrier"))
        {
            //Debug.Log("Hit obstacle");
            AddReward(-1.0f);
            ClearObstacles();
            EndEpisode();
        }

        if (other.gameObject.CompareTag("emptyBox"))
        {
            AddReward(1.0f);
        }
    }
}
