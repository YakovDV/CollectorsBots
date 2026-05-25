using System;
using UnityEngine;

public class CollectorBot : MonoBehaviour
{
    [SerializeField] private BotToTargetMover _mover;
    [SerializeField] private Picker _picker;

    private Transform _base;

    public event Action<Resource> ResourceDelivered;
    public event Action<CollectorBot> BuildPointReached;

    public bool IsBusy { get; private set; }

    private void OnEnable()
    {
        _mover.TargetReached += OnTargetReached;
        IsBusy = false;
    }

    private void OnDisable()
    {
        _mover.TargetReached -= OnTargetReached;
        IsBusy = false;
    }

    public void SetTarget(Transform target)
    {
        if (IsBusy)
            return;

        if (target.TryGetComponent(out Resource resource))
        {
            _mover.SetTarget(resource.transform);
            IsBusy = true;
        }
    }

    public void SetBuildTarget(Transform target)
    {
        if (target.TryGetComponent(out BaseFlag baseFlag))
        {
            _mover.SetTarget(baseFlag.transform);
            IsBusy = true;
        }
    }

    public void SetBase(Transform @base)
    {
        _base = @base;
    }

    private void OnTargetReached(Transform transform)
    {
        if (transform.TryGetComponent(out Resource resource))
        {
            _picker.PickUp(resource);
            _mover.SetTarget(_base.transform);
        }
        else if (transform.TryGetComponent<BaseResourceCollector>(out _))
        {
            IsBusy = false;

            if (_picker.PickedResource != null)
            {
                ResourceDelivered?.Invoke(_picker.PickedResource);
                _picker.Drop();
            }
        }
        else if (transform.TryGetComponent<BaseFlag>(out _))
        {
            IsBusy = false;
            BuildPointReached?.Invoke(this);
        }
    }
}