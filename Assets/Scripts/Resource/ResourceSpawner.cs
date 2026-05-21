using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [SerializeField] private ResourcePool _resourcePool;
    [SerializeField] private Transform[] _spawnZones;
    [SerializeField] private float _minDelay = 3f;
    [SerializeField] private float _maxDelay = 10f;

    private Coroutine _spawnDelayed;

    private void Start()
    {
        if (_spawnDelayed != null)
        {
            StopCoroutine(_spawnDelayed);
            _spawnDelayed = null;
        }

        _spawnDelayed = StartCoroutine(SpawnDelayed());
    }

    private IEnumerator SpawnDelayed()
    {
        while (true)
        {
            WaitForSeconds wait = new(Random.Range(_minDelay, _maxDelay));

            Vector3 spawnPoint = CalculateRandomSpawnPoint();

            Resource resource = _resourcePool.GetObject();
            resource.transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
            resource.ReadyToReturn += ReturnObject;

            yield return wait;
        }
    }

    private Vector3 CalculateRandomSpawnPoint()
    {
        Transform spawnZone = _spawnZones[Random.Range(0, _spawnZones.Length)];
        Vector3 point = new(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));

        return spawnZone.TransformPoint(point);
    }

    private void ReturnObject(Resource resource)
    {
        _resourcePool.ReleaseObject(resource);
        resource.ReadyToReturn -= ReturnObject;
    }
}