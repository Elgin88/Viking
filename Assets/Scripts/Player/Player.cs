using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Sounds))]
[RequireComponent(typeof(Mover))]

public class Player : MonoBehaviour
{
    [SerializeField] private Quaternion _maxQuaterniton;
    [SerializeField] private List<Transform> _shootPoints;
    [SerializeField] private List<Weapon> _weapons;
    [SerializeField] private float _delayChangeWeapon;
    [SerializeField] private float _duretionHit;
    [SerializeField] private float _durationDeath;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _slowdown;
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _maxNumberBullets;
    [SerializeField] private UnityEvent _isDamaged;

    private List<Vector2> _pastAndPresentPosition;
    private Vector2 _currentDirection;

    private WaitForSeconds _delayBetweenAttacksWork;
    private WaitForSeconds _delayChangeWeaponWork;
    private WaitForSeconds _duretionReloadWork;
    private WaitForSeconds _duretionDeathWork;
    private WaitForSeconds _duretionHitWork;

    private SpriteRenderer _spriteRenderer;
    private Transform _currentShootPoint;
    private Animator _animator;
    private Weapon _currentWeapon;
    private Mover _mover;

    private Coroutine _blockQuaternionWork = null;
    private Coroutine _changeWeaponWork = null;
    private Coroutine _setDerictionWork = null;
    private Coroutine _attackWork = null;
    private Coroutine _takeHitWork = null;    
    private Coroutine _reloadWork = null;
    private Coroutine _deathWork = null;
    private Coroutine _moveWork = null;

    private Sounds _sounds;

    private KeyCode _moveUp = KeyCode.W;
    private KeyCode _moveDown = KeyCode.S;
    private KeyCode _moveRight = KeyCode.D;
    private KeyCode _moveLeft = KeyCode.A;
    private KeyCode _shoot = KeyCode.K;
    private KeyCode _changeWeapon = KeyCode.L;

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

    private float _speedConversionFactor = 0.70709f;
    private float _verticalConstraint;
    private float _currentSpeed;

    private string _runWithGun = "RunGun";
    private string _runWithAxe = "RunAxe";
    private string _idleGun = "IdleGun";
    private string _idleAxe = "IdleAxe";
    private string _currentAnimation;
    private string _currentIdle;
    private string _currentRun;

    private int _currentWeaponNumber;
    private int _currentHealth;
    private int _currentNumberKills;
    private int _currentNumberBullets;

    private bool _isTurnRight;

    public Weapon CurrentWeapon => _currentWeapon;
    public int CurrentNumberKills => _currentNumberKills;
    public int MaxNumberBullets => _maxNumberBullets;
    public bool IsTurnRight => _isTurnRight;

    public event UnityAction <int, int> ChangedHealth;
    public event UnityAction <int> ChangedNumberKills;
    public event UnityAction<int> ChangedNumberBullets;

    private void Start()
    {
        _currentNumberBullets = _maxNumberBullets;
        _currentNumberKills = 0;
        _setDerictionWork = null;
        _currentWeapon = _weapons[0];
        _currentHealth = _maxHealth;

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _sounds = GetComponent<Sounds>();
        _mover = GetComponent<Mover>();

        _delayChangeWeaponWork = new WaitForSeconds(_delayChangeWeapon);
        _duretionDeathWork = new WaitForSeconds(_durationDeath);
        _duretionHitWork = new WaitForSeconds(_duretionHit);
        _pastAndPresentPosition = new List<Vector2>();
        _pastAndPresentPosition.Add(new Vector2(transform.position.x - 1, transform.position.y));

        _blockQuaternionWork = null;
        _changeWeaponWork = null;
        _setDerictionWork = null;
        _attackWork = null;
        _takeHitWork = null;
        _reloadWork = null;
        _deathWork = null;
        _moveWork = null;

        StartBlockQuaternion();
        StartMoveAndSetDirectionCoroutines();
        StartAttactCoroutine();
        StartChangeWeaponCoroutine();

        SetSprites();
        TurnRight();
    }

