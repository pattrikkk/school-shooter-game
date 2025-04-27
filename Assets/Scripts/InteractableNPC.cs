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
                Debug.Log("Mission Failed - You helped attacker!"); // TODO VYPISAT NA UI normalne ze konec a restart button

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
            Debug.Log("Mission Failed - You attacked an innocent student!");  // TODO VYPISAT NA UI normalne ze konec a restart button
            var enemyAi = GetComponent<EnemyAI>();
            enemyAi.Shoot();
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
