using System;
using UnityEngine;

public class BaseResourceCollector : MonoBehaviour
{
    public event Action<Resource> ResourceCollected;

    public void AddResource(Resource resource)
    {
        if (resource == null)
        {
            return;
        }

        ResourceCollected?.Invoke(resource);
    }
}
