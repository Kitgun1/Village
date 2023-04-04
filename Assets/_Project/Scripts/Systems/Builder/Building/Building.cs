using System;
using _Project.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Systems.Builder.Building
{
    public class Building : MonoBehaviour
    {
        [BoxGroup("Characteristics:"), LabelText("Max Level"), SerializeField]
        protected string Name;

        [BoxGroup("Properties:"), LabelText("Size building"), SerializeField, Min(1)]
        private Vector2Int _sizeBuild = Vector2Int.one;

        [BoxGroup("Dependencies:"), LabelText("Mesh Renderer"), SerializeField]
        private Renderer _renderer;

        [BoxGroup("Other:"), LabelText("Color 1"), SerializeField]
        private Color _color1;

        [BoxGroup("Other:"), LabelText("Color 2"), SerializeField]
        private Color _color2;

        private float _maxTime;

        public Vector2Int Size => _sizeBuild;

        #region Unity Methods

        private void OnDrawGizmosSelected()
        {
            for (int x = 0; x < _sizeBuild.x; x++)
            {
                for (int y = 0; y < _sizeBuild.y; y++)
                {
                    if ((x + y) % 2 == 0) Gizmos.color = _color1;
                    else Gizmos.color = _color2;

                    Gizmos.DrawCube(transform.position + new Vector3(x, 0f, y), new Vector3(1f, 0.1f, 1f));
                }
            }
        }

        #endregion

        #region Methods

        public void SetState(ConstructionState state)
        {
            _renderer.material.color = state switch
            {
                ConstructionState.NotAvailable => new Color(1f, 0.3f, 0.2f, 1f),
                ConstructionState.Available => new Color(0.3f, 1f, 0.2f, 1f),
                ConstructionState.Default => new Color(1f, 1f, 1f, 1f),
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }

        #endregion
    }
}