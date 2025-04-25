using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Animator))]
public class Hand : MonoBehaviour
{
    Animator animator;
    private float grabTarget;
    private float currentGrabValue;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimateHand();
    }

    public void SetGrab(float value)
    {
        grabTarget = value;
    }

    void AnimateHand()
    {
        if (grabTarget != currentGrabValue)
        {
            currentGrabValue = Mathf.MoveTowards(currentGrabValue, grabTarget, Time.deltaTime * 5f);
            animator.SetFloat("Grab", currentGrabValue);
        }
    }
}
