using System;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class BaseFlagPlacer : MonoBehaviour
{
    [SerializeField] private BaseFlag _flagPrefab;
    [SerializeField] private float _minimumFlagDistance = 10f;
    [SerializeField] private ParticleSystem _badPositionEffect;

    private BaseFlag _currentFlag;

    public bool HasFlag => _currentFlag != null;

    public Transform FlagPosition => _currentFlag.transform;

    public event Action FlagChanged;

    public void PlaceFlag(Vector3 flagPosition, Vector3 basePosition)
    {
        if ((flagPosition - basePosition).sqrMagnitude <= _minimumFlagDistance * 2)
        {
            ParticleSystem particleSystem = Instantiate(_badPositionEffect, flagPosition +Vector3.up, Quaternion.identity);
            particleSystem.Play();

            Destroy(particleSystem.gameObject, particleSystem.main.duration);

            return;
        }

        if (_currentFlag == null)
        {
            _currentFlag = Instantiate(_flagPrefab, flagPosition, Quaternion.identity);
        }
        else
        {
            _currentFlag.transform.position = flagPosition;
        }

        FlagChanged.Invoke();
    }

    public void RemoveFlag()
    {
        if (_currentFlag == null)
            return;

        Destroy(_currentFlag.gameObject);
        _currentFlag = null;
    }
}