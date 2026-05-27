using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private CollectorBot _prefab;

    public CollectorBot SpawnNewBot(Vector3 spawnPosition)
    {
        CollectorBot bot = Instantiate(_prefab, spawnPosition, Quaternion.identity);

        return bot;
    }

    public void DestroyBot(CollectorBot bot)
    {
        Destroy(bot.gameObject);
    }
}