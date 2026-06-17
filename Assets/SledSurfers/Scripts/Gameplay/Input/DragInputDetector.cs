using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using System;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class DragInputDetector : MonoBehaviour
{
    public event Action<Vector2> OnDragStarted;
    public event Action<Vector2> OnDragged;
    public event Action<Vector2> OnDragReleased;

    private bool _isEnabled;
    private bool _isDragging;
    private Vector2 _lastPosition;

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    public void Enable() => _isEnabled = true;

    public void Disable()
    {
        _isEnabled = false;
        if (_isDragging)
            EndDrag(_lastPosition);
    }

    private void Update()
    {
        if (!_isEnabled) return;

        if (Touch.activeTouches.Count > 0)
        {
            HandleTouch(Touch.activeTouches[0]);
        }
        else
        {
            HandleMouse();
        }
    }

    private void HandleMouse()
    {
        var mouse = Mouse.current;
        if (mouse == null) return;

        var position = mouse.position.ReadValue();

        if (mouse.leftButton.wasPressedThisFrame)
        {
            BeginDrag(position);
        }
        else if (mouse.leftButton.isPressed && _isDragging)
        {
            UpdateDrag(position);
        }
        else if (mouse.leftButton.wasReleasedThisFrame && _isDragging)
        {
            EndDrag(position);
        }
    }

    private void HandleTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case UnityEngine.InputSystem.TouchPhase.Began:
                BeginDrag(touch.screenPosition);
                break;
            case UnityEngine.InputSystem.TouchPhase.Moved:
            case UnityEngine.InputSystem.TouchPhase.Stationary:
                if (_isDragging) UpdateDrag(touch.screenPosition);
                break;
            case UnityEngine.InputSystem.TouchPhase.Ended:
            case UnityEngine.InputSystem.TouchPhase.Canceled:
                if (_isDragging) EndDrag(touch.screenPosition);
                break;
        }
    }

    private void BeginDrag(Vector2 position)
    {
        _isDragging = true;
        _lastPosition = position;
        OnDragStarted?.Invoke(position);
    }

    private void UpdateDrag(Vector2 position)
    {
        _lastPosition = position;
        OnDragged?.Invoke(position);
    }

    private void EndDrag(Vector2 position)
    {
        _isDragging = false;
        OnDragReleased?.Invoke(position);
    }
}