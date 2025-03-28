using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    public float shakeMagnitude = 0.1f;
    public float shakeDuration = 0.2f;
    public float tiltAmount = 10f;

    public float minZoom = 5f;
    public float maxZoom = 8f;
    public float zoomSpeed = 15f;
    private Rigidbody2D carRb;

    private Camera cam;
    private float shakeTimer = 0f;
    private float currentTilt = 0f;
    private GameObject player;

    private void Start()
    {
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        carRb = player.GetComponent<Rigidbody2D>();
        target = player.transform;
    }

    private void LateUpdate()
    {
        // Smooth camera follow
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Camera shake effect
        if (shakeTimer > 0)
        {
            smoothedPosition += (Vector3)Random.insideUnitCircle * shakeMagnitude;
            shakeTimer -= Time.deltaTime;
        }

        transform.position = smoothedPosition;

        // Dynamic Zoom based on speed
        float speed = carRb.linearVelocity.magnitude;
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, speed / zoomSpeed);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * 5f);

        // Camera Tilt based on drift direction
        float driftDirection = Mathf.Clamp(carRb.angularVelocity, -1f, 1f);
        float targetTilt = driftDirection * tiltAmount;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * 5f);
        transform.rotation = Quaternion.Euler(0f, 0f, currentTilt);
    }

    public void TriggerShake(float intensity, float duration)
    {
        shakeMagnitude = intensity;
        shakeDuration = duration;
        shakeTimer = duration;
    }
}
