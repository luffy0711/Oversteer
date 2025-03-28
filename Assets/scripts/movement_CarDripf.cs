using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class movement_CarDripf : MonoBehaviour
{
    public float acceleration = 5f;
    public float maxSpeed = 10f;
    public float driftFactor = 0.95f;
    public float turnFactor = 3.5f;
    public float straightenFactor = 2f;
    public float skid_needS;
    public float skid_needI;

    public TrailRenderer tr;
    public TrailRenderer tr_;

    private CameraShake cameraShake;
    private Rigidbody2D rb;
    private float rotationAngle = 0f;
    private float steeringInput = 0f;

    private bool isDrifting;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    void Update()
    {
        float lateralVelocity = Vector2.Dot(rb.linearVelocity, transform.right);
        //Debug.Log("shaking :)" + Mathf.Abs(lateralVelocity));

        steeringInput = 0f;
        if (Input.GetKey(KeyCode.A))
            steeringInput = -1f;
        else if (Input.GetKey(KeyCode.D))
            steeringInput = 1f;

        skitting();

       

  

      
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.up * acceleration);

        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        float speedFactor = rb.linearVelocity.magnitude / maxSpeed;

        if (steeringInput != 0)
        {
            rotationAngle -= steeringInput * turnFactor * speedFactor;
        }
        else
        {
            // Auto-straighten when no input
            rotationAngle = Mathf.LerpAngle(rotationAngle, 0f, straightenFactor * Time.fixedDeltaTime);
        }

        rb.MoveRotation(rotationAngle);

        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.linearVelocity, transform.up);
        Vector2 lateralVelocity = transform.right * Vector2.Dot(rb.linearVelocity, transform.right);
        rb.linearVelocity = forwardVelocity + lateralVelocity * driftFactor;

        float driftAmount = lateralVelocity.magnitude / maxSpeed; // Normalize drift
        if (driftAmount > 0.7f )  // Only shake if drifting
        {
            cameraShake.ShakeCamera();
        }
    }

    void skitting()
    {
        float lateralVelocity = Vector2.Dot(rb.linearVelocity, transform.right);
        isDrifting = Mathf.Abs(lateralVelocity) > skid_needS && Mathf.Abs(steeringInput) > skid_needI;
        tr.emitting = isDrifting;
        tr_.emitting = isDrifting;

    }
}
