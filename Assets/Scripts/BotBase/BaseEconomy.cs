using UnityEngine;

public class BaseEconomy : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private Base _base;
    [SerializeField] private BaseFlagPlacer _flagPlacer;
    [SerializeField] private int _botCost = 3;
    [SerializeField] private int _baseCost = 5;

    private bool _isProcessing;

    private void OnEnable()
    {
        _wallet.ValueChanged += OnWalletValueChanged;
        _flagPlacer.FlagChanged += OnFlagChanged;
    }

    private void OnDisable()
    {
        _wallet.ValueChanged -= OnWalletValueChanged;
        _flagPlacer.FlagChanged -= OnFlagChanged;
    }

    private void OnWalletValueChanged(int value)
    {
        TrySpendResources();
    }

    private void OnFlagChanged()
    {
        TrySpendResources();
    }

    private void TrySpendResources()
    {
        if (_isProcessing)
            return;

        _isProcessing = true;

        if (_flagPlacer.HasFlag && _base.HasEnoughBotsToBuild)
        {
            TryBuildNewBase();
        }
        else
        {
            TryBuyBot();
        }

        _isProcessing = false;
    }

    private void TryBuildNewBase()
    {
        if (_wallet.CanSpend(_baseCost) == false)
            return;

        if (_base.TrySendBuilderBot() == false)
            return;

        _wallet.TrySpend(_baseCost);
    }

    private void TryBuyBot()
    {
        if (_wallet.TrySpend(_botCost))
        {
            _base.RequestNewBot();
        }
    }
}