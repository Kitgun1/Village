using System.Collections;
using UnityEngine;

namespace _Project.Scripts.KimicuUtilities.Coroutines
{
    public sealed class Coroutines : MonoBehaviour
    {
        private static Coroutines Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                GameObject gameObject = new GameObject("[COROUTINE MANAGER]");
                _instance = gameObject.AddComponent<Coroutines>();
                DontDestroyOnLoad(gameObject);
                return _instance;
            }
        }

        private static Coroutines _instance;

        public static Coroutine StartRoutineLoop(IEnumerator enumerator)
        {
            return Instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine)
        {
            if (routine == null) return;
            Instance.StopCoroutine(routine);
        }
    }
}
