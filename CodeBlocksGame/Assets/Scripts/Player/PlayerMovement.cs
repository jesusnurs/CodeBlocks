using UnityEngine;
using UnityEngine.InputSystem;
using Data;
using Unity.Netcode;
using UnityEngine.Serialization;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private PlayerMovementData playerMovementData;
        
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _dashAction;
        private InputAction _interactAction;
    
        private Rigidbody _rigidbody;
        private Vector2 _movementDirection;
        
        private float _moveSpeed;
        private float _dashForce;
        
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if(IsOwner)
            {
                _playerInput = GetComponent<PlayerInput>();
                _playerInput.ActivateInput();
                _rigidbody = GetComponent<Rigidbody>();

                _moveAction = _playerInput.actions.FindAction("Move");
                _moveAction.Enable();
                _moveSpeed = playerMovementData.MovementSpeed;
            }
        }
        
        private void FixedUpdate()
        {
            if(!IsOwner)
                return;
            
            HandleMovement();
        }

        private void HandleMovement()
        {
            _movementDirection = _moveAction.ReadValue<Vector2>();
            Debug.Log(_movementDirection + "   " + _moveSpeed);
            Debug.Log(_playerInput.inputIsActive);
            Debug.Log(_playerInput.devices.ToArray());
            _rigidbody.AddForce(new Vector3(_movementDirection.x, 0.0f, _movementDirection.y) * _moveSpeed,ForceMode.Force);
        }
    }
}
