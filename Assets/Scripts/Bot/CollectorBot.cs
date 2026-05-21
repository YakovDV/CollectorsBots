using UnityEngine;

public class CollectorBot : MonoBehaviour
{
    [SerializeField] private BotToTargetMover _mover;
    [SerializeField] private Picker _picker;

    private BaseResourceCollector _resourceCollector;

    public bool IsBusy { get; private set; }

    private void OnEnable()
    {
        _mover.TargetReached += OnTargetReached;
    }

    private void OnDisable()
    {
        _mover.TargetReached -= OnTargetReached;
    }

    public void SetTarget(Transform target)
    {
        if (IsBusy)
        {
            return;
        }

        if (target.TryGetComponent(out Resource resource))
        {
            _mover.SetTarget(resource.transform);
            IsBusy = true;
        }
    }

    public void SetResourceCollector(BaseResourceCollector resourceCollector)
    {
        _resourceCollector = resourceCollector;
    }

    private void OnTargetReached(Transform transform)
    {
        if (transform.TryGetComponent<BaseResourceCollector>(out _))
        {
            IsBusy = false;

            if (_picker.PickedResource != null)
            {
                _picker.Drop();
            }

            return;
        }
        else if (transform.TryGetComponent(out Resource resource))
        {
            _picker.PickUp(resource);
            _mover.SetTarget(_resourceCollector.transform);
        }
    }
}