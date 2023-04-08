using System;
using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.Scripts.Systems.Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerScaling), typeof(PlayerRotate))]
    public class PlayerInput : MonoBehaviour
    {
        private Input _input;
        private PlayerMovement _movement;
        private PlayerScaling _scaling;
        private PlayerRotate _rotate;

        #region Unity Methods

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Confined;

            _movement = GetComponent<PlayerMovement>();
            _scaling = GetComponent<PlayerScaling>();
            _rotate = GetComponent<PlayerRotate>();
            _input = new Input();
        }

        private void Start()
        {
            InputInitStarted();
            InputInitPerformed();
            InputInitCanceled();
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        #endregion

        #region Methods

        private void InputInitStarted()
        {
            // Camera Rotation
            _input.CameraRotation.ResetXAxisRotation.started += ctx => { _rotate.SetDefault(AxisMode.AxisY); };
            _input.CameraRotation.ResetYAxisRotation.started += ctx => { _rotate.SetDefault(AxisMode.AxisX); };
            _input.CameraRotation.Reset2DRotation.started += ctx => { _rotate.SetDefault(AxisMode.Axis2D); };

            // Other
            _input.Other.CameraInspectionMode.started += ctx =>
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            };
        }

        private void InputInitPerformed()
        {
            // Scaling
            _input.Scaling.Zoom.performed += ctx => { _scaling.ChangeScale(1); };
            _input.Scaling.Decrease.performed += ctx => { _scaling.ChangeScale(-1); };
            _input.Scaling.ScrollZoom.performed += ctx =>
            {
                int scrollDirection = Mathf.RoundToInt(ctx.ReadValue<float>());
                _scaling.ChangeScale(scrollDirection);
            };

            // Camera Rotation
            _input.CameraRotation.Rotation2D.performed += ctx =>
            {
                _rotate.SetDirection(ctx.ReadValue<Vector2>());
            };

            // Movement
            _input.Move.Movement.performed += ctx =>
            {
                Vector2 moveDirection = ctx.ReadValue<Vector2>();
                _movement.SetDirection(moveDirection);
                _movement.SetSpeed((int)_scaling.ScaleType);
            };
        }

        private void InputInitCanceled()
        {
            // Camera Rotation
            _input.CameraRotation.Rotation2D.canceled += ctx =>
            {
                _rotate.SetDirection(ctx.ReadValue<Vector2>());
            };

            // Movement
            _input.Move.Movement.canceled += ctx =>
            {
                _movement.SetDirection(Vector2.zero);
                _movement.SetSpeed((int)_scaling.ScaleType);
            };

            // Other
            _input.Other.CameraInspectionMode.canceled += ctx =>
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            };
        }

        #endregion
    }
}