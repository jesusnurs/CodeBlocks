using UnityEngine;
using UnityEngine.InputSystem;
using Data;
using Unity.Netcode;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : NetworkBehaviour
    {
        private PlayerMovementData _playerMovementData;
        private PlayerInputSystem _playerInputSystem;
        
        private InputAction _moveAction;
        private InputAction _dashAction;
        private InputAction _interactAction;
    
        private Rigidbody _rigidbody;
        private Vector2 _movementDirection;
        
        private float _moveSpeed;
        private float _dashForce;

        [Inject]
        public void Construct(PlayerMovementData playerMovementData, PlayerInputSystem playerInputSystem)
        {
            _playerMovementData = playerMovementData;
            _playerInputSystem = playerInputSystem;
        }

        private void Init()
        {
            // Construct
            
            _rigidbody = GetComponent<Rigidbody>();
            _moveAction = _playerInputSystem.MoveAction;
        }
        
        private void Start()
        {
            Init();
            
            _moveSpeed = _playerMovementData.MovementSpeed;
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
