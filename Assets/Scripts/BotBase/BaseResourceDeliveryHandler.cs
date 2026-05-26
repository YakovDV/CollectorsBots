using UnityEngine;

public class BaseResourceDeliveryHandler : MonoBehaviour
{
    [SerializeField] private BaseResourceCollector _collector;

    private ResourceDatabase _resourceDatabase;

    public void SetResourceDatabase(ResourceDatabase resourceDatabase)
    {
        _resourceDatabase = resourceDatabase;
    }

    public void HandleResourceDelivered(Resource resource)
    {
        if (resource == null)
            return;

        _collector.AddResource(resource);
        _resourceDatabase.ConsumeResource(resource);
    }
}