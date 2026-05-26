using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseBotsDatabase : MonoBehaviour
{
    private readonly List<CollectorBot> _collectorBots = new();

    public event Action<CollectorBot> BotAdded;

    public int CurrentBotsCount => _collectorBots.Count;

    public bool TryGetFreeBot(out CollectorBot freeBot)
    {
        freeBot = null;

        foreach (CollectorBot bot in _collectorBots)
        {
            if (bot.IsBusy == false)
            {
                freeBot = bot;
                return true;
            }
        }

        return false;
    }

    public bool TryGetFreeBotMultiple(out List<CollectorBot> freeBots)
    {
        freeBots = new List<CollectorBot>();

        foreach (CollectorBot bot in _collectorBots)
        {
            if (bot.IsBusy == false)
            {
                freeBots.Add(bot);
            }
        }

        return freeBots.Count > 0;
    }

    public CollectorBot RemoveBot(CollectorBot bot)
    {
        if (bot == null)
            return null;

        _collectorBots.Remove(bot);

        return bot;
    }

    public void AddBot(CollectorBot bot)
    {
        if (bot == null)
            return;

        if (_collectorBots.Contains(bot))
            return;

        _collectorBots.Add(bot);

        BotAdded?.Invoke(bot);
    }

    public List<CollectorBot> GetAllBots()
    {
        List<CollectorBot> bots = new List<CollectorBot>();

        foreach (CollectorBot bot in _collectorBots)
        {
            bots.Add(bot);
        }

        return bots;
    }

}