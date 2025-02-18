using Unity.Netcode;

public class PlayerAnimator : NetworkBehaviour
{
    private void Update()
    {
        if(!IsOwner)
            return;
        
        //TODO Animations
    }
}
