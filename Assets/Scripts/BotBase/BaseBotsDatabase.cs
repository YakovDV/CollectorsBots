using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseBotsDatabase : MonoBehaviour
{
    [SerializeField] private BotSpawner _spawner;
    [SerializeField] private BaseResourceCollector _resourceCollector;
    [SerializeField] private Transform _spawnZone;

    private readonly List<CollectorBot> _collectorBots = new();
    private readonly List<CollectorBot> _freeCollectorBots = new();

    public event Action<CollectorBot> BotAdded;

    public int CurrentBotsCount => _collectorBots.Count;

    public void SetSpawner(BotSpawner spawner)
    {
        _spawner = spawner;
    }

    public bool TryGetFreeBot(out CollectorBot freeBot)
    {
        freeBot = null;

        if (_freeCollectorBots.Count > 0)
        {
            freeBot = _freeCollectorBots[0];

            return true;
        }

        return false;
    }

    public CollectorBot RemoveBot(CollectorBot bot)
    {
        if (bot == null)
            return null;

        _collectorBots.Remove(bot);
        _freeCollectorBots.Remove(bot);

        bot.StatusChanged -= OnBotStatusChanged;

        return bot;
    }

    public void AddBot(CollectorBot bot)
    {
        if (bot == null)
            return;

        if (_collectorBots.Contains(bot))
            return;

        bot.SetBasePosition(_resourceCollector.transform);

        _collectorBots.Add(bot);
        _freeCollectorBots.Add(bot);

        bot.StatusChanged += OnBotStatusChanged;

        BotAdded?.Invoke(bot);
    }

    public List<CollectorBot> GetAllBots()
    {
        return _collectorBots.ToList();
    }

    public void SpawnInitialBots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CreateNewBot();
        }
    }

    public CollectorBot CreateNewBot()
    {
        Vector3 spawnPosition = CalculateRandomSpawnPoint();

        CollectorBot bot = _spawner.SpawnNewBot(spawnPosition);

        AddBot(bot);

        return bot;
    }

    private Vector3 CalculateRandomSpawnPoint()
    {
        Vector3 point = new(UnityEngine.Random.Range(-0.5f, 0.5f), 0f, UnityEngine.Random.Range(-0.5f, 0.5f));

        return _spawnZone.TransformPoint(point);
    }

    private void OnBotStatusChanged(CollectorBot bot, bool isBusy)
    {
        if (isBusy == false)
        {
            if (_freeCollectorBots.Contains(bot) == false)
                _freeCollectorBots.Add(bot);
        }
        else
        {
            _freeCollectorBots.Remove(bot);
        }
    }
}