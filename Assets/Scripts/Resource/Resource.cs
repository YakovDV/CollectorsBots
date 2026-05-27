using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private int _worth = 1;

    public event Action<Resource> Consumed;

    public int Worth => _worth;

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