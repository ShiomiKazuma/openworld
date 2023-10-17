using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputKeyboard : MonoBehaviour
{
    [SerializeField] InputAction _inputAction;

    public Action OnStartEvent, OnCancelEvent;

    public Action<Vector2> OnPerformEvent;
    public bool IsInput { get; private set; }
    public Vector2 Vector { get; private set; }

    private void OnEnable() => _inputAction.Enable();
    private void OnDisable() => _inputAction.Disable();

    private void Awake()
    {
        _inputAction.started += OnStart;
        _inputAction.performed += OnPerform;
        _inputAction.canceled += OnCancel;
    }
    private void OnStart(InputAction.CallbackContext context)
    {
        IsInput = true;
        OnStartEvent?.Invoke();
    }
    private void OnPerform(InputAction.CallbackContext context)
    {
        Vector = _inputAction.ReadValue<Vector2>();
        OnPerformEvent?.Invoke(Vector);
    }
    private void OnCancel(InputAction.CallbackContext context)
    {
        IsInput = false;
        OnCancelEvent?.Invoke();
    }
}
