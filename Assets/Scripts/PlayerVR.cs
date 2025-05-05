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
        InteractableNPC.OnWrongDecision += ShowMessage;
    }

    private void OnDisable()
    {
        InteractableNPC.OnWrongDecision -= ShowMessage;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            ShowMessage( "You died, level will be restarted!");
        }
    }

    public async void ShowMessage(string text, bool hideOnly = false)
    {
        _failMessage.text = text;
        _parentMessage.SetActive(true);

        await Task.Delay(4000);

        if (hideOnly)
        {
            _parentMessage.SetActive(false);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}