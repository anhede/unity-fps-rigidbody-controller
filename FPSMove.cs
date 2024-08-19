using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMove : MonoBehaviour
{
    public float acceleration = 50f;
    public float friction = 0.9f;
    public float walkMaxSpeed = 2f;
    public float sprintMaxSpeed = 4f;
    private Rigidbody rb;
    private float maxSpeed
    {
        get { return Input.GetButton("Sprint") ? sprintMaxSpeed : walkMaxSpeed; }
    }

    void Awake()
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
    }
}
