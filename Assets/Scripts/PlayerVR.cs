using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class PlayerVR : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _failMessage;
    [SerializeField] private GameObject _parentMessage;
    private int _health = 3;

    private void OnEnable()
    {
        InteractableNPC.OnWrongDecision += Failed;
    }

    private void OnDisable()
    {
        InteractableNPC.OnWrongDecision -= Failed;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            Failed( "You died, level will be restarted!");
        }
    }

    private void Failed(string text)
    {
        
        _failMessage.text = text; 
        _parentMessage.gameObject.SetActive(true);

        Task.Delay(2000).ContinueWith(t =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }
}