using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Data;
using Unity.Netcode;


[RequireComponent(typeof(Rigidbody))]
public class Player : NetworkBehaviour
{
    public static Player LocalInstance;

    [SerializeField] private PlayerMovementData playerMovementData;

    public event Action<IInteractable> OnSelectedInteractableChanged;
    public static event Action OnAnyPlayerSpawned;

    private PlayerInputActions _playerInput;
    private InputAction _moveAction;
    private InputAction _dashAction;
    private InputAction _interactAction;

    private Rigidbody _rigidbody;
    private Vector2 _movementDirection;

    private float _moveSpeed;
    private float _dashForce;

    private IInteractable _selectedInteractable;

    private IInteractable SelectedInteractable
    {
        get => _selectedInteractable;
        set
        {
            _selectedInteractable = value;
            OnSelectedInteractableChanged?.Invoke(_selectedInteractable);
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {
            LocalInstance = this;

            _rigidbody = GetComponent<Rigidbody>();

            _playerInput = new PlayerInputActions();
            _playerInput.Enable();

            _moveAction = _playerInput.FindAction("Move");
            _moveAction.Enable();

            _interactAction = _playerInput.FindAction("Interact");
            _interactAction.performed += Interact;

            _moveSpeed = playerMovementData.MovementSpeed;
        }

        OnAnyPlayerSpawned?.Invoke();
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
            return;

        HandleMovement();
    }

    private void HandleMovement()
    {
        _movementDirection = _moveAction.ReadValue<Vector2>();
        _rigidbody.AddForce(new Vector3(_movementDirection.x, 0.0f, _movementDirection.y) * _moveSpeed,
            ForceMode.Force);
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if (SelectedInteractable != null)
        {
            SelectedInteractable.Interact();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SelectedInteractable != null)
            return;

        if (other.TryGetComponent(out IInteractable interactable))
        {
            SelectedInteractable = interactable;
            //SelectedInteractable.SelectedVisual(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (SelectedInteractable == null)
            return;

        if (other.TryGetComponent(out IInteractable interactable))
        {
            if (SelectedInteractable == interactable)
            {
                //SelectedInteractable.SelectedVisual(false);
                SelectedInteractable = null;
            }
        }
    }
}