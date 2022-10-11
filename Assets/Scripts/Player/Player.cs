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
    [SerializeField] private Quaternion _maxQuaterniton;
    [SerializeField] private float _delayChangeWeapon;
    [SerializeField] private float _duretionHit;    
    [SerializeField] private int _maxHealth;    

    private WaitForSeconds _delayChangeWeaponWork;
    private WaitForSeconds _delayBetweenAttacksWork;
    private WaitForSeconds _duretionHitWork;
    private WaitForSeconds _duretionReloadWork;

    private SpriteRenderer _spriteRenderer;

    private List<Vector3> _pastAndPresentPositions;

    private Transform _currentShootPoint;

    private Coroutine _blockQuaternionWork;
    private Coroutine _changeWeaponWork;
    private Coroutine _setDerictionWork;
    private Coroutine _attackWork;
    private Coroutine _takeHitWork;    
    private Coroutine _reloadWork;

    private Animator _animator;

    private Weapon _currentWeapon;

    private Mover _mover;

    private KeyCode _changeWeapon = KeyCode.L;
    private KeyCode _shoot = KeyCode.K;

    private string _changeGunToAxe = "ChangeGunToAxe";
    private string _changeAxeToGun = "ChangeAxeToGun";

    private string _takeHitGun = "HitGun";
    private string _takeHitAxe = "HitAxe";

    private string _shootGun = "ShootFromGun";
    private string _attackAxe = "AttackAxe";
    
    private string _reloadGun = "ReloadGun";
    private string _reloadAxe = "IdleAxe";


    private string _currentChangeWeapon;
    private string _currentTakeHit;
    private string _currentAttack;
    private string _currentReload;

    private bool _isTurnRight;

    private int _currentWeaponNumber;
    private int _currentHealth;

    public bool IsTurnRight => _isTurnRight;

    public Weapon CurrentWeapon => _currentWeapon;

    public event UnityAction <int, int> ChangedHealth;

    private void Start()
    {
        _currentWeapon = _weapons[0];
        _currentHealth = _maxHealth;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _mover = GetComponent<Mover>();

        _pastAndPresentPositions = new List<Vector3>();
        _pastAndPresentPositions.Add(new Vector3(0, 0, 0));
        
        _delayChangeWeaponWork = new WaitForSeconds(_delayChangeWeapon);
        _duretionHitWork = new WaitForSeconds(_duretionHit);       

        _blockQuaternionWork = StartCoroutine(BlockQuaternion());

        _setDerictionWork = StartCoroutine(SetDirection());
        _changeWeaponWork = StartCoroutine(ChangeWeapon());
        _attackWork = StartCoroutine(Attack());

        SetSkin();
        TurnRight();
    }

    private IEnumerator Attack()
    {
        bool isShoot = false;

        _delayBetweenAttacksWork = new WaitForSeconds(_currentWeapon.DelayBetweenAttacks);

        while (true)
        {
            if (Input.GetKey(_shoot) & isShoot == false)
            {
                isShoot = true;
                _mover.StopCoroutineMove();
                StopCoroutine(_setDerictionWork);
                StopCoroutine(_changeWeaponWork);                

                _currentWeapon.Attack(_currentShootPoint, this);
                _animator.Play(_currentAttack);                

                yield return _delayBetweenAttacksWork;
            }

            if (isShoot == true)
            {
                isShoot = false;
                _reloadWork = StartCoroutine(Reload());
                _setDerictionWork = StartCoroutine(SetDirection());
                _changeWeaponWork = StartCoroutine(ChangeWeapon());
            }

            yield return null;
        }
    }

    private IEnumerator Reload()
    {
        bool isReloard = false;

        _duretionReloadWork = new WaitForSeconds(_currentWeapon.DuretionReload);

        while (true)
        {
            if (isReloard == false)
            {
                _mover.StopCoroutineMove();
                StopCoroutine(_setDerictionWork);
                StopCoroutine(_changeWeaponWork);
                StopCoroutine(_attackWork);

                _animator.Play(_currentReload);
                isReloard = true;
                yield return _duretionReloadWork;
            }

            else if (isReloard == true)
            {
                _mover.StartCoroutineMove();
                _setDerictionWork = StartCoroutine(SetDirection());
                _changeWeaponWork = StartCoroutine(ChangeWeapon());
                StopCoroutine(_reloadWork);
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
            }
            else if (_pastAndPresentPositions[0].x > _pastAndPresentPositions[1].x)
            {
                TurnLeft();
            }

            yield return null;
        }
    }

    public IEnumerator ChangeWeapon()
    {
        bool isWeaponChanged = false;

        while (true)
        {
            if (Input.GetKey(_changeWeapon) & isWeaponChanged == false)
            {
                _mover.StopCoroutineMove();                
                StopCoroutine(_setDerictionWork);                
                StopCoroutine(_attackWork);

                _animator.Play(_currentChangeWeapon);
                _currentWeaponNumber++;
                
                isWeaponChanged = true;

                yield return _delayChangeWeaponWork;
            }

            if (isWeaponChanged == true)
            {
                if (_currentWeaponNumber == _weapons.Count)
                {
                    _currentWeaponNumber = 0;
                }

                _currentWeapon = _weapons[_currentWeaponNumber];

                _setDerictionWork = StartCoroutine(SetDirection());
                _attackWork = StartCoroutine(Attack());
                _mover.StartCoroutineMove();
                
                SetSkin();
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

    public Weapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public void ApplyDamage(int damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        ChangedHealth?.Invoke(_currentHealth, _maxHealth);

        if (_takeHitWork != null)
        {
            StopCoroutine(_takeHitWork);
        }
        
        _takeHitWork = StartCoroutine(TakeHit());

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

    private IEnumerator TakeHit()
    {
        bool takeHit = false;

        while (true)
        {
            if (takeHit == false)
            {
                _mover.StopCoroutineMove();
                StopCoroutine(_changeWeaponWork);
                StopCoroutine(_setDerictionWork);
                StopCoroutine(_attackWork);

                _animator.Play(_currentTakeHit);

                takeHit = true;

                yield return _duretionHitWork;
            }

            if (takeHit == true)
            {
                _setDerictionWork = StartCoroutine(SetDirection());
                _attackWork = StartCoroutine(Attack());
                _changeWeaponWork = StartCoroutine(ChangeWeapon());
                _mover.StartCoroutineMove();
                StopCoroutine(_takeHitWork);
            }

            yield return null;
        }        
    }

    private void SetSkin()
    {
        if (_currentWeapon.TryGetComponent<Gun>(out Gun gun))
        {
            _mover.ChangeSkinsToGun();
            _currentChangeWeapon = _changeGunToAxe;
            _currentTakeHit = _takeHitGun;
            _currentAttack = _shootGun;
            _currentReload = _reloadGun;
        }
        else if (_currentWeapon.TryGetComponent<Axe>(out Axe axe))
        {
            _mover.ChangeSkinsToAxe();
            _currentChangeWeapon = _changeAxeToGun;
            _currentTakeHit = _takeHitAxe;
            _currentAttack = _attackAxe;
            _currentReload = _reloadAxe;
        }
    }


    private void StartAllCoroutinesInPlayer()
    {
        _mover.StartCoroutineMove();

        _blockQuaternionWork = StartCoroutine(BlockQuaternion());
        _changeWeaponWork = StartCoroutine(ChangeWeapon());
        _setDerictionWork = StartCoroutine(SetDirection());
        _attackWork = StartCoroutine(Attack());
        _takeHitWork = StartCoroutine(TakeHit());
        _reloadWork = StartCoroutine(Reload());
    }

    private void StopAllCoroutinesInPlayer()
    {
        _mover.StopCoroutineMove();

        StopCoroutine(_blockQuaternionWork);
        StopCoroutine(_changeWeaponWork);
        StopCoroutine(_setDerictionWork);
        StopCoroutine(_attackWork);
        StopCoroutine(_takeHitWork);
        StopCoroutine(_reloadWork);
    }
}