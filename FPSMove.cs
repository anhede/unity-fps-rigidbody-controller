using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMove : MonoBehaviour
{
    public float acceleration = 50f;
    public float friction = 0.9f;
    public float walkMaxSpeed = 2f;
    public float sprintMaxSpeed = 4f;
    public Rigidbody rb;

    private float maxSpeed { get { return Input.GetButton("Sprint") ? sprintMaxSpeed : walkMaxSpeed; } }

    // Update is called once per frame
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
            rb.velocity *= friction;
        }
    }

    void FixedUpdate()
    {
        // Clamp the velocity to the max speed
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
    }
}
