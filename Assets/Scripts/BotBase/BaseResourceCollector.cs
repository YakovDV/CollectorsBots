using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class BaseResourceCollector : MonoBehaviour
{
    private Collider _collectorZone;

    public event Action ResourceCollected;

    private void Awake()
    {
        _collectorZone = GetComponent<Collider>();
        _collectorZone.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            ResourceCollected?.Invoke();
            resource.RequestReturn();
        }
    }
}
