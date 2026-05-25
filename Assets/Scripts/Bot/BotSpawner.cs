using System;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private BotPool _botPool;
    [SerializeField] private Transform _spawnZone;
    [SerializeField] private BaseResourceCollector _collector;

    private readonly List<CollectorBot> _collectorBots = new();

    public event Action<CollectorBot> BotSpawned;

    public int CurrentBotsCount => _collectorBots.Count;

    public void SpawnInitialBots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnBot();
        }
    }

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

    public void SpawnBot()
    {
        CollectorBot bot = _botPool.GetObject();
        bot.transform.position = CalculateRandomSpawnPoint();
        bot.SetBase(_collector.transform);

        _collectorBots.Add(bot);

        BotSpawned?.Invoke(bot);
    }

    public void SetBotPool(BotPool botPool)
    {
        _botPool = botPool;
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
        bot.SetBase(_collector.transform);

        BotSpawned?.Invoke(bot);
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

    private Vector3 CalculateRandomSpawnPoint()
    {
        Vector3 point = new(UnityEngine.Random.Range(-0.5f, 0.5f), 0f, UnityEngine.Random.Range(-0.5f, 0.5f));

        return _spawnZone.TransformPoint(point);
    }
}