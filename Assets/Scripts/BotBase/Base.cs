using System.Collections;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceDatabase _resourceDatabase;
    [SerializeField] private BotSpawner _spawner;
    [SerializeField] private BaseResourceCollector _collector;
    [SerializeField] private float _botsSendFrequency = 5f;

    private Coroutine _sendingBots;

    private void OnEnable()
    {
        _spawner.BotSpawned += OnBotSpawned;

        _sendingBots = StartCoroutine(SendBotsFrequently());
    }

    private void OnDisable()
    {
        _spawner.BotSpawned -= OnBotSpawned;

        if (_sendingBots != null)
        {
            StopCoroutine(_sendingBots);
            _sendingBots = null;
        }
    }

    private IEnumerator SendBotsFrequently()
    {
        WaitForSeconds wait = new(_botsSendFrequency);

        while (enabled)
        {
            TrySendBot();

            yield return wait;
        }
    }

    private void TrySendBot()
    {
        if (_spawner.TryGetFreeBot(out CollectorBot bot) == false)
            return;

        if (_resourceDatabase.TryRequestResource(out Resource resource) == false)
            return;

        bot.SetTarget(resource.transform);
    }

    private void OnBotSpawned(CollectorBot bot)
    {
        bot.ResourceDelivered -= OnResourceDelivered;
        bot.ResourceDelivered += OnResourceDelivered;
    }

    private void OnResourceDelivered(Resource resource)
    {
        _collector.AddResource(resource);
        _resourceDatabase.ConsumeResource(resource);
    }
}