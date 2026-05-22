using System.Collections.Generic;
using UnityEngine;

public class ResourceDatabase : MonoBehaviour
{
    [SerializeField] private BaseResourceScanner[] _resourceScanners;

    private readonly List<Resource> _foundResources = new();
    private readonly HashSet<Resource> _busyResources = new();

    private void OnEnable()
    {
        foreach (BaseResourceScanner scanner in _resourceScanners)
        {
            scanner.ResourceFound += OnResourceFound;
        }
    }

    private void OnDisable()
    {
        foreach (BaseResourceScanner scanner in _resourceScanners)
        {
            scanner.ResourceFound -= OnResourceFound;
        }
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

    private void OnResourceFound(Resource resource)
    {
        if (_foundResources.Contains(resource) || _busyResources.Contains(resource))
        {
            return;
        }

        _foundResources.Add(resource);
    }
}