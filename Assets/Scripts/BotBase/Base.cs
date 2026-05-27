using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BaseResourceCollector _collector;
    [SerializeField] private BaseResourceScanner _scanner;
    [SerializeField] private ResourceDetectionHandler _resourceDetectionHandler;

    [SerializeField] private BaseBotsDatabase _botsDatabase;

    [SerializeField] private BaseResourceBotDispatcher _resourceDispatcher;
    [SerializeField] private BaseConstruction _baseConstruction;

    [SerializeField] private BaseSpawner _baseSpawner;

    private ResourceDatabase _resourceDatabase;
    private bool _isInitialized;

    public bool HasEnoughBotsToBuild => _baseConstruction.HasEnoughBotsToBuild;

    public void Initialize(ResourceDatabase resourceDatabase, BaseSpawner baseSpawner, int initialBotsCount)
    {
        if (_isInitialized)
            return;

        _resourceDatabase = resourceDatabase;
        _baseSpawner = baseSpawner;
        _isInitialized = true;

        _resourceDispatcher.SetResourceDatabase(_resourceDatabase);
        _resourceDetectionHandler.SetResourceDatabase(resourceDatabase);
        _baseConstruction.SetBaseBuilder(_baseSpawner);

        StartWork();

        if (initialBotsCount > 0)
        {
            _botsDatabase.SpawnInitialBots(initialBotsCount);
        }
    }

    public void RequestNewBot()
    {
        _botsDatabase.CreateNewBot();
    }

    public void AddBotToBase(CollectorBot bot)
    {
        _botsDatabase.AddBot(bot);
    }

    public bool TrySendBuilderBot()
    {
        return _baseConstruction.TrySendBuilderBot();
    }

    private void StartWork()
    {
        _botsDatabase.BotAdded += OnBotAdded;

        foreach (CollectorBot bot in _botsDatabase.GetAllBots())
        {
            SubscribeBot(bot);
        }

        _resourceDispatcher.StartWork();
    }

    private void OnBotAdded(CollectorBot bot)
    {
        SubscribeBot(bot);
    }

    private void SubscribeBot(CollectorBot bot)
    {
        bot.ResourceDelivered += OnResourceDelivered;
        bot.BuildPointReached += OnBuildPointReached;
    }

    private void UnsubscribeBot(CollectorBot bot)
    {
        bot.ResourceDelivered -= OnResourceDelivered;
        bot.BuildPointReached -= OnBuildPointReached;
    }

    private void OnResourceDelivered(Resource resource)
    {
        HandleResourceDelivered(resource);
    }

    private void OnBuildPointReached(CollectorBot bot)
    {
        UnsubscribeBot(bot);
        _baseConstruction.HandleBuildPointReached(bot);
    }

    private void HandleResourceDelivered(Resource resource)
    {
        if (resource == null)
            return;

        _collector.AddResource(resource);
        _resourceDatabase.ConsumeResource(resource);
    }
}