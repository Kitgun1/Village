using _Project.Scripts.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _Project.Scripts.Systems.Builder
{
    public class Builder : MonoBehaviour
    {
        [BoxGroup("Dependencies"), LabelText("Building grid"), SerializeField]
        private BuildingGrid _buildingGrid;

        [BoxGroup("Dependencies"), LabelText("Particles confirmation"), SerializeField]
        private ParticleSystem _confirmParticle;

        private Buildings.Building _flyingBuilding;
        private Camera _mainCamera;
        private Input _playerInput;

        private bool _available;
        private Vector2Int _currentGridPosition;

        #region Unity Methods

        private void Awake()
        {
            _mainCamera = Camera.main;
            _playerInput = new Input();
            _playerInput.Builder.MoveCursor.performed += ctx => OnMoveBuilding();
            _playerInput.Builder.Accept.performed += ctx => OnEndPlacingBuilding();
        }

        private void OnEnable()
        {
            _playerInput.Enable();
        }

        private void OnDisable()
        {
            _playerInput.Disable();
        }

        #endregion

        #region Methods

        public void StartPlacingBuilding(Buildings.Building buildingPrefab)
        {
            if (_flyingBuilding != null) Destroy(_flyingBuilding.gameObject);

            _flyingBuilding = Instantiate(buildingPrefab);
            _flyingBuilding.SetState(ConstructionState.Available);
        }

        private void OnEndPlacingBuilding()
        {
            if (!_available) return;

            _buildingGrid.PlaceTake(_flyingBuilding, _currentGridPosition.x, _currentGridPosition.y);

            _flyingBuilding.SetState(ConstructionState.Default);
             //ParticleSystem confirmParticle = NightPool.Spawn(_confirmParticle, _flyingBuilding.transform, Quaternion.identity);
            
            _flyingBuilding = null;
            _available = false;
        }

        private void OnMoveBuilding()
        {
            if (_flyingBuilding == null) return;

            Vector3 mousePosition = _playerInput.Builder.MoveCursor.ReadValue<Vector2>();
            mousePosition.z = _mainCamera.farClipPlane;

            Plane groundPlane = new(Vector3.up, Vector3.zero);
            Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

            if (!groundPlane.Raycast(ray, out float position)) return;

            Vector3 worldPosition = ray.GetPoint(position);

            int x = Mathf.RoundToInt(worldPosition.x - _buildingGrid.Offset.x);
            int y = Mathf.RoundToInt(worldPosition.z - _buildingGrid.Offset.y);

            _currentGridPosition = new Vector2Int(x, y);

            _available = true;
            _flyingBuilding.SetState(ConstructionState.Available);

            if (x < 0 || x > _buildingGrid.Size.x - _flyingBuilding.Size.x ||
                y < 0 || y > _buildingGrid.Size.y - _flyingBuilding.Size.y ||
                _buildingGrid.IsPlaceTake(_flyingBuilding.Size, x, y))
            {
                _flyingBuilding.SetState(ConstructionState.NotAvailable);
                _available = false;
            }

            _flyingBuilding.transform.position =
                new Vector3(x + _buildingGrid.Offset.x, 0f, y + _buildingGrid.Offset.y);
        }

        #endregion
    }
}