using System.Collections.Generic;
using UnityEngine;

public class ResourceDatabase : MonoBehaviour
{
    private readonly List<Resource> _foundResources = new();
    private readonly HashSet<Resource> _busyResources = new();

    public void AddFoundResource(Resource resource)
    {
        if (resource == null)
            return;

        if (_foundResources.Contains(resource))
            return;

        if (_busyResources.Contains(resource))
            return;

        _foundResources.Add(resource);
    }

    public void ConsumeResource(Resource resource)
    {
        if (resource == null)
        {
            return;
        }

        _busyResources.Remove(resource);
        _foundResources.Remove(resource);

        resource.Consume();
    }

    public bool TryRequestResource(out Resource resource)
    {
        resource = null;

        if (_foundResources.Count > 0)
        {
            resource = _foundResources[0];
            _foundResources.RemoveAt(0);
            _busyResources.Add(resource);

            return true;
        }

        return false;
    }
}
