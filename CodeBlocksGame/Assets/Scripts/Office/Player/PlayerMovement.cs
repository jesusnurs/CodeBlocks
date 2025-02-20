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
        
        private PlayerInputActions _playerInput;
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
                _rigidbody = GetComponent<Rigidbody>();
                
                _playerInput = new PlayerInputActions();
                _playerInput.Enable();

                _moveAction = _playerInput.FindAction("Move");
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
            _rigidbody.AddForce(new Vector3(_movementDirection.x, 0.0f, _movementDirection.y) * _moveSpeed,ForceMode.Force);
        }
    }
}
