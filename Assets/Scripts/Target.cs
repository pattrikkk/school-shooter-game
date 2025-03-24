using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
     public Transform player;   // The player’s position to follow
    public float moveSpeed = 3f;  // The speed at which the AI moves


    public void Setup(Transform playerss)
    {
        player = playerss;
    }
    private void Update()
    {
        MoveTowardsPlayer();
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        // Calculate direction from AI to player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        // Move the AI towards the player
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Destroy(gameObject); // Destroy AI when hit by a bullet
        }
    }
}
