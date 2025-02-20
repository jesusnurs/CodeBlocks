using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance;
    
    public event Action<InputDevice, string> OnPlayerJoined;
    public event Action<InputDevice, string> OnFirstPlayerJoined;
    
    private InputManagerActions _inputManager;
    private InputAction _addNewDeviceAction;
    private InputAction _divideKeyboardAction;
    
    private Dictionary<InputDevice, int> _deviceIndices;

    private readonly int _maxPlayersCount = 4;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else 
            Destroy(gameObject);
        
        _deviceIndices = new Dictionary<InputDevice, int>();
        
        _inputManager = new InputManagerActions();
        _inputManager.Enable();

        _addNewDeviceAction = _inputManager.Input.AddNewDevice;
        _addNewDeviceAction.performed += OnAddNewDevicePerformed;
        _addNewDeviceAction.Enable();
        
        _divideKeyboardAction = _inputManager.Input.DivideKeyboard;
        _divideKeyboardAction.performed += OnKeyboardDividePerformed;
        _divideKeyboardAction.Enable();
    }

    private void OnDestroy()
    {
        _addNewDeviceAction.performed -= OnAddNewDevicePerformed;
        _divideKeyboardAction.performed -= OnKeyboardDividePerformed;
    }

    private void OnAddNewDevicePerformed(InputAction.CallbackContext context)
    {
        InputDevice device = context.control.device;
        string controlScheme = GetControlScheme(device);
        
        if (!_deviceIndices.ContainsKey(device) & controlScheme != null & _deviceIndices.Count < _maxPlayersCount)
        {
            int index = _deviceIndices.Count;
            _deviceIndices.Add(device, index);
            OnPlayerJoined?.Invoke(device, controlScheme);
            if(index == 0)
                OnFirstPlayerJoined?.Invoke(device, controlScheme);
            Debug.Log($"Device {device.name} has entered index {index}");
        }
    }

    private void OnKeyboardDividePerformed(InputAction.CallbackContext context)
    {
        InputDevice device = context.control.device;

        if (_deviceIndices.ContainsKey(device))
        {
            //TODO Split Keyboard Controls
        }
    }

    private string GetControlScheme(InputDevice device)
    {
        switch (device)
        {
            case Keyboard:
                return "Keyboard";
            case Gamepad:
                return "Gamepad";
        }
        return null;
    }
}
