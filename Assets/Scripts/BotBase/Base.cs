using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceDatabase _resourceDatabase;
    [SerializeField] private ResourceDetectionHandler _resourceDetectionHandler;
    [SerializeField] private BaseResourceScanner _scanner;

    [SerializeField] private BaseBotsDatabase _botsDatabase;
    [SerializeField] private BaseBotsController _botsController;

    [SerializeField] private BaseResourceDeliveryHandler _resourceDeliveryHandler;
    [SerializeField] private BaseResourceBotDispatcher _resourceDispatcher;
    [SerializeField] private BaseConstructionController _constructionController;

    [SerializeField] private BaseBuilder _builder;
    [SerializeField] private bool _selfInitialize = false;
    [SerializeField] private int _initialBotCount = 3;

    private bool _isInitialized;

    public bool HasEnoughBotsToBuild => _constructionController.HasEnoughBotsToBuild;

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

    public void Initialize(ResourceDatabase resourceDatabase, BaseBuilder baseBuilder, int initialBotsCount)
    {
        if (_isInitialized)
            return;

        _resourceDatabase = resourceDatabase;
        _builder = baseBuilder;
        _isInitialized = true;

        _resourceDetectionHandler.SetResourceDatabase(_resourceDatabase);
        _resourceDeliveryHandler.SetResourceDatabase(_resourceDatabase);
        _resourceDispatcher.SetResourceDatabase(_resourceDatabase);
        _constructionController.SetBaseBuilder(_builder);

        StartWork();

        if (initialBotsCount > 0)
        {
            _botsController.SpawnInitialBots(initialBotsCount);
        }
    }

    public void RequestNewBot()
    {
        _botsController.RequestNewBot();
    }

    public void AddBotToBase(CollectorBot bot)
    {
        _botsController.AddBot(bot);
    }

    public bool TrySendBuilderBot()
    {
        return _constructionController.TrySendBuilderBot();
    }

    private void StartWork()
    {
        _botsDatabase.BotAdded -= OnBotAdded;
        _botsDatabase.BotAdded += OnBotAdded;

        foreach (CollectorBot bot in _botsDatabase.GetAllBots())
        {
            SubscribeBot(bot);
        }

        _resourceDispatcher.StartWork();
    }

    private void StopWork()
    {
        _botsDatabase.BotAdded -= OnBotAdded;

        foreach (CollectorBot bot in _botsDatabase.GetAllBots())
        {
            UnsubscribeBot(bot);
        }

        _resourceDispatcher.StopWork();
    }

    private void OnBotAdded(CollectorBot bot)
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
        _resourceDeliveryHandler.HandleResourceDelivered(resource);
    }

    private void OnBuildPointReached(CollectorBot bot)
    {
        UnsubscribeBot(bot);
        _constructionController.HandleBuildPointReached(bot);
    }
}