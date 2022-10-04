using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    public Transform StartPoint { get; private set; }
    public Player Player { get; private set; }
    private SpriteRenderer _spriteRenderer;
    private List<Vector3> _pastAndPresentPositions;
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _pastAndPresentPositions = new List<Vector3>();
        _pastAndPresentPositions.Add(new Vector3 (0,0,0));
    }

    private void Update()
    {
        _pastAndPresentPositions.Add(transform.position);

        if (_pastAndPresentPositions.Count > 2)
        {
            _pastAndPresentPositions.Remove(_pastAndPresentPositions[0]);           
        }

        if (_pastAndPresentPositions[0].x > _pastAndPresentPositions[1].x)
        {
            TurnLeft();
        }
        else if (_pastAndPresentPositions[0].x < _pastAndPresentPositions[1].x)
        {
            TurnRight();
        }
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_currentHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    public void TurnLeft()
    {
        _spriteRenderer.flipX = true;
    }

    public void TurnRight()
    {
        _spriteRenderer.flipX = false;
    }

    public void InitTarget(Player player)
    {
        Player = player;
    }

    public void InitStartPoint(Transform startPoint)
    {
        StartPoint = startPoint;
    }
}
