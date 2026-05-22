using System;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private BotPool _botPool;
    [SerializeField] private Transform _spawnZone;
    [SerializeField] private BaseResourceCollector _collector;
    [SerializeField] private int _initialBotCount = 3;

    private readonly List<CollectorBot> _collectorBots = new();

    public event Action<CollectorBot> BotSpawned;

    private void Start()
    {
        for (int i = 0; i < _initialBotCount; i++)
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

    private void SpawnBot()
    {
        CollectorBot bot = _botPool.GetObject();
        bot.transform.position = CalculateRandomSpawnPoint();
        bot.SetBase(_collector.transform);

        _collectorBots.Add(bot);

        BotSpawned?.Invoke(bot);
    }

    private Vector3 CalculateRandomSpawnPoint()
    {
        Vector3 point = new(UnityEngine.Random.Range(-0.5f, 0.5f), 0f, UnityEngine.Random.Range(-0.5f, 0.5f));

        return _spawnZone.TransformPoint(point);
    }
}