using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Player))]

public class Mover : MonoBehaviour
{
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _slowdown;
    [SerializeField] private Transform _upLimit;
    [SerializeField] private Transform _downLimit;
    [SerializeField] private Transform _leftLimit;
    [SerializeField] private Transform _rightLimit;

    private Vector2 _currentDirection;

    private Coroutine _setMoveCommandWork;
    private Coroutine _moveWork;
    private Animator _animator;
    private Player _player;

    private string _animationRunWithGun = "RunGun";
    private string _animationIdle = "Idle";
    private string _currentAnimation;

    private float _speedConversionFactor = 0.70709f;
    private float _verticalConstraint;
    private float _currentSpeed;

    private KeyCode _moveUp = KeyCode.W;
    private KeyCode _moveDown = KeyCode.S;
    private KeyCode _moveRight = KeyCode.D;
    private KeyCode _moveLeft = KeyCode.A;

    private void Start()
    {
        _player = GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _setMoveCommandWork = StartCoroutine(SetMoveCommand());
    }

    private void Update()
    {
        _moveWork = StartCoroutine(Move());

        if (_currentSpeed <= 0.1f)
        {
            _currentSpeed = 0;
            _currentAnimation = _animationIdle;
        }

        _currentSpeed = Mathf.Lerp(_currentSpeed , 0, _slowdown * Time.deltaTime);

        StopCoroutine(_moveWork);
    }

    private IEnumerator SetMoveCommand()
    {
        while (true)
        {
            if (Input.GetKey(_moveRight) && Input.GetKey(_moveUp))
            {
                _currentSpeed = _maxSpeed;
                _currentAnimation = _animationRunWithGun;
                _player.TurnRight();
                _currentDirection = (Vector2.up + Vector2.right) * _speedConversionFactor;                
            }
            else if (Input.GetKey(_moveLeft) && Input.GetKey(_moveUp))
            {
                _currentSpeed = _maxSpeed;
                _currentAnimation = _animationRunWithGun;
                _player.TurnLeft();
                _currentDirection = (Vector2.up + Vector2.left) * _speedConversionFactor;               
            }
            else if (Input.GetKey(_moveLeft) && Input.GetKey(_moveDown))
            {
                _currentSpeed = _maxSpeed;
                _currentAnimation = _animationRunWithGun;
                _player.TurnLeft();
                _currentDirection = (Vector2.down + Vector2.left) * _speedConversionFactor;                
            }
            else if (Input.GetKey(_moveDown) && Input.GetKey(_moveRight))
            {
                _currentSpeed = _maxSpeed;
                _currentAnimation = _animationRunWithGun;
                _player.TurnRight();
                _currentDirection = (Vector2.down + Vector2.right) * _speedConversionFactor;                
            }
            else if (Input.GetKey(_moveRight))
            {
                _currentSpeed = _maxSpeed;
                _currentAnimation = _animationRunWithGun;
                _player.TurnRight();
                _currentDirection = Vector2.right;
            }
            else if (Input.GetKey(_moveLeft))
            {
                _currentSpeed = _maxSpeed;
                _currentAnimation = _animationRunWithGun;
                _player.TurnLeft();
                _currentDirection = Vector2.left;
            }
            else if (Input.GetKey(_moveUp))
            {
                _currentSpeed = _maxSpeed;
                _currentAnimation = _animationRunWithGun;
                _currentDirection = Vector2.up;
            }
            else if (Input.GetKey(_moveDown))
            {
                _currentSpeed = _maxSpeed;
                _currentAnimation = _animationRunWithGun;
                _currentDirection = Vector2.down;
            }

            yield return null;
        }
    }

    private IEnumerator Move()
    {
        while (true)
        {
            _animator.Play(_currentAnimation);

            if (_player.transform.position.y > _upLimit.position.y)
            {
                _player.SetPosition(_player.transform.position.x, _upLimit.position.y);
            }
            else if (_player.transform.position.y < _downLimit.position.y)
            {
                _player.SetPosition(_player.transform.position.x, _downLimit.position.y);
            }
            else if (_player.transform.position.x < _leftLimit.position.x)
            {
                _player.SetPosition(_leftLimit.position.x, transform.position.y);
            }
            else if (_player.transform.position.x > _rightLimit.position.x)
            {
                _player.SetPosition(_rightLimit.position.x, transform.position.y);
            }

            transform.Translate(_currentDirection * _currentSpeed * Time.deltaTime, Space.World);

            yield return null;
        }
    }    
}
