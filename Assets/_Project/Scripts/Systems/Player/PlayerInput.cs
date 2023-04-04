using _Project.Scripts.Enums;
using UnityEngine;

namespace _Project.Scripts.Systems.Player
{
    [RequireComponent(
        typeof(PlayerMovement), 
        typeof(PlayerScaling), 
        typeof(PlayerRotate))]
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

            _input.Scaling.Zoom.performed += ctx => _scaling.ChangeScale(1);
            _input.Scaling.Decrease.performed += ctx => _scaling.ChangeScale(-1);
            _input.Rotation.ResetHorizontalRotation.performed += ctx => _rotate.SetDefault(AxisMode.Horizontal);
            _input.Rotation.ResetVerticalRotation.performed += ctx => _rotate.SetDefault(AxisMode.Vertical);
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
            // scaling
            int scrollDirection = Mathf.RoundToInt(_input.Scaling.Scroll.ReadValue<float>());
            if (scrollDirection != 0)
            {
                _scaling.ChangeScale(scrollDirection);
            }

            // movement
            Vector2 moveDirection = _input.Move.Movement.ReadValue<Vector2>();
            _movement.SetDirection(moveDirection);
            _movement.SetSpeed((int)_scaling.ScaleType);

            Vector2Int rotateDirection = Vector2Int.RoundToInt(
                new Vector2(
                    _input.Rotation.HorizontalTurn.ReadValue<float>(),
                    _input.Rotation.VerticalTurn.ReadValue<float>()
                ).normalized);

            //  horizontal rotate
            if (rotateDirection.x != 0)
            {
                _rotate.Rotate(AxisMode.Horizontal, rotateDirection.x);
            }

            // vertical rotate
            if (rotateDirection.y != 0)
            {
                _rotate.Rotate(AxisMode.Vertical, rotateDirection.y);
            }
        }

        #endregion
    }
}