using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Data;
using Zenject;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Inject]
        private PlayerMovementData _playerMovementData;
        [Inject]
        private PlayerInputSystem _playerInputSystem;
        
        private InputAction _moveAction;
        private InputAction _dashAction;
        private InputAction _interactAction;
    
        private Rigidbody _rigidbody;
        private Vector2 _movementDirection;
        
        private float _moveSpeed;
        private float _dashForce;
        

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _moveAction = _playerInputSystem.MoveAction;
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
