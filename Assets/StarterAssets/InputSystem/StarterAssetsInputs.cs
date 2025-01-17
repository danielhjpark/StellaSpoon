using System.Runtime.CompilerServices;
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
        public bool dodge;
        public bool aiming;

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
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            if(value.isPressed)
            {
                JumpInput(true);
            }
        }

        public void OnDodge(InputValue value)
        {
            // Dodge 입력을 한 프레임 동안만 처리하도록 상태를 true로 설정
            if (value.isPressed)
            {
                DodgeInput(true);
            }
        }

        public void OnAiming(InputValue value)
        {
            AimingInput(value.isPressed);
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

            if(newJumpState)
            {
                Invoke(nameof(ResetJumpInput), 0.1f); // 한 프레임 후 초기화 (0.1초는 수정 가능)
            }
        }

        public void DodgeInput(bool newDodgeState)
        {
            // Dodge 입력이 true일 때 한 프레임 후 자동으로 초기화
            dodge = newDodgeState;

            if (newDodgeState)
            {
                Invoke(nameof(ResetDodgeInput), 0.1f); // 한 프레임 후 초기화 (0.1초는 수정 가능)
            }
        }

        public void AimingInput(bool newAimingState)
        {
            aiming = newAimingState;
        }

        private void ResetDodgeInput()
        {
            dodge = false;
        }

        private void ResetJumpInput()
        {
            jump = false;
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
