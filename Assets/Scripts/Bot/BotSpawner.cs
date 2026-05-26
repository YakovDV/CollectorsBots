using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private BotPool _botPool;

    public CollectorBot SpawnNewBot(Vector3 spawnPosition)
    {
        CollectorBot bot = _botPool.GetObject();
        bot.transform.position = spawnPosition;

        return bot;
    }

    public void RemoveBot(CollectorBot bot)
    {
        _botPool.ReleaseObject(bot);
    }
}