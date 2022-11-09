using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform _upLimit;
    [SerializeField] private Transform _downLimit;
    [SerializeField] private Transform _leftLimit;
    [SerializeField] private Transform _rightLimit;

    private Coroutine _restrictMoveWork = null;
    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>();

        if (_restrictMoveWork == null)
            _restrictMoveWork = StartCoroutine(RestrictMove());
    }

    private IEnumerator RestrictMove()
    {
        while (true)
        {
            if (transform.position.y > _upLimit.position.y)
            {
                _player.SetPosition(_player.transform.position.x, _upLimit.position.y);
            }
            else if (transform.position.y < _downLimit.position.y)
            {
                _player.SetPosition(_player.transform.position.x, _downLimit.position.y);
            }
            else if (transform.position.x < _leftLimit.position.x)
            {
                _player.SetPosition(_leftLimit.position.x, transform.position.y);
            }
            else if (transform.position.x > _rightLimit.position.x)
            {
                _player.SetPosition(_rightLimit.position.x, transform.position.y);
            }            

            yield return null;
        }
    }
}