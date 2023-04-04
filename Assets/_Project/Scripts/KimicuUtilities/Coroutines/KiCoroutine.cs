using System.Collections;
using KimicuUtilities;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.KimicuUtilities.Coroutines
{
    public class KiCoroutine
    {
        private Coroutine _routine;

        #region Routine Delay

        public void StartRoutineDelay(UnityAction call, float delay)
        {
            if (_routine != null) StopRoutine();

            _routine = Coroutines.StartRoutineLoop(RoutineDelay(call, delay));
        }

        public bool TryStartRoutineDelay(UnityAction call, float delay)
        {
            if (_routine != null) return false;

            _routine = Coroutines.StartRoutineLoop(RoutineDelay(call, delay));
            return true;
        }

        private IEnumerator RoutineDelay(UnityAction call, float delay)
        {
            yield return new WaitForSeconds(delay);
            call?.Invoke();
            StopRoutine();
        }

        #endregion

        #region Routine Loop

        public void StartRoutineLoop(UnityAction<float> call, float duration, float speed = 1F)
        {
            if (_routine != null) StopRoutine();

            _routine = Coroutines.StartRoutineLoop(RoutineLoop(call, duration, speed));
        }

        public bool TryStartRoutineLoop(UnityAction<float> call, float duration, float speed = 1F)
        {
            if (_routine != null) return false;

            _routine = Coroutines.StartRoutineLoop(RoutineLoop(call, duration, speed));
            return true;
        }

        private IEnumerator RoutineLoop(UnityAction<float> call, float duration, float speed = 1F)
        {
            float time = 0;
            float value = 0;
            while (true)
            {
                call?.Invoke(value);
                yield return new WaitForSeconds(Time.deltaTime * speed);
                time += Time.deltaTime * speed;
                value = (float)KiMath.CalculatePercentage(time, duration) / 100;
                Debug.Log(value);

                if (time > duration)
                    StopRoutine();
            }
        }

        #endregion

        public void StopRoutine()
        {
            Coroutines.StopRoutine(_routine);
            _routine = null;
        }
    }
}