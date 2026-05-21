using UnityEngine;

public class Picker : MonoBehaviour
{
    private Vector3 _pickedObjectPosition = new(0f, 1f, 0f);

    public Resource PickedResource { get; private set; }

    public void PickUp(Resource resource)
    {
        if (PickedResource != null)
            return;

        PickedResource = resource;

        if (resource.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
            rigidbody.isKinematic = true;
        }

        resource.transform.SetParent(transform);
        resource.transform.localPosition = _pickedObjectPosition;
    }

    public void Drop()
    {
        PickedResource.transform.SetParent(null);

        if (PickedResource.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.isKinematic = false;
        }

        PickedResource = null;
    }
}