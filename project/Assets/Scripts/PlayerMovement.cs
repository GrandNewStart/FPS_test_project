using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Properties")]
    public float speed = 0f;
    public int stepInterval = 0;
    public AudioSource walk = null;
    private int walkStep = 0;

    [Header("Fall Properties")]
    public float gravity = 9.81f;
    public float jumpHeight = 0f;
    public CharacterController characterController = null;
    public GameObject groundCheck;
    public LayerMask groundMask;
    private float verticalVelocity = 0f;
    private bool isGrounded = true;
    private float groundDistance = 0.4f;

    void Update()
    {
        Move();
        Jump();
    }

    void Move()
    {
        float moveV = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float moveH = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        Vector3 move = transform.right * moveH + transform.forward * moveV;

        characterController.Move(move);
        StepNoise();
    }

    void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, groundDistance, groundMask);

        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;

            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpHeight;
            }
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 jumpVector = new Vector3(0f, verticalVelocity, 0f);
        characterController.Move(jumpVector * Time.deltaTime);
    }

    void StepNoise()
    {
        if (walkStep == stepInterval)
        {
            if (isGrounded)
            {
                walk.Play();
                walkStep++;
            }
        }
        if (walkStep > stepInterval)
        {
            walkStep = 0;
        }

        if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
        {
            walkStep++;
        }
    }
}
