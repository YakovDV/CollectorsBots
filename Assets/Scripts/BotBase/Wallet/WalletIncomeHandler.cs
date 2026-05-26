using UnityEngine;

public class WalletIncomeHandler : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private BaseResourceCollector _collector;

    private void OnEnable()
    {
        _collector.ResourceCollected += OnResourceCollected;
    }

    private void OnDisable()
    {
        _collector.ResourceCollected -= OnResourceCollected;
    }

    private void OnResourceCollected()
    {
        _wallet.AddValue(1);
    }
}