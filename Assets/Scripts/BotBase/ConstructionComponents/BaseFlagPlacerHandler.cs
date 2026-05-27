using UnityEngine;

public class BaseFlagPlacerHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _baseLayerMask;
    [SerializeField] private LayerMask _mapLayerMask;
    [SerializeField] private float _rayDistance = 500f;

    private BaseFlagPlacer _selectedBase;

    private void Awake()
    {
        if (_camera == null)
            _camera = Camera.main;
    }

    private void OnEnable()
    {
        _input.SelectClicked += OnSelectClicked;
    }

    private void OnDisable()
    {
        _input.SelectClicked -= OnSelectClicked;
    }

    private void OnSelectClicked(Vector2 screenPosition)
    {
        Ray ray = _camera.ScreenPointToRay(screenPosition);

        if (_selectedBase != null)
        {
            TryPlaceFlag(ray);
        }

        TrySelectBase(ray);
    }

    private bool TrySelectBase(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _baseLayerMask))
        {
            if (hit.collider.TryGetComponent(out BaseFlagPlacer baseFlagPlacer))
            {
                _selectedBase = baseFlagPlacer;
                return true;
            }
        }

        return false;
    }

    private void TryPlaceFlag(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance, _mapLayerMask))
        {
            _selectedBase.PlaceFlag(hit.point);
            _selectedBase = null;
        }
    }
}