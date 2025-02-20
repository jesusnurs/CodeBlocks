using System;
using UnityEngine;

public class SelectedInteractableVisual : MonoBehaviour
{
    [SerializeField] private GameObject buttonIcon;
    
    private IInteractable _interactable;

    private void Awake()
    {
        _interactable = GetComponent<IInteractable>();
    }

    private void Start()
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedInteractableChanged += OnSelectedVisualChanged;
        }
        else
        {
            Player.OnAnyPlayerSpawned += OnAnyPlayerSpawned;
        }
    }
    
    private void OnSelectedVisualChanged(IInteractable selected)
    {
        buttonIcon.SetActive(selected == _interactable);
    }

    private void OnAnyPlayerSpawned()
    {
        if (Player.LocalInstance != null)
        {
            Player.LocalInstance.OnSelectedInteractableChanged -= OnSelectedVisualChanged;
            Player.LocalInstance.OnSelectedInteractableChanged += OnSelectedVisualChanged;
        }
    }
}
