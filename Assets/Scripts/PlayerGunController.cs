using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayerGunController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform holsterPoint; // The transform at the player's hip
    [SerializeField] private GameObject gunObject; // Reference to the existing gun in the scene

    [Header("Settings")]
    [SerializeField] private float returnToHolsterDelay = 2f; // Time before gun returns to holster after being dropped

    private XRGrabInteractable gunGrabInteractable;
    private Rigidbody gunRigidbody;
    private bool isGunHolstered = true;
    private Coroutine returnToHolsterCoroutine;

    void Start()
    {
        if (holsterPoint == null)
        {
            Debug.LogError("No holster point assigned to PlayerGunController!");
            return;
        }
        if (gunObject == null)
        {
            Debug.LogError("No gun object assigned to PlayerGunController!");
            return;
        }

        gunGrabInteractable = gunObject.GetComponent<XRGrabInteractable>();
        gunRigidbody = gunObject.GetComponent<Rigidbody>();

        if (gunGrabInteractable == null)
        {
            Debug.LogError("Gun doesn't have XRGrabInteractable component!");
            return;
        }

        gunGrabInteractable.selectEntered.AddListener(OnGunGrabbed);
        gunGrabInteractable.selectExited.AddListener(OnGunReleased);

        PlaceGunAtHolster();
    }

    private void OnDestroy()
    {
        if (gunGrabInteractable != null)
        {
            gunGrabInteractable.selectEntered.RemoveListener(OnGunGrabbed);
            gunGrabInteractable.selectExited.RemoveListener(OnGunReleased);
        }
    }

    public void OnGunGrabbed(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.gameObject == gunObject)
        {
            isGunHolstered = false;

            if (returnToHolsterCoroutine != null)
            {
                StopCoroutine(returnToHolsterCoroutine);
                returnToHolsterCoroutine = null;
            }
        }
    }

    public void OnGunReleased(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.gameObject == gunObject)
        {
            returnToHolsterCoroutine = StartCoroutine(ReturnGunToHolsterAfterDelay());
        }
    }

    private IEnumerator ReturnGunToHolsterAfterDelay()
    {
        yield return new WaitForSeconds(returnToHolsterDelay);

        if (!isGunHolstered && gunObject != null)
        {
            PlaceGunAtHolster();
        }

        returnToHolsterCoroutine = null;
    }

    private void PlaceGunAtHolster()
    {
        if (gunRigidbody != null)
        {
            gunRigidbody.isKinematic = true;
        }

        gunObject.transform.SetParent(holsterPoint);
        gunObject.transform.localPosition = Vector3.zero;
        gunObject.transform.rotation = Quaternion.Euler(90, 0, 0);

        isGunHolstered = true;
    }
}