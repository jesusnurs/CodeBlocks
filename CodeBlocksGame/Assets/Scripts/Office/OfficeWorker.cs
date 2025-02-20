using Unity.Netcode;
using UnityEngine;

public class OfficeWorker : NetworkBehaviour, IInteractable
{
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
}
