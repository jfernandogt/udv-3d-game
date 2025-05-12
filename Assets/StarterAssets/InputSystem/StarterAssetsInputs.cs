using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		[SerializeField] private Vector2 move;
		[SerializeField] private Vector2 look;
		[SerializeField] private bool jump;
		[SerializeField] private bool sprint;

		[Header("Movement Settings")]
		[SerializeField] private bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputAction.CallbackContext value)
		{
			MoveInput(value.ReadValue<Vector2>());
		}

		public void OnLook(InputAction.CallbackContext value)
		{
			if (cursorInputForLook)
			{
				LookInput(value.ReadValue<Vector2>());
			}
		}

		public void OnJump(InputAction.CallbackContext value)
		{

			JumpInput(value.action.triggered);
		}

		public void OnSprint(InputAction.CallbackContext value)
		{
			SprintInput(value.action.ReadValue<float>() == 1);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}


		public Vector2 GetMove()
		{
			return move;
		}

		public Vector2 GetLook()
		{
			return look;
		}

		public bool IsJumping()
		{
			return jump;
		}

		public bool IsSprinting()
		{
			return sprint;
		}

		public bool IsAnalog()
		{
			return analogMovement;
		}

	}

}