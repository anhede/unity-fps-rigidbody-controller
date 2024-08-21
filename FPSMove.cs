using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMove : MonoBehaviour
{
    public bool IsGrounded { get { return isGrounded; } }

    [Header("Movement")]
    public float acceleration = 10f;
    public float maxSpeed = 5f;
    public float friction = 0.9f;
    public float jumpSpeed = 5f;

    [Header("Grounding")]
    public LayerMask groundLayer = -1; // Default to all
    public Vector3 groundCheckStart;
    public Vector3 groundCheckEnd;
    public float groundCheckRadius = 0.1f;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = (
            transform.right * moveHorizontal + transform.forward * moveVertical
        ).normalized;
        rb.AddForce(movement * acceleration * Time.deltaTime, ForceMode.VelocityChange);

        // friction
        if (movement.sqrMagnitude < 0.01f)
        {
            Vector3 velocity = rb.velocity;
            velocity.x *= friction;
            velocity.z *= friction;
            rb.velocity = velocity;
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
        }

        // Double gravity when not jumping
        if (!isGrounded && !Input.GetButton("Jump"))
        {
            rb.AddForce(Physics.gravity * Time.deltaTime, ForceMode.VelocityChange);
        }
    }

    void FixedUpdate()
    {
        // Clamp the velocity to the max speed, ignoring the y-component
        Vector3 velocity = rb.velocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);

        if (horizontalVelocity.magnitude > maxSpeed)
        {
            horizontalVelocity = horizontalVelocity.normalized * maxSpeed;
            rb.velocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
        }

        // Check if grounded
        isGrounded = Physics.SphereCast(
            transform.TransformPoint(groundCheckStart),
            groundCheckRadius,
            transform.TransformDirection(groundCheckEnd),
            out RaycastHit hit,
            groundCheckEnd.magnitude,
            groundLayer
        );
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            transform.TransformPoint(groundCheckStart),
            groundCheckRadius
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.TransformPoint(groundCheckStart) + transform.TransformDirection(groundCheckEnd),
            groundCheckRadius
        );
    }
}
