using System;
using UnityEngine;
using UnityEngine.UI;

public class InteractableNPC : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Canvas _interactionCanvas; // Assign your Canvas in inspector
    [SerializeField] private Button _safePlaceButton;
    [SerializeField] private Button _attackButton;

    [Header("NPC Settings")]
    [SerializeField] private bool _isAttacker = false; // Set true if this NPC is an attacker
    private BoxCollider _safeZone;

    public static event Action<bool> OnAllRequiredNpcsSaved;

    private int rescuedNpc = 0;

    public Canvas InteractableCanvas => _interactionCanvas;
    public static Action<string, bool> OnWrongDecision;

    public void Setup(BoxCollider safeZone)
    {
        _safeZone = safeZone;
    }

    private void Start()
    {
        _interactionCanvas.enabled = false; // Hide UI at start

        _safePlaceButton.onClick.AddListener(OnSafePlaceClicked);
        _attackButton.onClick.AddListener(OnAttackClicked);
    }

    private void OnDisable()
    {
        _safePlaceButton.onClick.RemoveListener(OnSafePlaceClicked);
        _attackButton.onClick.RemoveListener(OnAttackClicked);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure player has "Player" tag
        {
            _interactionCanvas.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _interactionCanvas.enabled = false;
        }
    }

    private void OnSafePlaceClicked()
    {
        if (_safeZone != null)
        {
            if (_isAttacker)
            {
                OnWrongDecision?.Invoke("Mission Failed - You helped the attacker! \n level will be restarted!", false);
            }
            else
            {
                Vector3 safeZonePosition = GetRandomPositionInSafeZone();
                transform.position = safeZonePosition;
                Debug.Log("NPCSaved");
                rescuedNpc++;
                if (rescuedNpc >= 5)
                {
                    OnAllRequiredNpcsSaved?.Invoke(true);
                    Debug.Log("NPC is moving to the safe place!");
                }
            }
            _interactionCanvas.enabled = false;
        }
    }

    private void OnAttackClicked()
    {
        if (!_isAttacker)
        {
            OnWrongDecision?.Invoke("Mission Failed - You attacked an innocent children! \n level will be restarted!", false);
            _interactionCanvas.enabled = false;
        }
        else
        {
            Debug.Log("You eliminated an attacker!");
            // Handle attacker defeat
        }
        _interactionCanvas.enabled = false;
    }


    private Vector3 GetRandomPositionInSafeZone()
    {
        Bounds bounds = _safeZone.bounds;
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            0.85f,
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
