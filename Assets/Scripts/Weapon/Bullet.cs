using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;

    private SpriteRenderer _spriteRenderer;
    private Player _player;
    private int _direction = 1;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_player.IsTurnRight == false)
        {
            _direction = -1;
            _spriteRenderer.flipX = true;
        }

        transform.Translate(Vector2.right * _speed * _direction * Time.deltaTime, Space.World);
    }

    public void Init(Player player)
    {
        _player = player;
    }
}
