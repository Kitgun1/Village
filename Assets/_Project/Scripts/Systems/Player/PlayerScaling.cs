using System;
using System.Collections;
using _Project.Scripts.Enums;
using _Project.Scripts.Interfaces;
using _Project.Scripts.KimicuUtilities.Coroutines;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Systems.Player
{
    public class PlayerScaling : MonoBehaviour, IScalable
    {
        [BoxGroup("Properties:"), LabelText("Duration lerp"), SerializeField, Range(0.1F, 1.5F)]
        private float _duration = 1f;

        [BoxGroup("Properties:"), LabelText("Default zoom"), SerializeField]
        private ScaleType _defaultZoom = ScaleType.Normal;

        [BoxGroup("Dependencies:"), LabelText("Cinemachine Virtual Camera"), SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        private CinemachineTransposer _transposer;
        private KiCoroutine _lerpOffsetRoutine;

        public ScaleType ScaleType { get; private set; }

        private void Awake()
        {
            _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

            _lerpOffsetRoutine = new KiCoroutine();
            
            ScaleType = _defaultZoom;
            SetScale(ScaleType);
        }

        public void ChangeScale(int value = 0)
        {
            value = Mathf.Clamp(value, -1, 1);

            if ((int)ScaleType + value <= (int)ScaleType.SuperZoom &&
                (int)ScaleType + value >= (int)ScaleType.SuperDecrease && value != 0)
            {
                ScaleType += value;
            }

            SetScale(ScaleType);
        }

        private void SetScale(ScaleType scaleType)
        {
            switch (scaleType)
            {
                case ScaleType.SuperDecrease:
                    StartRoutine(new Vector3(0F, 15F, -3F));
                    break;
                case ScaleType.Decrease:
                    StartRoutine(new Vector3(0F, 12F, -2.66F));
                    break;
                case ScaleType.Normal:
                    StartRoutine(new Vector3(0F, 9F, -2.36F));
                    break;
                case ScaleType.Zoom:
                    StartRoutine(new Vector3(0F, 6F, -1.66F));
                    break;
                case ScaleType.SuperZoom:
                    StartRoutine(new Vector3(0F, 3F, -1.33F));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void StartRoutine(Vector3 target)
        {
            _lerpOffsetRoutine.StopRoutine();

            _lerpOffsetRoutine.StartRoutineLoop(
                time => _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset, target, time),
                0F,
                _duration
            );
        }

        private void StopRoutine()
        {
            _lerpOffsetRoutine.StopRoutine();
        }
    }
}