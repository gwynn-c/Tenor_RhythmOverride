using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool attack;
		public bool dash;
		public bool crouch;
		public bool interact;
		public bool secondaryAttack;
		public bool specialAttack;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnAttack(InputValue value)
		{
			AttackInput(value.isPressed);
		}

		public void OnCrouch(InputValue value)
		{
			CrouchInput(value.isPressed);
		}

		public void OnDash(InputValue value)
		{
			DashInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
		{
			InteractValue(value.isPressed);
		}

		private void InteractValue(bool newInteract)
		{
			interact = newInteract;		
		}

		public void OnSecondaryAttack(InputValue value)
		{
			SecondaryAttack(value.isPressed);
		}

		public void OnSpecialAttack(InputValue value)
		{
			SpecialAttack(value.isPressed);
		}
#endif

		public void AttackInput(bool newAttackInput)
		{
			attack = newAttackInput;
		}

		public void DashInput(bool newDashInput)
		{
			dash = newDashInput;
		}
		public void CrouchInput(bool newCrouchInput)
		{
			crouch = newCrouchInput;
		}
		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void SecondaryAttack(bool newSecondaryAttack)
		{
			secondaryAttack = newSecondaryAttack;
		}

		public void SpecialAttack(bool newSpecialAttack)
		{
			specialAttack = newSpecialAttack;
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
	}
	
}