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
    private bool _isTurnRight = true;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_isTurnRight == false)
        {
            _direction = -1;
            _spriteRenderer.flipX = true;
        }
        else
        {
            _direction = 1;
            _spriteRenderer.flipX = false;
        }

        transform.Translate(Vector2.right * _speed * _direction * Time.deltaTime, Space.Self);
    }

    public void SetDirection(bool isTurnRight)
    {
        _isTurnRight = isTurnRight;
    }

    public void Init(Player player)
    {
        _player = player;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.ApplyDamage(_damage);
            Destroy(gameObject);
        }

        if (collision.TryGetComponent<Destroyer>(out Destroyer destroyer))
        {
            Destroy(gameObject);
        }
    }
}
