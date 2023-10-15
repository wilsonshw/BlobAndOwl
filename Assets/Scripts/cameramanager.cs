using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameramanager : MonoBehaviour
{
    public Transform player1;
    public Transform player2;

    public float minSize = 5f; // Minimum camera size
    public float maxSize = 10f; // Maximum camera size
    public float zoomSpeed = 2f; // Zoom speed
    public float lerpSpeed = 5f; // Smoothness when camera adjusts

    public Camera cam;

    private void Start()
    {

    }

    private void FixedUpdate()
    {
        HandleCamera();
    }

    private void HandleCamera()
    {
        if (player1 != null && player2 != null)
        {
            Vector3 midpoint = (player1.position + player2.position) / 2f;

            float distance = Vector3.Distance(player1.position, player2.position);

            // Calculate the desired camera size based on the distance between players
            float targetSize = Mathf.Clamp(distance, minSize, maxSize);

            // Smoothly adjust the camera size and position
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * zoomSpeed);
            transform.position = Vector3.Lerp(transform.position, midpoint, Time.deltaTime * lerpSpeed);
        }
    }

}
