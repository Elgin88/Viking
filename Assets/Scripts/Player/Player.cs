using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private List<Transform> _shootPoints;

    private WaitForSeconds _delayBetweenBullets;
    private SpriteRenderer _spriteRenderer;
    private Transform _currentShootPoint;
    private Coroutine _shootWork;

    private bool _isTurnRight;
    private int _curretnHealth;

    private KeyCode _shoot = KeyCode.K;

    public bool IsTurnRight => _isTurnRight;

    private void Start()
    {
        _isTurnRight = true;
        _curretnHealth = _maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _shootWork = StartCoroutine(Shoot());
        _delayBetweenBullets = new WaitForSeconds(_weapon.DelayBetweenBullets);
    }

    private IEnumerator Shoot()
    {
        while (true)
        {
            if (Input.GetKey(_shoot))
            {
                _weapon.Shoot(_currentShootPoint, this);

                yield return _delayBetweenBullets;
            }

            yield return null;
        }
    }

    public void ApplyDamage(int damage)
    {
        _curretnHealth -= damage;
        _curretnHealth = Mathf.Clamp(_curretnHealth, 0, _maxHealth);
    }

    public void ApplyHeal(int heal)
    {
        _curretnHealth += heal;
        _curretnHealth = Mathf.Clamp(_curretnHealth, 0, _maxHealth);
    }

    public void TurnLeft()
    {
        _isTurnRight = false;
        _spriteRenderer.flipX = true;

        if (_shootPoints[0].position.x < _shootPoints[1].position.x)
        {
            _currentShootPoint = _shootPoints[0];
        }
        else
        {
            _currentShootPoint = _shootPoints[1];
        }        
    }

    public void TurnRight()
    {
        _isTurnRight = true;
        _spriteRenderer.flipX = false;

        if (_shootPoints[1].position.x > _shootPoints[0].position.x)
        {
            _currentShootPoint = _shootPoints[1];
        }
        else
        {
            _currentShootPoint = _shootPoints[0];
        }
    }

    public void SetPosition(float positionX,float positionY)
    {
        transform.position = new Vector3(positionX, positionY, transform.position.z);
    }

    public bool GetDirection()
    {
        return _isTurnRight;
    }
}