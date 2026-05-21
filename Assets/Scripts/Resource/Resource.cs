using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public bool IsPicked { get; private set; }

    public event Action<Resource> ReadyToReturn;

    private void OnEnable()
    {
        _particleSystem.Play();
        IsPicked = false;
    }

    private void OnDisable()
    {
        _particleSystem.Stop();
    }

    public void ChangePickedStatus(bool isPicked)
    {
        IsPicked = isPicked;
    }

    public void RequestReturn()
    {
        ReadyToReturn?.Invoke(this);
    }
}
