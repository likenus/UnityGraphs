using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
	public Camera mainCamera;
	private CameraActions inputActions;
	
	private void Awake()
	{
		inputActions = new CameraActions();
	}
	
	#region - Enable / Disable -
	private void OnEnable()
	{
		inputActions.Player.Zoom.performed += ChangeZoom;
		inputActions.Enable();
	}

	private void ChangeZoom(InputAction.CallbackContext context)
	{
		float mouseScrollY = context.ReadValue<float>();
		if (mouseScrollY > 0) // Scrolled up
		{
			mainCamera.orthographicSize -= mainCamera.orthographicSize < 0.5f ? 0f : 0.1f;
		}
		if (mouseScrollY < 0) // Scrolled down
		{
			mainCamera.orthographicSize += mainCamera.orthographicSize > 10f ? 0f : 0.1f;
		}
		
	}

	private void OnDisable()
	{
		inputActions.Disable();
	}
	#endregion
}