    private IEnumerator Move()
    {
        while (true)
        {
            if (Input.GetKey(_moveRight) && Input.GetKey(_moveUp))
            {
                _currentAnimation = _currentRun;
                _currentSpeed = _maxSpeed;
                _currentDirection = (Vector2.up + Vector2.right) * _speedConversionFactor;
            }
            else if (Input.GetKey(_moveLeft) && Input.GetKey(_moveUp))
            {
                _currentAnimation = _currentRun;
                _currentSpeed = _maxSpeed;
                _currentDirection = (Vector2.up + Vector2.left) * _speedConversionFactor;
            }
            else if (Input.GetKey(_moveLeft) && Input.GetKey(_moveDown))
            {
                _currentAnimation = _currentRun;
                _currentSpeed = _maxSpeed;
                _currentDirection = (Vector2.down + Vector2.left) * _speedConversionFactor;
            }
            else if (Input.GetKey(_moveDown) && Input.GetKey(_moveRight))
            {
                _currentAnimation = _currentRun;
                _currentSpeed = _maxSpeed;
                _currentDirection = (Vector2.down + Vector2.right) * _speedConversionFactor;
            }
            else if (Input.GetKey(_moveRight))
            {
                _currentAnimation = _currentRun;
                _currentSpeed = _maxSpeed;
                _currentDirection = Vector2.right;
            }
            else if (Input.GetKey(_moveLeft))
            {
                _currentAnimation = _currentRun;
                _currentSpeed = _maxSpeed;
                _currentDirection = Vector2.left;
            }
            else if (Input.GetKey(_moveUp))
            {
                _currentAnimation = _currentRun;
                _currentSpeed = _maxSpeed;
                _currentDirection = Vector2.up;
            }
            else if (Input.GetKey(_moveDown))
            {
                _currentAnimation = _currentRun;
                _currentSpeed = _maxSpeed;
                _currentDirection = Vector2.down;
            }

            if (_currentSpeed <= 0.1f)
            {
                _currentSpeed = 0;
                _currentAnimation = _currentIdle;
            }

            transform.Translate(_currentDirection * _currentSpeed * Time.deltaTime, Space.World);
            _animator.Play(_currentAnimation);

            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _slowdown * Time.deltaTime);

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        bool isShoot = false;

        _delayBetweenAttacksWork = new WaitForSeconds(_currentWeapon.DelayBetweenAttacks);

        while (true)
        {
            if (Input.GetKey(_shoot) & isShoot == false)
            {
                StopMoveAndSetDirectionCoroutine();
                StopChangeWeaponCoroutine();

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
                StartReloadCoroutine();
                StopAttackCoroutine();

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
                _animator.Play(_currentReload);                

                if (_currentWeapon.TryGetComponent<Gun>(out Gun gun))
                    _sounds.PlayReloadGun();                
                
                isReload = true;

                yield return _duretionReloadWork;
            }

            else if (isReload == true)
            {
                StartMoveAndSetDirectionCoroutines();
                StartAttactCoroutine();
                StartChangeWeaponCoroutine();
                StopReloadCoroutine();              
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
                StopMoveAndSetDirectionCoroutine();
                StopAttackCoroutine();

                _animator.Play(_currentChangeWeapon);
                _sounds.PlayChangeWeapon();
                _currentWeaponNumber++;

                if (_currentWeaponNumber >= _weapons.Count)
                    _currentWeaponNumber = 0;

                isWeaponChanged = true;

                yield return _delayChangeWeaponWork;
            }

            if (isWeaponChanged == true)
            {
                _currentWeapon = _weapons[_currentWeaponNumber];

                SetSprites();
                StartMoveAndSetDirectionCoroutines();
                StartAttactCoroutine();

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
                StopMoveAndSetDirectionCoroutine();
                StopAttackCoroutine();
                StopChangeWeaponCoroutine();
                StopReloadCoroutine();

                _animator.Play(_currentTakeHit);
                _sounds.PlayTakeDamage();               

                takeHit = true;

                yield return _duretionHitWork;
            }

            if (takeHit == true)
            {
                StartMoveAndSetDirectionCoroutines();
                StartAttactCoroutine();
                StartChangeWeaponCoroutine();
                StopTakeHitCoroutine();
            }

            yield return null;
        }
    }

