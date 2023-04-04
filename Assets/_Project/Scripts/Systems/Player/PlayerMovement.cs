using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using _Project.Scripts.Interfaces;

namespace _Project.Scripts.Systems.Player
{
    [RequireComponent(typeof(PlayerPhysics))]
    public class PlayerMovement : MonoBehaviour, IMovable
    {
        [BoxGroup("Properties:"), LabelText("Speeds movement"), SerializeField]
        private List<float> _speeds = new();

        private PlayerPhysics _physics;

        private Vector2 _direction;
        private int _currentSpeedIndex;

        public void SetDirection(Vector2 direction) => _direction = direction;
        public void SetSpeed(int index) => _currentSpeedIndex = index;

        private void Awake()
        {
            _physics = GetComponent<PlayerPhysics>();
        }

        private void FixedUpdate()
        {
            Move(_direction);
        }

        public void Move(Vector2 direction)
        {
            Vector3 moveDirection = Vector3.zero;
            if (direction.x != 0)
            {
                moveDirection += transform.right * direction.x;
            }

            if (direction.y != 0)
            {
                moveDirection += transform.forward * direction.y;
            }

            Vector3 targetVelocity = moveDirection * (_speeds[_currentSpeedIndex] * Time.deltaTime * 100);
            targetVelocity.y = _physics.Rigidbody.velocity.y;
            _physics.Rigidbody.velocity = targetVelocity;
        }
    }
}