using System;
using _Project.Scripts.Enums;
using _Project.Scripts.KimicuUtilities.Coroutines;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Systems.Player
{
    [RequireComponent(typeof(PlayerPhysics))]
    public class PlayerRotate : MonoBehaviour
    {
        [BoxGroup("Properties:"), LabelText("Speed horizontal rotate"), SerializeField, Range(60F, 180F)]
        private float _horizontalSpeed = 60F;

        [BoxGroup("Properties:"), LabelText("Duration horizontal lerp"), SerializeField, Range(0.1F, 3F)]
        private float _horizontalDuration = 1F;

        [BoxGroup("Properties:"), LabelText("Default horizontal rotation"), SerializeField, Range(-180F, 180F)]
        private float _horizontalDefault = 0F;

        [PropertySpace(15F)]
        [BoxGroup("Properties:"), LabelText("Speed vertical rotate"), SerializeField, Range(60F, 180F)]
        private float _verticalSpeed = 60F;

        [BoxGroup("Properties:"), LabelText("Duration vertical lerp"), SerializeField, Range(0.1F, 3F)]
        private float _verticalDuration = 1F;

        [BoxGroup("Properties:"), LabelText("Default vertical rotation"), SerializeField, Range(50F, 80F)]
        private float _verticalDefault = 50F;

        [BoxGroup("Dependencies:"), LabelText("Cinemachine Virtual Camera"), SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        private PlayerPhysics _physics;
        private CinemachinePOV _pov;
        private CinemachineOrbitalTransposer _orbitalTransposer;

        private KiCoroutine _horizontalRoutine;
        private KiCoroutine _verticalRoutine;

        private void Awake()
        {
            _physics = GetComponent<PlayerPhysics>();

            _pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
            _orbitalTransposer = _virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

            _horizontalRoutine = new KiCoroutine();
            _verticalRoutine = new KiCoroutine();

            SetHorizontalRotation(1F);
            SetVerticalRotation(1F);
        }

        public void Rotate(int direction, AxisMode axisMode)
        {
            StopRoutine(axisMode);
            float angle;
            switch (axisMode)
            {
                case AxisMode.Horizontal:
                    angle = direction * _horizontalSpeed * Time.deltaTime;
                    _physics.Rigidbody.rotation =
                        Quaternion.Euler(new Vector3(0f, angle, 0f) + _physics.Rigidbody.rotation.eulerAngles);
                    _pov.m_HorizontalAxis.Value += angle;
                    _orbitalTransposer.m_XAxis.Value = _pov.m_HorizontalAxis.Value;
                    break;
                case AxisMode.Vertical:
                    angle = direction * _verticalSpeed * Time.deltaTime;
                    _pov.m_VerticalAxis.Value += angle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axisMode), axisMode, null);
            }
        }

        public void ResetRotation(AxisMode axisMode)
        {
            StartRoutine(axisMode);
        }

        private void SetHorizontalRotation(float time)
        {
            _pov.m_HorizontalAxis.Value = Mathf.Lerp(_pov.m_HorizontalAxis.Value, _horizontalDefault, time);
            _orbitalTransposer.m_XAxis.Value = Mathf.Lerp(_orbitalTransposer.m_XAxis.Value, _horizontalDefault, time);
            _physics.Rigidbody.rotation = Quaternion.Euler(Vector3.Lerp(_physics.Rigidbody.rotation.eulerAngles,
                new Vector3(0f, _horizontalDefault, 0f), time));
        }

        private void SetVerticalRotation(float time)
        {
            _pov.m_VerticalAxis.Value = Mathf.Lerp(_pov.m_VerticalAxis.Value, _verticalDefault, time);
        }

        private void StartRoutine(AxisMode axisMode)
        {
            switch (axisMode)
            {
                case AxisMode.Horizontal:
                {
                    _horizontalRoutine.StopRoutine();
                    _horizontalRoutine.StartRoutineLoop(
                        SetHorizontalRotation,
                        0F,
                        _horizontalDuration
                    );
                    break;
                }
                case AxisMode.Vertical:
                {
                    _verticalRoutine.StopRoutine();
                    _verticalRoutine.StartRoutineLoop(
                        SetVerticalRotation,
                        0F,
                        _verticalDuration
                    );
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(axisMode), axisMode, null);
            }
        }

        private void StopRoutine(AxisMode axisMode)
        {
            switch (axisMode)
            {
                case AxisMode.Horizontal:
                    _horizontalRoutine.StopRoutine();
                    break;
                case AxisMode.Vertical:
                    _verticalRoutine.StopRoutine();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axisMode), axisMode, null);
            }
        }
    }
}