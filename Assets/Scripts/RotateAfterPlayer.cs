using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAfterPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //get this object and rotate it to the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0; // Ignore vertical difference
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion rotation = Quaternion.LookRotation(-direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
            }
        }
        else
        {
            Debug.LogWarning("Player not found");
        }
    }
}
