using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;
    [SerializeField] private int _initialBotsCount = 0;
    [SerializeField] private ResourceDatabase _resourceDatabase;
    [SerializeField] private LayerMask _terrainLayerMask;
    [SerializeField] private float _rayLenght = 5f;
    [SerializeField] private float _baseYOffset = 2f;

    public Base CreateBase(Vector3 position)
    {
        Vector3 buildPoint = CalculateBuildPosition(position);

        Base newBase = Instantiate(_basePrefab, buildPoint, Quaternion.identity);
        newBase.Initialize(_resourceDatabase, this, _initialBotsCount);

        return newBase;
    }

    private Vector3 CalculateBuildPosition(Vector3 position)
    {
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, _rayLenght, _terrainLayerMask))
        {
            return hit.point + Vector3.up * _baseYOffset;
        }

        return position + Vector3.up * _baseYOffset;
    }
}