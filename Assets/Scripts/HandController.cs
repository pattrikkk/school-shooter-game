using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    ActionBasedController controller;
    public Hand hand;
    public XRRayInteractor rayInteractor; // Assign in inspector

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Check if the ray interactor is holding a gun
        if (rayInteractor != null && rayInteractor.selectTarget != null)
        {
            hand.SetGrab(1f);
            return;
        }
        // If not holding a gun, release grab
        hand.SetGrab(0f);
    }
}
