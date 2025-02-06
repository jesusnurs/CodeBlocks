using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem
{
    private PlayerInputActions _playerInputActions;

    public InputAction MoveAction;
    public InputAction DashAction;
    public InputAction InteractAction;
    
    private PlayerInputSystem()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Enable();

        MoveAction = _playerInputActions.Player.Move;
        //DashAction = _playerInputActions.Player.Dash;
        //InteractAction = _playerInputActions.Player.Interact;
    }
}
