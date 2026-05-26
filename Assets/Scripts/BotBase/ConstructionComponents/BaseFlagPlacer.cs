using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Base))]
public class BaseFlagPlacer : MonoBehaviour
{
    [SerializeField] private BaseFlag _flagPrefab;
    [SerializeField] private float _minimumFlagDistance = 30f;
    [SerializeField] private LayerMask _baseLayerMask;
    [SerializeField] private ParticleSystem _badPositionEffect;

    private readonly Collider[] _colliders = new Collider[16];

    public bool HasFlag => CurrentFlag != null;

    public Transform FlagPosition => CurrentFlag.transform;

    public event Action<FlagChangeType> FlagChanged;

    public BaseFlag CurrentFlag { get; private set; }

    public bool IsBaseRequested { get; private set; }

    public void PlaceFlag(Vector3 flagPosition)
    {
        if (IsAbleToPlace(flagPosition) == false)
            return;

        if (CurrentFlag == null)
        {
            CurrentFlag = Instantiate(_flagPrefab, flagPosition, Quaternion.identity);
            FlagChanged?.Invoke(FlagChangeType.Created);
        }
        else
        {
            CurrentFlag.transform.position = flagPosition;
            FlagChanged?.Invoke(FlagChangeType.Moved);
        }

    }

    public void RemoveFlag()
    {
        if (CurrentFlag == null)
            return;

        Destroy(CurrentFlag.gameObject);
        CurrentFlag = null;

        FlagChanged?.Invoke(FlagChangeType.Removed);
    }

    private bool IsAbleToPlace(Vector3 position)
    {
        int count = Physics.OverlapSphereNonAlloc(position, _minimumFlagDistance, _colliders, _baseLayerMask);

        for (int i = 0; i < count; i++)
        {
            Collider collider = _colliders[i];

            if (collider.TryGetComponent<Base>(out _))
            {
                PlayBadPositionEffect(position);
                return false;
            }
        }

        return true;
    }

    private void PlayBadPositionEffect(Vector3 position)
    {
        ParticleSystem particleSystem = Instantiate(_badPositionEffect, position + Vector3.up, Quaternion.identity);
        particleSystem.Play();

        Destroy(particleSystem.gameObject, particleSystem.main.duration);
    }
}
