using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private Quaternion _maxQuaternion;
    [SerializeField] private float _delayDirection;

    private SpriteRenderer _spriteRenderer;
    private WaitForSeconds _delayBetweenDirection;
    private List<Vector3> _pastAndPresentPositions;
    private Coroutine _setDirectionWork;
    private Coroutine _blockQuaternionWork;
    private int _currentHealth;

    public Transform StartPoint { get; private set; }
    public event UnityAction IsAttacked;
    public Player Target { get; private set; }
    public int CurrentHealth => _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;

        _pastAndPresentPositions = new List<Vector3>();
        _delayBetweenDirection = new WaitForSeconds(_delayDirection);

        _spriteRenderer = GetComponent<SpriteRenderer>();
       

        _pastAndPresentPositions.Add(new Vector3 (0,0,0));

        _blockQuaternionWork = StartCoroutine(BlockQuaternion());
        _setDirectionWork = StartCoroutine(SetDirection());
    }

    public IEnumerator SetDirection()
    {
        while (true)
        {
            _pastAndPresentPositions.Add(transform.position);

            if (_pastAndPresentPositions.Count > 2)
            {
                _pastAndPresentPositions.Remove(_pastAndPresentPositions[0]);
            }

            if (_pastAndPresentPositions[0].x > _pastAndPresentPositions[1].x)
            {
                TurnLeft();
                yield return _delayBetweenDirection;
            }
            else if (_pastAndPresentPositions[0].x < _pastAndPresentPositions[1].x)
            {
                TurnRight();
                yield return _delayBetweenDirection;
            }

            yield return null;
        }
    }

    public void StopSetDirection()
    {
        StopCoroutine(_setDirectionWork);
    }

    private IEnumerator BlockQuaternion()
    {
        while (true)
        {
            transform.rotation = _maxQuaternion;

            yield return null;
        }
    }     

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        
        if (_currentHealth > 0)
            IsAttacked?.Invoke();
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
        Target = player;
    }

    public void InitStartPoint(Transform startPoint)
    {
        StartPoint = startPoint;
    }   
}
