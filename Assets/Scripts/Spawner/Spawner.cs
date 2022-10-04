using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _spawner;
    [SerializeField] private List<Enemy> _enemyes;
    [SerializeField] private float _delay;
    [SerializeField] private Player _player;

    private Transform[] _spawnPoints;
    private Coroutine _instantiateEnemyWork;
    private WaitForSeconds _delayBetweenSpawn;   


    private void Start()
    {
        _delayBetweenSpawn = new WaitForSeconds(_delay) ;
        _spawnPoints = new Transform[_spawner.childCount];

        for (int i = 0; i < _spawner.childCount; i++)
        {
            _spawnPoints[i] = _spawner.GetChild(i);
        }

        _instantiateEnemyWork = StartCoroutine(InstantiateEnemy());
    }



    private IEnumerator InstantiateEnemy()
    {
        while (true)
        {
            Transform chosenSpawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
            Enemy choseEnemy = _enemyes[Random.Range(0, _enemyes.Count)];

            Enemy enemy = Instantiate(choseEnemy, chosenSpawnPoint.position, Quaternion.identity);

            enemy.InitTarget(_player);
            enemy.InitStartPoint(chosenSpawnPoint);

            yield return _delayBetweenSpawn;
        }
    }
}
