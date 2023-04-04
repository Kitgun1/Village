using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Systems.Builder
{
    public class BuildingGrid : MonoBehaviour
    {
        [BoxGroup("Properties:"), LabelText("Size map"), SerializeField, Min(1)]
        private Vector2Int _gridSize = Vector2Int.one * 10;

        [BoxGroup("Properties:"), LabelText("Map offset"), SerializeField]
        private Vector2 _gridOffset = Vector2.zero;

        [BoxGroup("Other:"), LabelText("Color 1"), SerializeField]
        private Color _color1;

        [BoxGroup("Other:"), LabelText("Color 2"), SerializeField]
        private Color _color2;

        private Building.Building[,] _grid;

        public Vector2Int Size => _gridSize;
        public Vector2 Offset => _gridOffset;

        #region Unity Methods

        private void Awake()
        {
            _grid = new Building.Building[_gridSize.x, _gridSize.y];
        }

        private void OnDrawGizmosSelected()
        {
            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    if ((x + y) % 2 == 0) Gizmos.color = _color1;
                    else Gizmos.color = _color2;

                    Gizmos.DrawCube(transform.position + new Vector3(x + _gridOffset.x, 0f, y + _gridOffset.y),
                        new Vector3(1f, 0.1f, 1f));
                }
            }
        }

        #endregion

        #region Methods

        public void PlaceTake(Building.Building building, int placeX, int placeY)
        {
            for (int x = 0; x < building.Size.x; x++)
            {
                for (int y = 0; y < building.Size.y; y++)
                {
                    _grid[placeX + x, placeY + y] = building;
                }
            }
        }

        public Building.Building GetBuilding(Vector2Int boxSize, int placeX, int placeY)
        {
            for (int x = 0; x < boxSize.x; x++)
            {
                for (int y = 0; y < boxSize.y; y++)
                {
                    if (_grid[placeX + x, placeY + y] != null) return _grid[placeX + x, placeY + y];
                }
            }

            return null;
        }

        public bool IsPlaceTake(Vector2Int buildingSize, int placeX, int placeY)
        {
            for (int x = 0; x < buildingSize.x; x++)
            {
                for (int y = 0; y < buildingSize.y; y++)
                {
                    if (_grid[placeX + x, placeY + y] != null) return true;
                }
            }

            return false;
        }

        #endregion
    }
}