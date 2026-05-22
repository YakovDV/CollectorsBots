using System.Collections;
using System.Collections.Generic;
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
            TrySendBotsMultiple();

            yield return wait;
        }
    }

    private void TrySendBotsMultiple()
    {
        if (_spawner.TryGetFreeBotMultiple(out List<CollectorBot> bots) == false)
            return;

        foreach (CollectorBot bot in bots)
        {
            if (_resourceDatabase.TryRequestResource(out Resource resource) == false)
                continue;

            bot.SetTarget(resource.transform);
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