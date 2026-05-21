using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BotShop : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private Button _button;
    [SerializeField] private int _botCost = 3;
    [SerializeField] private float _textTime = 2;
    [SerializeField] private string _deniedText = "Not enough resources.";
    [SerializeField] private string _successText = "Succsessfully.";

    private Coroutine _textCoroutine;

    public event Action OnBotRequested;

    private void Awake()
    {
        _text.alpha = 0f;
        _cost.text = _botCost.ToString();
    }

    private void OnEnable()
    {
        _button?.onClick.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        _button?.onClick.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        if (_wallet.TrySpend(_botCost) == false)
        {

            if (_textCoroutine != null)
            {
                StopCoroutine(_textCoroutine);
                _textCoroutine = null;
            }

            _textCoroutine = StartCoroutine(TextMessege(_deniedText));

            return;
        }

        if (_textCoroutine != null)
        {
            StopCoroutine(_textCoroutine);
            _textCoroutine = null;
        }

        _textCoroutine = StartCoroutine(TextMessege(_successText));

        OnBotRequested?.Invoke();

        _botCost++;
        _cost.text = _botCost.ToString();
    }

    private IEnumerator TextMessege(string text)
    {
        WaitForSeconds wait = new(_textTime);

        _text.alpha = 1;
        _text.text = text;

        yield return wait;

        _text.alpha = 0;
    }
}