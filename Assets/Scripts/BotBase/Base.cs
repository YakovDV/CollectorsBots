using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private BaseResourceScaner _scaner;
    [SerializeField] private BotSpawner _spawner;

    private void OnEnable()
    {
        _scaner.ResourceFound += TrySendBot;
    }

    private void OnDisable()
    {
        _scaner.ResourceFound -= TrySendBot;
    }

    private void TrySendBot(Resource resource)
    {
        foreach (CollectorBot bot in _spawner.CollectorBots)
        {
            if (bot.IsBusy == false && resource.IsPicked == false)
            {
                bot.SetTarget(resource.transform);
                resource.ChangePickedStatus(true);
                break;
            }
        }
    }
}