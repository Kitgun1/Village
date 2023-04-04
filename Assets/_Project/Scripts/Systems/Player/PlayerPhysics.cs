using System;
using UnityEngine;

namespace _Project.Scripts.Systems.Player
{
    public class PlayerPhysics : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public Rigidbody Rigidbody => _rigidbody;
        
        #region Unity Methods

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        #endregion
    }
}