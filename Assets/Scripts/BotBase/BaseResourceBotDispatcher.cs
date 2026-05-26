using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseResourceBotDispatcher : MonoBehaviour
{
    [SerializeField] private BaseBotsDatabase _botsDatabase;
    [SerializeField] private BaseConstructionController _constructionController;
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
        if (_botsDatabase.TryGetFreeBotMultiple(out List<CollectorBot> bots))
        {
            foreach (CollectorBot bot in bots)
            {
                TrySendResourceBot(bot);
            }
        }
    }

    private void TrySendResourceBot(CollectorBot bot)
    {
        if (_constructionController.IsBuilderBot(bot))
            return;

        if (_resourceDatabase.TryRequestResource(out Resource resource) == false)
            return;

        bot.SetTarget(resource.transform);
    }
}