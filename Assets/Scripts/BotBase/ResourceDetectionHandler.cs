using UnityEngine;

public class ResourceDetectionHandler : MonoBehaviour
{
    [SerializeField] private BaseResourceScanner _scanner;

    private ResourceDatabase _resourceDatabase;
    private bool _isInitialized;

    public void SetResourceDatabase(ResourceDatabase resourceDatabase)
    {
        _resourceDatabase = resourceDatabase;
        _isInitialized = true;

        SubscribeScanner();
    }

    private void OnEnable()
    {
        if (_isInitialized)
            SubscribeScanner();
    }

    private void OnDisable()
    {
        UnsubscribeScanner();
    }

    private void SubscribeScanner()
    {
        _scanner.ResourceFound -= OnResourceFound;
        _scanner.ResourceFound += OnResourceFound;
    }

    private void UnsubscribeScanner()
    {
        _scanner.ResourceFound -= OnResourceFound;
    }

    private void OnResourceFound(Resource resource)
    {
        _resourceDatabase.AddFoundResource(resource);
    }
}