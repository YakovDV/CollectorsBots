using System;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private BaseResourceCollector _resourceCollector;

    public event Action<int> ValueChanged; 

    public int Value {  get; private set; }

    private void Awake()
    {
        Value = 0;
    }

    public bool CanSpend(int amount)
    {
        return amount >= 0 && Value >= amount;
    }

    public bool TrySpend(int amount)
    {
        if (amount < 0)
            return false;

        if (Value < amount)
            return false;

        Value -= amount;
        ValueChanged?.Invoke(Value);

        return true;
    }

    public void AddValue(int amount)
    {
        if (amount <= 0)
            return;

        Value += amount;
        ValueChanged?.Invoke(Value);
    }
}