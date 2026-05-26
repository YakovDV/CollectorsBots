using System;
using UnityEngine;

[RequireComponent(typeof(Base))]
public class BaseFlagPlacer : MonoBehaviour
{
    [SerializeField] private BaseFlag _flagPrefab;
    [SerializeField] private float _minimumFlagDistance = 10f;
    [SerializeField] private ParticleSystem _badPositionEffect;

    public bool HasFlag => CurrentFlag != null;

    public Transform FlagPosition => CurrentFlag.transform;

    public event Action FlagChanged;

    public BaseFlag CurrentFlag {  get; private set; }

    public bool IsBaseRequested { get; private set; }

    public void PlaceFlag(Vector3 flagPosition, Vector3 basePosition)
    {
        if ((flagPosition - basePosition).sqrMagnitude <= _minimumFlagDistance * _minimumFlagDistance)
        {
            ParticleSystem particleSystem = Instantiate(_badPositionEffect, flagPosition +Vector3.up, Quaternion.identity);
            particleSystem.Play();

            Destroy(particleSystem.gameObject, particleSystem.main.duration);

            return;
        }

        if (CurrentFlag == null)
        {
            CurrentFlag = Instantiate(_flagPrefab, flagPosition, Quaternion.identity);
            IsBaseRequested = true;
        }
        else
        {
            CurrentFlag.transform.position = flagPosition;
        }

        FlagChanged.Invoke();
    }

    public void RemoveFlag()
    {
        if (CurrentFlag == null)
            return;

        Destroy(CurrentFlag.gameObject);
        CurrentFlag = null;
        IsBaseRequested = false;
    }
}