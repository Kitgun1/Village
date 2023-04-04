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

        [BoxGroup("Properties:"), LabelText("Default horizontal rotation"), SerializeField, Range(-180F, 180F)]
        private float _horizontalDefault;

        [BoxGroup("Properties:"), LabelText("Duration horizontal lerp"), SerializeField, Range(0.1F, 3F)]
        private float _horizontalDuration = 1F;

        [PropertySpace(15F)]
        [BoxGroup("Properties:"), LabelText("Speed vertical rotate"), SerializeField, Range(60F, 180F)]
        private float _verticalSpeed = 60F;

        [BoxGroup("Properties:"), LabelText("Default vertical rotation"), SerializeField, Range(50F, 80F)]
        private float _verticalDefault = 50F;

        [BoxGroup("Properties:"), LabelText("Duration vertical lerp"), SerializeField, Range(0.1F, 3F)]
        private float _verticalDuration = 1F;

        [BoxGroup("Dependencies:"), LabelText("Cinemachine Virtual Camera"), SerializeField]
        private CinemachineVirtualCamera _virtualCamera;

        private PlayerPhysics _physics;
        private CinemachinePOV _pov;
        private CinemachineOrbitalTransposer _orbitalTransposer;

        private KiCoroutine _horizontalRoutine;
        private KiCoroutine _verticalRoutine;

        private Quaternion _currentHorizontalQuaternion;
        private float _currentHorizontalValue;
        private float _currentVerticalValue;

        private void Awake()
        {
            _physics = GetComponent<PlayerPhysics>();

            _pov = _virtualCamera.GetCinemachineComponent<CinemachinePOV>();
            _orbitalTransposer = _virtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();

            _horizontalRoutine = new KiCoroutine();
            _verticalRoutine = new KiCoroutine();

            SetDefault(AxisMode.Horizontal);
            SetDefault(AxisMode.Vertical);
        }

        public void Rotate(AxisMode axisMode, int direction)
        {
            switch (axisMode)
            {
                case AxisMode.Horizontal:
                    _horizontalRoutine.StopRoutine();
                    _orbitalTransposer.m_XAxis.Value += direction * _horizontalSpeed * Time.deltaTime;
                    _pov.m_HorizontalAxis.Value += direction * _horizontalSpeed * Time.deltaTime;
                    Vector3 targetAngle = new Vector3(0F, direction * _horizontalSpeed * Time.deltaTime, 0F);
                    Quaternion target = Quaternion.Euler(targetAngle + _physics.Rigidbody.rotation.eulerAngles);
                    _physics.Rigidbody.rotation = target;
                    break;
                case AxisMode.Vertical:
                    _verticalRoutine.StopRoutine();
                    _pov.m_VerticalAxis.Value += direction * _verticalSpeed * Time.deltaTime;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axisMode), axisMode, null);
            }
        }

        public void SetDefault(AxisMode axisMode, float duration = -1F)
        {
            switch (axisMode)
            {
                case AxisMode.Horizontal:
                    _currentHorizontalValue = _orbitalTransposer.m_XAxis.Value;
                    _currentHorizontalQuaternion = _physics.Rigidbody.rotation;
                    _horizontalRoutine.StopRoutine();
                    if (duration <= 0F)
                    {
                        _horizontalRoutine.StartRoutineLoop(
                            t => LerpRotationToDefault(axisMode, t),
                            _horizontalDuration
                        );
                    }
                    else
                    {
                        _horizontalRoutine.StartRoutineLoop(
                            t => LerpRotationToDefault(axisMode, t),
                            duration
                        );
                    }

                    break;
                case AxisMode.Vertical:
                    _currentVerticalValue = _pov.m_VerticalAxis.Value;
                    _verticalRoutine.StopRoutine();
                    if (duration <= 0F)
                    {
                        _verticalRoutine.StartRoutineLoop(
                            t => LerpRotationToDefault(axisMode, t),
                            _verticalDuration
                        );
                    }
                    else
                    {
                        _verticalRoutine.StartRoutineLoop(
                            t => LerpRotationToDefault(axisMode, t),
                            duration
                        );
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axisMode), axisMode, null);
            }
        }

        private void LerpRotationToDefault(AxisMode axisMode, float t)
        {
            switch (axisMode)
            {
                case AxisMode.Horizontal:
                    _orbitalTransposer.m_XAxis.Value = Mathf.Lerp(_currentHorizontalValue, _horizontalDefault, t);
                    _pov.m_HorizontalAxis.Value = Mathf.Lerp(_currentHorizontalValue, _horizontalDefault, t);
                    Quaternion targetRotation = Quaternion.Euler(new Vector3(0F, _horizontalDefault, 0F));
                    _physics.Rigidbody.rotation = Quaternion.Lerp(_currentHorizontalQuaternion, targetRotation, t);
                    break;
                case AxisMode.Vertical:
                    _pov.m_VerticalAxis.Value = Mathf.Lerp(_currentVerticalValue, _verticalDefault, t);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(axisMode), axisMode, null);
            }
        }
    }
}