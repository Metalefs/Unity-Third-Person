using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Movement
{
    public class InputReader : MonoBehaviour, Controls.IPlayerActions
    {
        public Vector2 MovementValue { get; private set; }
        public Vector2 MouseValue { get; private set; }
        public event Action JumpEvent;
        public event Action DodgeEvent;
        public event Action LookEvent;
        public event Action TargetEvent;

        public bool IsAttacking { get; private set; }
        public bool IsBlocking { get; private set; }
        private Controls controls;

        private void Start()
        {
            controls = new Controls();
            controls.Player.SetCallbacks(this);
            controls.Player.Enable();
        }

        private void OnDestroy()
        {
            controls.Player.Disable();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            JumpEvent?.Invoke();
        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            DodgeEvent?.Invoke();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            MovementValue = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            LookEvent?.Invoke();
            MouseValue = context.ReadValue<Vector2>();
        }

        public void OnTarget(InputAction.CallbackContext context)
        {
            if (!context.performed) return;
            TargetEvent?.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsAttacking = true;
            }
            else if (context.canceled)
            {
                IsAttacking = false;
            }
        }
        public void OnBlock(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                IsBlocking = true;
            }
            else if (context.canceled)
            {
                IsBlocking = false;
            }
        }
    }
}