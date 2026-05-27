using System;
using UnityEngine;

public class CollectorBot : MonoBehaviour
{
    [SerializeField] private BotToTargetMover _mover;
    [SerializeField] private Picker _picker;

    private Transform _basePosition;

    public event Action<Resource> ResourceDelivered;
    public event Action<CollectorBot> BuildPointReached;
    public event Action<CollectorBot, bool> StatusChanged;

    public bool IsBusy { get; private set; }

    private void Awake()
    {
        IsBusy = false;
    }

    private void OnDisable()
    {
        _mover.TargetReached -= OnResourceReached;
        _mover.TargetReached -= OnBaseReached;
        _mover.TargetReached -= OnBuildPositionReached;

        IsBusy = false;
    }

    public void SetTargetResource(Resource resource)
    {
        if (IsBusy)
            return;

        if (resource == null)
            return;

        _mover.SetTarget(resource.transform);
        _mover.TargetReached += OnResourceReached;

        IsBusy = true;
        StatusChanged?.Invoke(this, IsBusy);
    }

    public void SetBuildTarget(BaseFlag flag)
    {
        if (IsBusy)
            return;

        if (flag == null)
            return;

        _mover.SetTarget(flag.transform);
        _mover.TargetReached += OnBuildPositionReached;

        IsBusy = true;
        StatusChanged?.Invoke(this, IsBusy);
    }

    public void SetBasePosition(Transform basePosition)
    {
        _basePosition = basePosition;
    }

    private void OnResourceReached(Transform transform)
    {
        if (transform.TryGetComponent(out Resource resource) == false)
            return;

        _mover.TargetReached -= OnResourceReached;
        _mover.TargetReached += OnBaseReached;

        _picker.PickUp(resource);

        _mover.SetTarget(_basePosition);
    }

    private void OnBaseReached(Transform transform)
    {
        if (transform != _basePosition)
            return;

        IsBusy = false;
        StatusChanged?.Invoke(this, IsBusy);

        if (_picker.PickedResource != null)
        {
            ResourceDelivered?.Invoke(_picker.PickedResource);
            _picker.Drop();
        }

        _mover.TargetReached -= OnBaseReached;
    }

    private void OnBuildPositionReached(Transform transform)
    {
        if (transform.TryGetComponent<BaseFlag>(out _) == false)
            return;

        IsBusy = false;

        StatusChanged?.Invoke(this, IsBusy);
        BuildPointReached?.Invoke(this);

        _mover.TargetReached -= OnBuildPositionReached;
    }
}