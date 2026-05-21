using TMPro;
using UnityEngine;

public class WalletView : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private string _walletName;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;

    private void Awake()
    {
        _textMeshProUGUI.text = _walletName + " " + 0;
    }

    private void OnEnable()
    {
        _wallet.ValueChanged += UpdateValue;
    }

    private void OnDisable()
    {
        _wallet.ValueChanged -= UpdateValue;
    }

    private void UpdateValue(int value)
    {
        _textMeshProUGUI.text = _walletName + " " + value.ToString();
    }
}