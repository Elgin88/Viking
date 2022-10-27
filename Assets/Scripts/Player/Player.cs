using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Mover))]

public class Player : MonoBehaviour
{
    [SerializeField] private Quaternion _maxQuaterniton;
    [SerializeField] private List<Transform> _shootPoints;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private float _delayChangeWeapon;
    [SerializeField] private float _duretionHit;
    [SerializeField] private float _durationDeath;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxNumberBullets;

    private WaitForSeconds _delayBetweenAttacksWork;
    private WaitForSeconds _delayChangeWeaponWork;
    private WaitForSeconds _duretionReloadWork;
    private WaitForSeconds _duretionDeathWork;
    private WaitForSeconds _duretionHitWork;

    private SpriteRenderer _spriteRenderer;
    private List<Vector3> _pastAndPresentPositions;
    private Transform _currentShootPoint;
    private Animator _animator;
    private Weapon _currentWeapon;
    private Mover _mover;

    private Coroutine _blockQuaternionWork;
    private Coroutine _changeWeaponWork;
    private Coroutine _setDerictionWork;
    private Coroutine _attackWork;
    private Coroutine _takeHitWork;    
    private Coroutine _reloadWork;
    private Coroutine _deathWork;

    private Sounds _sounds;

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
    private string _deathGun = "DeathGun";
    private string _deathAxe = "DeathAxe";
    private string _currentChangeWeapon;
    private string _currentTakeHit;
    private string _currentAttack;
    private string _currentReload;
    private string _currentDeath;

    private int _currentWeaponNumber;
    private int _currentHealth;
    private int _currentNumberKills;
    private int _currentNumberBullets;

    private bool _isTurnRight;

    public bool IsTurnRight => _isTurnRight;
    public Weapon CurrentWeapon => _currentWeapon;
    public int MaxNumberBullets => _maxNumberBullets;
    public int CurrentNumberKills => _currentNumberKills;

    public event UnityAction <int, int> ChangedHealth;
    public event UnityAction <int> ChangedNumberKills;
    public event UnityAction<int> ChangedNumberBullets;

    private void Start()
    {
        _currentWeapon = _weapons[0];
        _currentHealth = _maxHealth;
        _currentNumberKills = 0;
        _currentNumberBullets = _maxNumberBullets;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _sounds = GetComponent<Sounds>();
        _mover = GetComponent<Mover>();

        _pastAndPresentPositions = new List<Vector3>();
        _pastAndPresentPositions.Add(new Vector3(0, 0, 0));
        
        _delayChangeWeaponWork = new WaitForSeconds(_delayChangeWeapon);
        _duretionDeathWork = new WaitForSeconds(_durationDeath);
        _duretionHitWork = new WaitForSeconds(_duretionHit);

        _blockQuaternionWork = StartCoroutine(BlockQuaternion());

        _setDerictionWork = StartCoroutine(SetDirection());
        _changeWeaponWork = StartCoroutine(ChangeWeapon());
        _attackWork = StartCoroutine(Attack());

        SetSprites();
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
                _mover.StopCoroutineMove();

                if (_changeWeaponWork != null)
                {
                    StopCoroutine(_changeWeaponWork);
                    _changeWeaponWork = null;
                }

                if (_currentNumberBullets > 0 & _currentWeapon.TryGetComponent<Gun>(out Gun gun))
                {
                    _currentNumberBullets--;
                    ChangedNumberBullets?.Invoke(_currentNumberBullets);
                    _currentWeapon.Attack(_currentShootPoint, this);
                    _animator.Play(_currentAttack);
                    _sounds.PlayAttackGun();
                }

                else if (_currentWeapon.TryGetComponent<Axe>(out Axe axe))
                {
                    _currentWeapon.Attack(_currentShootPoint, this);
                    _animator.Play(_currentAttack);
                    _sounds.PlayAttackAxe();
                }

                isShoot = true;

                yield return _delayBetweenAttacksWork;
            }

