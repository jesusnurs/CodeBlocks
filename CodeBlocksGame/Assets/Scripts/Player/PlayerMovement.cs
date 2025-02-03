using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Data;
using Zenject;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Inject]
        private PlayerMovementData _playerMovementData;
    
        private Rigidbody _rigidbody;
        private PlayerInputSystem _playerInputSystem;
        private InputAction _moveAction;
    
        private float _moveSpeed;
        private float _dashForce;
        
        private Vector2 _movementDirection;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            
            _playerInputSystem = new PlayerInputSystem();
            _playerInputSystem.Player.Enable();
            
            _moveAction = _playerInputSystem.Player.Move;
        }

        private void Start()
        {
            _moveSpeed = _playerMovementData.MovementSpeed;
        }
        
        private void FixedUpdate()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            _movementDirection = _moveAction.ReadValue<Vector2>();
            _rigidbody.AddForce(new Vector3(_movementDirection.x, 0.0f, _movementDirection.y) * _moveSpeed,ForceMode.Force);
        }
    }
}
