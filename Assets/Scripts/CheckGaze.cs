using System;
using UnityEngine;

public class CheckGaze : MonoBehaviour
{
    [Tooltip("The transform representing the VR camera's forward direction.")]
    public Transform raycastOrigin;

    [Tooltip("The maximum distance for the raycast.")]
    public float maxRaycastDistance = 100f;

    [Tooltip("The layer mask to filter which objects the raycast can hit.")]
    public LayerMask targetLayerMask;

    [Tooltip("The time in seconds a target needs to be looked at before the action is triggered.")]
    public float gazeTimeThreshold = 5f;

    [Tooltip("Event triggered when a target is gazed at for the required time.")]
    public static Action OnGazeAction;

    private GameObject currentlyLookedAt;
    private float currentGazeTime = 0f;

    void Update()
    {
        // Ensure we have a valid raycast origin
        if (raycastOrigin == null)
        {
            Debug.LogError("Raycast Origin Transform is not assigned!");
            enabled = false; // Disable the script to prevent further errors
            return;
        }

        // Perform the raycast
        if (Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out RaycastHit hitInfo, maxRaycastDistance, targetLayerMask))
        {
            // Check if we hit a new object
            if (hitInfo.collider.gameObject != currentlyLookedAt)
            {
                currentlyLookedAt = hitInfo.collider.gameObject;
                currentGazeTime = 0f; // Reset the gaze timer
                Debug.Log($"Now looking at: {currentlyLookedAt.name}");
            }
            else
            {
                // We are still looking at the same object, so increment the timer
                currentGazeTime += Time.deltaTime;

                // Check if the gaze time threshold has been reached
                if (currentGazeTime >= gazeTimeThreshold)
                {
                    // Trigger the action event
                    OnGazeAction?.Invoke();
                    Debug.Log($"Gazed at {currentlyLookedAt.name} for {gazeTimeThreshold} seconds. Action triggered!");

                    // Optionally, you might want to reset the currentlyLookedAt object
                    // or the timer depending on your desired behavior.
                    currentlyLookedAt = null;
                    currentGazeTime = 0f;
                }
            }
        }
        else
        {
            // If we are not hitting anything, reset the currently looked at object and timer
            currentlyLookedAt = null;
            currentGazeTime = 0f;
        }
    }
}
