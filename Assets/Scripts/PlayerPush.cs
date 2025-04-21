using UnityEngine;

public class PlayerPush : MonoBehaviour
{
    public float pushForce = 1f;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // No Rigidbody, or the object is kinematic (not affected by physics) — skip
        if (body == null || body.isKinematic)
            return;

        // Don't push vertically
        if (hit.moveDirection.y < -0.3f)
            return;

        // Apply force
        Vector3 force = hit.moveDirection * pushForce;
        body.AddForce(force, ForceMode.VelocityChange);
    }
}