    private IEnumerator SetDirection()
    {
        while (true)
        {
            _pastAndPresentPosition.Add(transform.position);

            if (_pastAndPresentPosition.Count > 2)            
                _pastAndPresentPosition.Remove(_pastAndPresentPosition[0]);            

            if (_pastAndPresentPosition[0].x < _pastAndPresentPosition[1].x)            
                TurnRight();

            if (_pastAndPresentPosition[0].x > _pastAndPresentPosition[1].x)
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
                StopMoveAndSetDirectionCoroutine();
                StopAttackCoroutine();
                StopChangeWeaponCoroutine();
                StopTakeHitCoroutine();

                _animator.Play(_currentDeath);
                _sounds.PlayDie();

                isDeath = true;

                yield return _duretionDeathWork;
            }
            else if (isDeath == true)
            {
                if (gameObject != null)
                    Destroy(gameObject);
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

    private void StartMoveAndSetDirectionCoroutines()
    {
        if (_moveWork == null)
            _moveWork = StartCoroutine(Move());

        if (_setDerictionWork == null)
            _setDerictionWork = StartCoroutine(SetDirection());

    }

    private void StartChangeWeaponCoroutine()
    {
        if (_changeWeaponWork == null)
            _changeWeaponWork = StartCoroutine(ChangeWeapon());        
    }

    private void StartAttactCoroutine()
    {
        if (_attackWork == null)
            _attackWork = StartCoroutine(Attack());
    }

    private void StartTakeHitCoroutine()
    {
        if (_takeHitWork == null)
            _takeHitWork = StartCoroutine(TakeHit());
    }

    private void StartReloadCoroutine()
    {
        if (_reloadWork == null)        
            _reloadWork = StartCoroutine(Reload());
    }

    private void StartDeathCoroutine()
    {
        if (_deathWork == null)
            _deathWork = StartCoroutine(Death());
    }

    private void StartBlockQuaternion()
    {
        if (_blockQuaternionWork == null)
            _blockQuaternionWork = StartCoroutine(BlockQuaternion());
    }

    private void StopMoveAndSetDirectionCoroutine()
    {
        if (_moveWork != null)
        {
            StopCoroutine(_moveWork);
            _moveWork = null;
        }

        if (_setDerictionWork != null)
        {
            StopCoroutine(_setDerictionWork);
            _setDerictionWork = null;
        }
    }

    private void StopChangeWeaponCoroutine()
    {
        if (_changeWeaponWork != null)
        {
            StopCoroutine(_changeWeaponWork);
            _changeWeaponWork = null;
        }
    }

    private void StopAttackCoroutine()
    {

        if (_attackWork != null)
        {
            StopCoroutine(_attackWork);
            _attackWork = null;
        }
    }

    private void StopTakeHitCoroutine()
    {
        if (_takeHitWork != null)
        {
            StopCoroutine(_takeHitWork);
            _takeHitWork = null;
        }
    }

    private void StopReloadCoroutine()
    {
        if (_reloadWork != null)
        {
            StopCoroutine(_reloadWork);
            _reloadWork = null;
        }
    }

    private void StopDeathCoroutine()
    {
        if (_deathWork != null)
        {
            StopCoroutine(_deathWork);
            _deathWork = null;
        }
    }

    private void StopQuaterniionCoroutine()
    {
        if (_blockQuaternionWork != null)
        {
            StopCoroutine(_blockQuaternionWork);
            _blockQuaternionWork = null;
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

        _isDamaged?.Invoke();

        ChangedHealth?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth > 0)
            StartTakeHitCoroutine();     

        if (_currentHealth == 0)        
            StartDeathCoroutine();
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
            _currentShootPoint = _shootPoints[0];
        
        else        
            _currentShootPoint = _shootPoints[1];
              
    }

    public void TurnRight()
    {
        _isTurnRight = true;
        _spriteRenderer.flipX = false;

        if (_shootPoints[1].position.x > _shootPoints[0].position.x)
            _currentShootPoint = _shootPoints[1];
        
        else        
            _currentShootPoint = _shootPoints[0];        
    }

    public void ChangeSpritesToGun()
    {
        _currentIdle = _idleGun;
        _currentRun = _runWithGun;
    }

    public void ChangeSpritesToAxe()
    {
        _currentIdle = _idleAxe;
        _currentRun = _runWithAxe;
    }

    public void SetPosition(float positionX,float positionY)
    {
        transform.position = new Vector3(positionX, positionY, transform.position.z);
    }   

    private void SetSprites()
    {
        if (_currentWeapon.TryGetComponent<Gun>(out Gun gun))
        {
            ChangeSpritesToGun();
            _currentChangeWeapon = _changeGunToAxe;
            _currentTakeHit = _takeHitGun;
            _currentAttack = _shootGun;
            _currentReload = _reloadGun;
            _currentDeath = _deathGun;
        }
        else if (_currentWeapon.TryGetComponent<Axe>(out Axe axe))
        {
            ChangeSpritesToAxe();
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
}