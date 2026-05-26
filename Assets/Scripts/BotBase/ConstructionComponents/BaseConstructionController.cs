using UnityEngine;

public class BaseConstructionController : MonoBehaviour
{
    [SerializeField] private BaseBotsDatabase _botsDatabase;
    [SerializeField] private BaseBotsController _botsController;
    [SerializeField] private BaseFlagPlacer _flagPlacer;
    [SerializeField] private int _minBotsToBuildBase = 2;

    private BaseBuilder _builder;
    private CollectorBot _builderBot;

    public bool HasEnoughBotsToBuild => _botsDatabase.CurrentBotsCount >= _minBotsToBuildBase;

    public void SetBaseBuilder(BaseBuilder builder)
    {
        _builder = builder;
    }

    public bool IsBuilderBot(CollectorBot bot)
    {
        return bot == _builderBot;
    }

    public bool TrySendBuilderBot()
    {
        if (_flagPlacer.HasFlag == false)
            return false;

        if (_builderBot != null)
            return false;

        if (HasEnoughBotsToBuild == false)
            return false;

        if (_botsDatabase.TryGetFreeBot(out CollectorBot bot) == false)
            return false;

        _builderBot = bot;
        bot.SetBuildTarget(_flagPlacer.CurrentFlag);

        return true;
    }

    public void HandleBuildPointReached(CollectorBot bot)
    {
        if (bot != _builderBot)
            return;

        Vector3 buildPosition = _flagPlacer.FlagPosition.transform.position;

        _builderBot = null;

        _flagPlacer.RemoveFlag();

        _botsController.RemoveBot(bot);

        Base newBase = _builder.CreateBase(buildPosition);
        newBase.AddBotToBase(bot);
    }
}
