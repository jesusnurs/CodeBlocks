using Unity.Netcode;
using UnityEngine;

public class OfficeWorker : NetworkBehaviour, IInteractable
{
    [SerializeField] private GameObject buttonIcon;

    public bool interactable;
    
    public void Interact()
    {
        if(!interactable)
            return;
        
        Debug.Log("Interact");
        interactable = false;
    }
    
    public void UnInteract()
    {
        Debug.Log("UnInteract");
    }

    public void SelectedVisual(bool isSelected)
    {
        buttonIcon.SetActive(isSelected);
    }
}
