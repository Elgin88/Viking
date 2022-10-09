using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Mover))]

public class Player : MonoBehaviour
{
    [SerializeField] private List<Transform> _shootPoints;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField]private Quaternion _maxQuaterniton;
    [SerializeField] private float _delayChangeWeapon;
    [SerializeField] private float _duretionHit;
    [SerializeField] private float _duretionChangingWeapon;
    [SerializeField] private int _maxHealth;

    private WaitForSeconds _delayBetweenChangeWeapons;
    private WaitForSeconds _delayBetweenBullets;

    private SpriteRenderer _spriteRenderer;

    private List<Vector3> _pastAndPresentPositions;

    private Transform _currentShootPoint;

    private Coroutine _blockQuaternionWork;
    private Coroutine _changeWeaponWork;
    private Coroutine _setDerictionWork;
    private Coroutine _attackWork;
    private Coroutine _hitWork;
    private Coroutine _playAnimationIdle;

    private Mover _mover;

    private Animator _animator;

    private Weapon _currentWeapon;

    private string _changeGunToAxe = "ChangeGunToAxe";
    private string _animationHitGun = "HitGun";
    private string _idleGun = "IdleGun";
    private string _idleAxe = "IdleAxe";

    private bool _isTurnRight;
    private bool _isAttacked;

    private int _currentWeaponNumber;
    private int _currentHealth;

    private KeyCode _changeWeapon = KeyCode.L;
    private KeyCode _shoot = KeyCode.K;

    public bool IsTurnRight => _isTurnRight;
    public bool IsAttacked => _isAttacked;

    public Weapon CurrentWeapon => _currentWeapon;

    public event UnityAction <int, int> ChangedHealth;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _mover = GetComponent<Mover>();

        _isTurnRight = true;

        _currentHealth = _maxHealth;
        _currentWeapon = _weapons[0];

        _pastAndPresentPositions = new List<Vector3>();
        _pastAndPresentPositions.Add(new Vector3(0, 0, 0));

        _delayBetweenChangeWeapons = new WaitForSeconds(_delayChangeWeapon);
        _delayBetweenBullets = new WaitForSeconds(_currentWeapon.DelayBetweenAttacks);

        _blockQuaternionWork = StartCoroutine(BlockQuaternion());
        _changeWeaponWork = StartCoroutine(ChangeWeapon());
        _setDerictionWork = StartCoroutine(SetDirection());
        _attackWork = StartCoroutine(Attack());
        _playAnimationIdle = StartCoroutine(PlayAnimationEdle());

        TurnRight();
    }

    private IEnumerator PlayAnimationEdle()
    {
        while (true)
        {
            if (_currentWeapon.TryGetComponent<Pistol>(out Pistol pistol))
            {
                _animator.Play(_idleGun);
                Debug.Log("Gun");
            }
            else if (_currentWeapon.TryGetComponent<Axe> (out Axe axe))
            {
                _animator.Play(_idleAxe);
                Debug.Log("Axe");
            }

            yield return null;
        }

    }

    private IEnumerator SetDirection()
    {
        while (true)
        {
            _pastAndPresentPositions.Add(new Vector3(transform.position.x, transform.position.y, transform.position.z));

            if (_pastAndPresentPositions.Count > 2)
            {
                _pastAndPresentPositions.Remove(_pastAndPresentPositions[0]);
            }            

            if (_pastAndPresentPositions[0].x < _pastAndPresentPositions[1].x)
            {
                TurnRight();
                _isTurnRight = true;
            }
            else if (_pastAndPresentPositions[0].x > _pastAndPresentPositions[1].x)
            {
                TurnLeft();
                _isTurnRight = false;
            }

            yield return null;
        }
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
        float delayChangeWeapon = 0;
        bool isWeaponChanged = false;

        while (true)
        {
            delayChangeWeapon += Time.deltaTime;

            if (Input.GetKey(_changeWeapon))
            {
                delayChangeWeapon = 0;

                _mover.StopCoroutineMove();
                _mover.StopCoroutineSetDirection();
                _animator.Play(_changeGunToAxe);

                _currentWeaponNumber++;

                isWeaponChanged = true;

                if (_currentWeaponNumber == _weapons.Count)
                {
                    _currentWeaponNumber = 0;
                }                
            }
            
            if (delayChangeWeapon > _delayChangeWeapon & isWeaponChanged == true)
            {
                StopCoroutine(_changeWeaponWork);

                _mover.StartCorounineSetDirection();
                _mover.StartCoroutineMove();

                _changeWeaponWork = StartCoroutine(ChangeWeapon());
            }

            yield return null;
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

        TakeHit();

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

    private void TakeHit()
    {
        _hitWork = StartCoroutine(Hit());        
    }

    private IEnumerator Hit()
    {
        float durationHit = 0;

        _mover.StopCoroutineSetDirection();
        _mover.StopCoroutineMove();
        StopCoroutine(_changeWeaponWork);
        
        while (true)
        {
            durationHit += Time.deltaTime;

            _animator.Play(_animationHitGun);

            if (durationHit > _duretionHit)
            {
                _mover.StartCorounineSetDirection();
                _mover.StartCoroutineMove();
                _changeWeaponWork = StartCoroutine(ChangeWeapon());

                StopCoroutine(_hitWork);
            }

            yield return null;
        }
    }

    private IEnumerator ChangeWeaponCoroutine()
    {
        float durationChangeWeapon = 0;

        while (true)
        {
            durationChangeWeapon += Time.deltaTime;

            if (_currentWeapon.TryGetComponent<Pistol>(out Pistol pistol))
            {
                _animator.Play(_changeGunToAxe);

                if (durationChangeWeapon > _duretionChangingWeapon)
                {
    
                }
            }

            yield return null;
        }
    }


}