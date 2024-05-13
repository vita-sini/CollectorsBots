using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private ObjectPool _pool;

    private void Start()
    {
        StartCoroutine(GenerateResource());
    }

    private IEnumerator GenerateResource()
    {
        var wait = new WaitForSeconds(_delay);

        while (enabled)
        {
            Spawn();
            yield return wait;
        }
    }

    private void Spawn()
    {
        int spawnPointNumber = Random.Range(0, _spawnPoints.Length);

        _spawnPoints[spawnPointNumber].transform.position = new Vector3(Random.Range(-20.0f, 20.0f), 0.5f, Random.Range(-20.0f, 20.0f));

        var resourse = _pool.GetObject();

        resourse.transform.position = _spawnPoints[spawnPointNumber].transform.position;
       
        resourse.gameObject.SetActive(true);

    }
}
