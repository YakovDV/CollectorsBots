using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;

    public event Action<Resource> Consumed;

    private void OnEnable()
    {
        _particleSystem.Play();
    }

    private void OnDisable()
    {
        _particleSystem.Stop();
    }

    public void Consume()
    {
        Consumed?.Invoke(this);
    }
}