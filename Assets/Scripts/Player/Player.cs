using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private List<Transform> _shootPoints;
    [SerializeField]private Quaternion _maxQuaterniton;
    [SerializeField] private float _delayChangeWeapon;

    private Animator _animator;

    private WaitForSeconds _delayBetweenBullets;
    private WaitForSeconds _delayBetweenChangeWeapons;

    private SpriteRenderer _spriteRenderer;

    private Transform _currentShootPoint;

    private Coroutine _attackWork;
    private Coroutine _blockQuaternionWork;
    private Coroutine _changeWeaponWork;

    private Weapon _currentWeapon;



    private bool _isTurnRight;
    private bool _isAttacked;

    private int _currentHealth;
    private int _currentWeaponNumber;

    private KeyCode _shoot = KeyCode.K;
    private KeyCode _changeWeapon = KeyCode.L;

    public bool IsTurnRight => _isTurnRight;
    public bool IsAttacked => _isAttacked;

    public event UnityAction <int, int> ChangedHealth;
    public event UnityAction ChangedWeapon;
    public event UnityAction TakedDamage;

    public Weapon CurrentWeapon => _currentWeapon;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();

        _isTurnRight = true;

        _currentHealth = _maxHealth;
        _currentWeapon = _weapons[0];

        _attackWork = StartCoroutine(Attack());
        _blockQuaternionWork = StartCoroutine(BlockQuaternion());
        _changeWeaponWork = StartCoroutine(ChangeWeapon());

        _delayBetweenBullets = new WaitForSeconds(_currentWeapon.DelayBetweenBullets);
        _delayBetweenChangeWeapons = new WaitForSeconds(_delayChangeWeapon);
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            if (Input.GetKey(_shoot))
            {
                _currentWeapon.Shoot(_currentShootPoint, this);

                yield return _delayBetweenBullets;
            }

            yield return null;
        }
    }

    public IEnumerator ChangeWeapon()
    {
        while (true)
        {
            if (Input.GetKey(_changeWeapon))
            {
                _currentWeaponNumber++;

                if (_currentWeaponNumber > _weapons.Count - 1)
                {
                    _currentWeaponNumber = 0;
                }

                ChangedWeapon?.Invoke();
            }

            yield return _delayBetweenChangeWeapons;
        }
    }


    private IEnumerator BlockQuaternion()
    {
        while (true)
        {
            transform.rotation = _maxQuaterniton;         

            yield return null;
        }
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);       

        ChangedHealth?.Invoke(_currentHealth, _maxHealth);
        TakedDamage?.Invoke();

        if (_currentHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    public void ApplyHeal(int heal)
    {
        _currentHealth += heal;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        ChangedHealth?.Invoke(_currentHealth, _maxHealth);
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