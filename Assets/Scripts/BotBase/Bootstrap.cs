using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private BaseSpawner _baseSpawner;
    [SerializeField] private Transform _initialBasePoint;
    [SerializeField] private int _initialBotCount = 3;

    private void Start()
    {
        _baseSpawner.CreateBase(_initialBasePoint.position,_initialBotCount);
    }
}