using _Project.Scripts.KimicuUtilities.Coroutines;
using UnityEngine;

namespace _Project.Scripts.KimicuUtilities.Pool
{
    public class Despawner : MonoBehaviour
    {
        private KiCoroutine _routine;
        private bool _initialized = false;

        public void Init(float delay)
        {
            if (_initialized) return;

            _routine.StartRoutineDelay(() => gameObject.SetActive(false), delay);

            _initialized = true;
        }
    }
}