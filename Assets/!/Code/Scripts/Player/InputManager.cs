using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class InputManager : MonoBehaviour
{
	private static TouchInput touchInput;

	public delegate void TouchingEvent(TouchControl touch);
	public event TouchingEvent OnTouchPerfoming;
	public event TouchingEvent OnTouchCanceled;

	private void OnEnable()
	{
		if (touchInput == null)
			touchInput = new TouchInput();
		touchInput.Touch.TouchPoint.performed += ctx => TouchingPerfoming(ctx);
		touchInput.Touch.TouchPress.canceled += ctx => TouchingCanceled(ctx);
		touchInput.Enable();
	}
	private void OnDisable()
	{
		touchInput.Touch.TouchPoint.performed -= ctx => TouchingPerfoming(ctx);
		touchInput.Touch.TouchPress.canceled -= ctx => TouchingCanceled(ctx);
		touchInput.Disable();
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
