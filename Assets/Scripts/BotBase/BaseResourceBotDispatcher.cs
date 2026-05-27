using System.Collections;
using UnityEngine;

public class BaseResourceBotDispatcher : MonoBehaviour
{
    [SerializeField] private BaseBotsDatabase _botsDatabase;
    [SerializeField] private BaseConstruction _constructionController;
    [SerializeField] private float _sendFrequency = 5f;

    private ResourceDatabase _resourceDatabase;
    private Coroutine _sendingBots;

    public void SetResourceDatabase(ResourceDatabase resourceDatabase)
    {
        _resourceDatabase = resourceDatabase;
    }

    public void StartWork()
    {
        if (_sendingBots != null)
            return;

        _sendingBots = StartCoroutine(SendBotsFrequently());
    }

    public void StopWork()
    {
        if (_sendingBots == null)
            return;

        StopCoroutine(_sendingBots);
        _sendingBots = null;
    }

    private IEnumerator SendBotsFrequently()
    {
        WaitForSeconds wait = new(_sendFrequency);

        while (enabled)
        {
            SendBots();
            yield return wait;
        }
    }

    private void SendBots()
    {
        while (_botsDatabase.TryGetFreeBot(out CollectorBot freeBot))
        {
            if (TrySendResourceBot(freeBot) == false)
                return;
        }
    }

    private bool TrySendResourceBot(CollectorBot bot)
    {
        if (_constructionController.IsBuilderBot(bot))
            return false;

        if (_resourceDatabase.TryRequestResource(out Resource resource) == false)
            return false;

        bot.SetTargetResource(resource);

        return true;
    }
}