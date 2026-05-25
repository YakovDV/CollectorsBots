using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceDatabase _resourceDatabase;
    [SerializeField] private BaseResourceScanner _scanner;
    [SerializeField] private BotSpawner _spawner;
    [SerializeField] private BaseResourceCollector _collector;
    [SerializeField] private BaseFlagPlacer _flagPlacer;
    [SerializeField] private int _minBotsToBuildBase = 2;
    [SerializeField] private BaseBuilder _builder;
    [SerializeField] private float _botsSendFrequency = 5f;
    [SerializeField] private bool _selfInitialize = false;
    [SerializeField] private int _initialBotCount = 3;

    private bool _isInitialized;
    private CollectorBot _builderBot;

    private Coroutine _sendingBots;

    public bool HasEnoughBotsToBuild => _spawner.CurrentBotsCount >= _minBotsToBuildBase;

    private void Start()
    {
        if (_selfInitialize)
        {
            Initialize(_resourceDatabase, _builder, _initialBotCount);
        }
    }

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        StartWork();
    }

    private void OnDisable()
    {
        StopWork();
    }

    private void OnDestroy()
    {
        if (_resourceDatabase != null)
            _resourceDatabase.UnregisterScanner(_scanner);
    }

    public void Initialize(ResourceDatabase resourceDatabase, BaseBuilder baseBuilder, int initialBotsCount)
    {
        if (_isInitialized)
            return;

        _resourceDatabase = resourceDatabase;
        _builder = baseBuilder;
        _isInitialized = true;

        _resourceDatabase.RegisterScanner(_scanner);

        StartWork();

        if (initialBotsCount > 0)
        {
            _spawner.SpawnInitialBots(initialBotsCount);
        }
    }

    public void RequestNewBot()
    {
        _spawner.SpawnBot();
    }

    public void AddBotToSpawner(CollectorBot bot)
    {
        _spawner.AddBot(bot);
    }

    public bool TrySendBuilderBot()
    {
        if (_flagPlacer.HasFlag == false)
            return false;

        if (_builderBot != null)
            return false;

        if (_spawner.TryGetFreeBot(out CollectorBot bot) == false)
            return false;

        _builderBot = bot;
        bot.SetBuildTarget(_flagPlacer.FlagPosition);

        return true;
    }

    private void StartWork()
    {
        _spawner.BotSpawned -= OnBotSpawned;
        _spawner.BotSpawned += OnBotSpawned;

        foreach (CollectorBot bot in _spawner.GetAllBots())
        {
            SubscribeBot(bot);
        }

        if (_sendingBots == null)
        {
            _sendingBots = StartCoroutine(SendBotsFrequently());
        }
    }

    private void StopWork()
    {
        _spawner.BotSpawned -= OnBotSpawned;

        foreach (CollectorBot bot in _spawner.GetAllBots())
        {
            UnsubscribeBot(bot);
        }

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
            SendBots();

            yield return wait;
        }
    }

    private void SendBots()
    {
        if (_spawner.TryGetFreeBotMultiple(out List<CollectorBot> bots) == false)
            return;

        foreach (CollectorBot bot in bots)
        {
            TrySendResourceBot(bot);
        }
    }

    private void TrySendResourceBot(CollectorBot bot)
    {
        if (bot == _builderBot)
        {
            return;
        }

        if (_resourceDatabase.TryRequestResource(out Resource resource) == false)
        {
            return;
        }

        bot.SetTarget(resource.transform);
    }

    private void OnBotSpawned(CollectorBot bot)
    {
        SubscribeBot(bot);
    }

    private void SubscribeBot(CollectorBot bot)
    {
        bot.ResourceDelivered -= OnResourceDelivered;
        bot.ResourceDelivered += OnResourceDelivered;

        bot.BuildPointReached -= OnBuildPointReached;
        bot.BuildPointReached += OnBuildPointReached;
    }

    private void UnsubscribeBot(CollectorBot bot)
    {
        bot.ResourceDelivered -= OnResourceDelivered;
        bot.BuildPointReached -= OnBuildPointReached;
    }

    private void OnResourceDelivered(Resource resource)
    {
        _collector.AddResource(resource);
        _resourceDatabase.ConsumeResource(resource);
    }

    private void OnBuildPointReached(CollectorBot bot)
    {
        if (bot != _builderBot)
            return;

        Vector3 buildPosition = _flagPlacer.FlagPosition.transform.position;

        _builderBot = null;

        _flagPlacer.RemoveFlag();

        UnsubscribeBot(bot);

        _spawner.RemoveBot(bot);

        Base newBase = _builder.CreateBase(buildPosition);
        newBase.AddBotToSpawner(bot);
    }
}