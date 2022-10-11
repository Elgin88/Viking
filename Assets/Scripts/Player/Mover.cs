using System;
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

    private Coroutine _restrictMoveWork;
    private Coroutine _playAnimationWork;
    private Coroutine _mover;   

    private Animator _animator;

    private Player _player;

    private string _runWithGun = "RunGun";
    private string _runWithAxe = "RunAxe";
    private string _idleGun = "IdleGun";
    private string _idleAxe = "IdleAxe";
    private string _currentAnimation;
    private string _currentIdle;
    private string _currentRun;

    private float _speedConversionFactor = 0.70709f;
    private float _verticalConstraint;
    private float _currentSpeed;

    private KeyCode _moveUp = KeyCode.W;
    private KeyCode _moveDown = KeyCode.S;
    private KeyCode _moveRight = KeyCode.D;
    private KeyCode _moveLeft = KeyCode.A;    

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GetComponent<Player>();

        _mover = StartCoroutine(Move());
        _restrictMoveWork = StartCoroutine(RestrictMove());
    }

    public void StartCoroutineMover()
    {
        _mover = StartCoroutine(Move());
    }

    public void StopCoroutineMove()
    {
        StopCoroutine(_mover);
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
            _currentSpeed = Mathf.Lerp(_currentSpeed, 0, _slowdown * Time.deltaTime);
            _animator.Play(_currentAnimation);

            yield return null;
        }
    }

    private IEnumerator RestrictMove()
    {
        while (true)
        {
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

            yield return null;
        }
    }

    public void ChangeSkinsToGun()
    {
        _currentIdle = _idleGun;
        _currentRun = _runWithGun;
    }

    public void ChangeSkinsToAxe()
    {
        _currentIdle = _idleAxe;
        _currentRun = _runWithAxe;
    }
}