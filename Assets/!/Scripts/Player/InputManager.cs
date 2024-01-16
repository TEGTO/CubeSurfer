using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour
{
    private TouchInput touchInput;

    public delegate void TouchingEvent(TouchControl touch);
    public event TouchingEvent OnTouchPerfoming;
    public event TouchingEvent OnTouchCanceled;

    private void Awake()
    {
        touchInput = new TouchInput();
    }
    private void OnEnable()
    {
        touchInput.Enable();
    }
    private void OnDisable()
    {
        touchInput.Disable();
    }
    private void Start()
    {
        touchInput.Touch.TouchPoint.performed += ctx => TouchingPerfoming(ctx);
        touchInput.Touch.TouchPress.canceled += ctx => TouchingCanceled(ctx);
    }
    private void TouchingPerfoming(InputAction.CallbackContext ctx)
    {
        OnTouchPerfoming?.Invoke(Touchscreen.current.primaryTouch);
    }
    private void TouchingCanceled(InputAction.CallbackContext ctx)
    {
        OnTouchCanceled?.Invoke(Touchscreen.current.primaryTouch);
    }
}
