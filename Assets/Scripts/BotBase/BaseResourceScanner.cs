using System;
using System.Collections;
using UnityEngine;

public class BaseResourceScanner : MonoBehaviour
{
    [SerializeField] private LayerMask _resourceLayerMask;
    [SerializeField] private float _sensorRadius = 100f;
    [SerializeField] private float _scanFrequency = 5f;
    [SerializeField] private ParticleSystem _effect;

    private readonly Collider[] _colliders = new Collider[64];

    private Coroutine _scaning;

    public event Action<Resource> ResourceFound;

    private void OnEnable()
    {
        _scaning = StartCoroutine(ScanFrequently());
    }

    private void OnDisable()
    {
        if (_scaning != null)
        {
            StopCoroutine(_scaning);
            _scaning = null;
        }
    }

    private IEnumerator ScanFrequently()
    {
        WaitForSeconds wait = new(_scanFrequency);

        while (enabled)
        {
            Scan();
            PlayEffect();

            yield return wait;
        }
    }

    private void Scan()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, _sensorRadius, _colliders, _resourceLayerMask, QueryTriggerInteraction.Ignore);

        for (int i = 0; i < count; i++)
        {
            Collider collider = _colliders[i];

            if (collider.TryGetComponent(out Resource resource))
            {
                ResourceFound?.Invoke(resource);
            }
        }
    }

    private void PlayEffect()
    {
        if (_effect != null)
            _effect.Play();
    }
}