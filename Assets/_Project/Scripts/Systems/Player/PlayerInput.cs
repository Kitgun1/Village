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
            _movement = GetComponent<PlayerMovement>();
            _scaling = GetComponent<PlayerScaling>();
            _rotate = GetComponent<PlayerRotate>();
            _input = new Input();

            _input.Scaling.Zoom.performed += ctx => OnScalingKeyboard(1);
            _input.Scaling.Decrease.performed += ctx => OnScalingKeyboard(-1);
            _input.Rotation.ResetHorizontalRotation.performed += ctx => OnResetRotation(AxisMode.Horizontal);
            _input.Rotation.ResetVerticalRotation.performed += ctx => OnResetRotation(AxisMode.Vertical);
        }

        private void OnEnable()
        {
            _input.Enable();
        }

        private void OnDisable()
        {
            _input.Disable();
        }

        private void Update()
        {
            int scrollDirection = Mathf.RoundToInt(_input.Scaling.Scroll.ReadValue<float>());
            if (scrollDirection != 0)
            {
                _scaling.ChangeScale(scrollDirection);
            }
         
            Vector2 moveDirection = _input.Move.Movement.ReadValue<Vector2>();
            _movement.SetDirection(moveDirection);
            _movement.SetSpeed((int)_scaling.ScaleType);

            int turnHorizontalDirection = Mathf.RoundToInt(_input.Rotation.HorizontalTurn.ReadValue<float>());
            if (turnHorizontalDirection != 0)
            {
                _rotate.Rotate(turnHorizontalDirection, AxisMode.Horizontal);
            }
            
            int turnVerticalDirection = Mathf.RoundToInt(_input.Rotation.VerticalTurn.ReadValue<float>());
            if (turnVerticalDirection != 0)
            {
                _rotate.Rotate(turnVerticalDirection, AxisMode.Vertical);
            }
        }

        #endregion

        #region Methods

        private void OnScalingKeyboard(int value)
        {
            _scaling.ChangeScale(value);
        }

        private void OnResetRotation(AxisMode axisMode)
        {
            _rotate.ResetRotation(axisMode);
        }

        #endregion
    }
}