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
    private Transform _safePlaceTarget;

    public Canvas InteractableCanvas => _interactionCanvas;
    public static Action<string> OnWrongDecision;

    public void Setup(Transform safePlace)
    {
        _safePlaceTarget = safePlace;
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
        if (_safePlaceTarget != null)
        {
            if (_isAttacker)
            {
                OnWrongDecision?.Invoke("Mission Failed - You helped attacker! \n level will be restarted!");
            }
            else
            {
                transform.position = _safePlaceTarget.position;
                Debug.Log("NPC is moving to the safe place!");
            }
        }
        _interactionCanvas.enabled = false;
    }

    private void OnAttackClicked()
    {
        if (!_isAttacker)
        {
            OnWrongDecision?.Invoke("Mission Failed - You attacked on innocent children! \n level will be restarted!");
            _interactionCanvas.enabled = false;
        }
        else
        {
            Debug.Log("You eliminated an attacker!");
            // Handle attacker defeat
        }
        _interactionCanvas.enabled = false;
    }
}
