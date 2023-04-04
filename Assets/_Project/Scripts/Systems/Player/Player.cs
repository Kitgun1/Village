using UnityEngine;

namespace _Project.Scripts.Systems.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        private PlayerCharacteristics _characteristics;

        private void Awake()
        {
            _characteristics = new PlayerCharacteristics();
        }
    }
}