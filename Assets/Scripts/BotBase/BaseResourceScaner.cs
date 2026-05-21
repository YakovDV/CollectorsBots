using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class BaseResourceScaner : MonoBehaviour
{
    private const string ScanButton = "Scan";

    [SerializeField] private Collider _sensor;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _sensorMinSize = 3f;
    [SerializeField] private float _sensorMaxSize = 100f;
    [SerializeField] private float _scanSpeed = 3f;

    private bool _canScan = true;
    private int _scanCycle = 2;
    private Coroutine _scaning;

    public event Action<Resource> ResourceFound;

    private void Awake()
    {
        _sensor.isTrigger = true;
        _sensor.transform.localScale = new(_sensorMinSize, _sensorMinSize, _sensorMinSize);
    }

    private void Update()
    {
        if (Input.GetButtonDown(ScanButton) && _canScan)
        {
            if (_scaning != null)
            {
                StopCoroutine(_scaning);
                _scaning = null;
            }

            _scaning = StartCoroutine(Scan());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
        {
            ResourceFound?.Invoke(resource);
        }
    }

    private IEnumerator Scan()
    {
        WaitForSeconds wait = new(_scanSpeed * _scanCycle);

        _effect.Play();

        transform.DOScale(new Vector3(_sensorMaxSize, _sensorMinSize, _sensorMaxSize), _scanSpeed)
    .SetLoops(_scanCycle, LoopType.Yoyo);

        _canScan = false;

        yield return wait;

        _canScan = true;
    }
}