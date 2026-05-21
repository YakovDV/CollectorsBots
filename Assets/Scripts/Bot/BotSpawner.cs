using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private BotPool _botPool;
    [SerializeField] private Transform _spawnZone;
    [SerializeField] private BaseResourceCollector _collector;
    [SerializeField] private BotShop _botShop;
    [SerializeField] private int _initialBotCount = 2;

    public List<CollectorBot> CollectorBots { get; private set; }

    private void Start()
    {
        CollectorBots = new List<CollectorBot>();

        for (int i = 0; i < _initialBotCount; i++)
        {
            SpawnBot();
        }
    }

    private void OnEnable()
    {
        _botShop.OnBotRequested += SpawnBot;
    }

    private void OnDisable()
    {
        _botShop.OnBotRequested -= SpawnBot;
    }

    private void SpawnBot()
    {
        CollectorBot bot = _botPool.GetObject();
        bot.transform.position = CalculateRandomSpawnPoint();
        bot.SetResourceCollector(_collector);

        CollectorBots.Add(bot);
    }

    private Vector3 CalculateRandomSpawnPoint()
    {
        Vector3 point = new(Random.Range(-0.5f, 0.5f), 0f, Random.Range(-0.5f, 0.5f));

        return _spawnZone.TransformPoint(point);
    }
}