using UnityEngine;

public class BaseBotsController : MonoBehaviour
{
    [SerializeField] private BotSpawner _spawner;
    [SerializeField] private BaseBotsDatabase _botsDatabase;
    [SerializeField] private BaseResourceCollector _resourceCollector;
    [SerializeField] private Transform _spawnZone;

    public void SetSpawner(BotSpawner spawner)
    {
        _spawner = spawner;
    }

    public void SpawnInitialBots(int count)
    {
        for (int i = 0; i < count; i++)
        {
            RequestNewBot();
        }
    }

    public CollectorBot RequestNewBot()
    {
        Vector3 spawnPosition = CalculateRandomSpawnPoint();

        CollectorBot bot = _spawner.SpawnNewBot(spawnPosition);
        AddBot(bot);

        return bot;
    }

    public void AddBot(CollectorBot bot)
    {
        if (bot == null)
            return;

        bot.SetBase(_resourceCollector.transform);
        _botsDatabase.AddBot(bot);
    }

    public void RemoveBot(CollectorBot bot)
    {
        _botsDatabase.RemoveBot(bot);
    }

    private Vector3 CalculateRandomSpawnPoint()
    {
        Vector3 point = new(UnityEngine.Random.Range(-0.5f, 0.5f), 0f, UnityEngine.Random.Range(-0.5f, 0.5f));

        return _spawnZone.TransformPoint(point);
    }
}