            if (isShoot == true)
            {
                _mover.StartCoroutineMove();

                if (_changeWeaponWork == null)
                {
                    _changeWeaponWork = StartCoroutine(ChangeWeapon());
                }

                if (_reloadWork == null)
                {
                    _reloadWork = StartCoroutine(Reload());
                }

                isShoot = false;
            }

            yield return null;
        }
    }

    private IEnumerator Reload()
    {
        bool isReload = false;

        _duretionReloadWork = new WaitForSeconds(_currentWeapon.DuretionReload);

        while (true)
        {
            if (isReload == false)
            {
                _mover.StopCoroutineMove();

                if (_changeWeaponWork != null)
                {
                    StopCoroutine(_changeWeaponWork);
                    _changeWeaponWork = null;
                }

                if (_attackWork != null)
                {
                    StopCoroutine(_attackWork);
                    _attackWork = null;
                }                   

                _animator.Play(_currentReload);                

                if (_currentWeapon.TryGetComponent<Gun>(out Gun gun))
                {
                    _sounds.PlayReloadGun();
                }
                
                isReload = true;

                yield return _duretionReloadWork;
            }

            else if (isReload == true)
            {
                _mover.StartCoroutineMove();

                if (_setDerictionWork == null)
                {
                    _setDerictionWork = StartCoroutine(SetDirection());
                }

                if (_changeWeaponWork == null)
                {
                    _changeWeaponWork = StartCoroutine(ChangeWeapon());
                }

                if (_attackWork == null)
                {
                    _attackWork = StartCoroutine(Attack());
                }

                if (_reloadWork != null)
                {
                    StopCoroutine(_reloadWork);
                    _reloadWork = null;
                }
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

                if (_setDerictionWork != null)
                {
                    StopCoroutine(_setDerictionWork);
                    _setDerictionWork = null;
                }

                if (_attackWork != null)
                {
                    StopCoroutine(_attackWork);
                    _attackWork = null;
                }

                _animator.Play(_currentChangeWeapon);
                _sounds.PlayChangeWeapon();

                _currentWeaponNumber++;

                if (_currentWeaponNumber >= _weapons.Count)
                {
                    _currentWeaponNumber = 0;
                }

                isWeaponChanged = true;

                yield return _delayChangeWeaponWork;
            }

            if (isWeaponChanged == true)
            {
                _currentWeapon = _weapons[_currentWeaponNumber];

                _mover.StartCoroutineMove();

                if (_setDerictionWork == null)
                    _setDerictionWork = StartCoroutine(SetDirection());

                if (_attackWork == null)
                    _attackWork = StartCoroutine(Attack());

                SetSprites();
                isWeaponChanged = false;
            }

            yield return null;
        }
    }

    private IEnumerator TakeHit()
    {
        bool takeHit = false;

        while (true)
        {
            if (takeHit == false)
            {
                _mover.StopCoroutineMove();

                if (_setDerictionWork != null)
                {
                    StopCoroutine(_setDerictionWork);
                    _setDerictionWork = null;
                }

                if (_attackWork!=null)
                {
                    StopCoroutine(_attackWork);
                    _attackWork = null;
                }

                if (_changeWeaponWork != null)
                {
                    StopCoroutine(_changeWeaponWork);
                    _changeWeaponWork = null;
                }                                   

                if (_reloadWork != null)
                {
                    StopCoroutine(_reloadWork);
                    _reloadWork = null;
                }                                  

                _animator.Play(_currentTakeHit);
                _sounds.PlayTakeDamage();

                takeHit = true;

                yield return _duretionHitWork;
            }

            if (takeHit == true)
            {
                _mover.StartCoroutineMove();

                if (_changeWeaponWork == null)
                    _changeWeaponWork = StartCoroutine(ChangeWeapon());

                if (_setDerictionWork == null)
                    _setDerictionWork = StartCoroutine(SetDirection());

                if (_attackWork == null)
                    _attackWork = StartCoroutine(Attack());

                if (_takeHitWork != null)
                    StopCoroutine(_takeHitWork);
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
                _pastAndPresentPositions.Remove(_pastAndPresentPositions[0]);

            if (_pastAndPresentPositions[0].x < _pastAndPresentPositions[1].x)
                TurnRight();

            else if (_pastAndPresentPositions[0].x > _pastAndPresentPositions[1].x)
                TurnLeft();

            yield return null;
        }
    }

    private IEnumerator Death()
    {
        bool isDeath = false;

        while (true)
        {
            if (isDeath == false)
            {
                _mover.StopCoroutineMove();

                if (_changeWeaponWork != null)
                {
                    StopCoroutine(_changeWeaponWork);
                    _changeWeaponWork = null;
                }

                if (_setDerictionWork != null)
                {
                    StopCoroutine(_setDerictionWork);
                    _setDerictionWork = null;
                }

                if (_attackWork != null)
                {
                    StopCoroutine(_attackWork);
                    _attackWork = null;
                }

                _animator.Play(_currentDeath);
                _sounds.PlayDie();
                isDeath = true;

                yield return _duretionDeathWork;
            }
            else if (isDeath == true)
            {
                Destroy(gameObject);
                StopCoroutine(_deathWork);
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
            _takeHitWork = null;
        }                  

        if (_currentHealth > 0)
            _takeHitWork = StartCoroutine(TakeHit());        

        if (_currentHealth == 0)
        {
            if (_deathWork == null)
            {
                _deathWork = StartCoroutine(Death());
            }
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

    private void SetSprites()
    {
        if (_currentWeapon.TryGetComponent<Gun>(out Gun gun))
        {
            _mover.ChangeSkinsToGun();
            _currentChangeWeapon = _changeGunToAxe;
            _currentTakeHit = _takeHitGun;
            _currentAttack = _shootGun;
            _currentReload = _reloadGun;
            _currentDeath = _deathGun;
        }
        else if (_currentWeapon.TryGetComponent<Axe>(out Axe axe))
        {
            _mover.ChangeSkinsToAxe();
            _currentChangeWeapon = _changeAxeToGun;
            _currentTakeHit = _takeHitAxe;
            _currentAttack = _attackAxe;
            _currentReload = _reloadAxe;
            _currentDeath = _deathAxe;
        }
    }

    public void IncreaaseNumberKills()
    {
        _currentNumberKills++;
        ChangedNumberKills?.Invoke(_currentNumberKills);
    }


    private void StartAndStopCoroutinesPlayer()
    {
        _mover.StartCoroutineMove();
        _mover.StopCoroutineMove();

        _blockQuaternionWork = StartCoroutine(BlockQuaternion());
        _changeWeaponWork = StartCoroutine(ChangeWeapon());
        _setDerictionWork = StartCoroutine(SetDirection());
        _attackWork = StartCoroutine(Attack());
        _takeHitWork = StartCoroutine(TakeHit());
        _reloadWork = StartCoroutine(Reload());
        _deathWork = StartCoroutine(Death());        

        if (_blockQuaternionWork != null)
        {
            StopCoroutine(_blockQuaternionWork);
            _blockQuaternionWork = null;
        }

        if (_changeWeaponWork != null)
        {
            StopCoroutine(_changeWeaponWork);
            _changeWeaponWork = null;
        }

        if (_setDerictionWork != null)
        {
            StopCoroutine(_setDerictionWork);
            _setDerictionWork = null;
        }

        if (_attackWork != null)
        {
            StopCoroutine(_attackWork);
            _attackWork = null;
        }

        if (_takeHitWork != null)
        {
            StopCoroutine(_takeHitWork);
            _takeHitWork = null;
        }

        if (_reloadWork != null)
        {
            StopCoroutine(_reloadWork);
            _reloadWork = null;
        }

        if (_deathWork != null)
        {
            StopCoroutine(_deathWork);
            _deathWork = null;
        }
    }
}