using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShooterDescriptionDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private CharacterManager _characterManager;

    void Start()
    {
        if (_characterManager != null)
        {
            _characterManager.OnShooterSpawned += SetDescription;
        }
    }

    private void OnDestroy()
    {
        if (_characterManager != null)
        {
            _characterManager.OnShooterSpawned -= SetDescription;
        }
    }

    public void SetDescription(string description)
    {
        if (_descriptionText != null)
        {
            _descriptionText.text = description;
        }
    }
}
