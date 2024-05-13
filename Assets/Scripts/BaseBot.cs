using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Scanner))]
[RequireComponent(typeof(CreateBot))]
public class BaseBot : MonoBehaviour
{
    [SerializeField] private float _resourceCollectionDelay = 0.1f;
    [SerializeField] private ObjectPool _pool;
    [SerializeField] private float _spawnRadius = 3f;

    private List<Unit> _bots = new List<Unit>();
    private List<Resource> _resourse = new List<Resource>();

    private CreateBot _createBot;
    private Scanner _scanner;

    public event UnityAction<int> CountFreeResourse;

    private void Awake()
    {
        _scanner = GetComponent<Scanner>();
        _createBot = GetComponent<CreateBot>();
    }

    private void Start()
    {
        CreateBot();
        StartCoroutine(CollectResourcesRoutine());
    }

    public void ReturnResource(Resource resource)
    {
        _pool.PutObject(resource);
        _resourse.Add(resource);
        CountFreeResourse?.Invoke(_resourse.Count);
    }

    private void CreateBot()
    {
        int startCount = 3;

        for (int i = 0; i < startCount; i++)
        {
            float randomX = Random.Range(-_spawnRadius, _spawnRadius);
            float randomZ = Random.Range(-_spawnRadius, _spawnRadius);
            Vector3 randomPosition = transform.position + new Vector3(randomX, 0, randomZ);
            Unit bot = _createBot.SpawnBot(randomPosition);
            bot.SetBaseBot(this);
            _bots.Add(bot);
        }
    }

    private IEnumerator CollectResourcesRoutine()
    {
        var waitSeconds = new WaitForSeconds(_resourceCollectionDelay);

        while (true)
        {
            yield return waitSeconds;
            CollectResource();
        }
    }

    private void CollectResource()
    {
        Resource resource = _scanner.GiveResource();

        if (resource != null)
        {
            foreach (Unit bot in _bots)
            {
                if (!bot._isBusy)
                {
                    bot.SetDestination(resource);
                    break;
                }
            }
        }
    }
}
