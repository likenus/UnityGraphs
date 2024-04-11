using UnityEngine.InputSystem;

public interface ITool
{
	void DoUpdate();
	void LeftClicked(InputAction.CallbackContext context);
	void RightClicked(InputAction.CallbackContext context);
	void LeftReleased(InputAction.CallbackContext context);
	void RightReleased(InputAction.CallbackContext context);
	void ShiftClicked(InputAction.CallbackContext context);
	void ShiftReleased(InputAction.CallbackContext context);
	void Reset();
}
