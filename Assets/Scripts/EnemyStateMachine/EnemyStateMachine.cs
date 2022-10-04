using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    [SerializeField] private State _startState;

    private Transform _startPoint;
    private Player _target;
    private Coroutine _changeStateWork;
    private State _currentState;

    private void Start()
    {
        _target = GetComponent<Enemy>().Player;
        _startPoint = GetComponent<Enemy>().StartPoint;
        SetState(_startState);
        _changeStateWork = StartCoroutine(ChangeState());        
    }

    private IEnumerator ChangeState()
    {
        while (true)
        {
            if (_currentState == null)
            {
                yield return null;
            }            

            State nextState = _currentState.TryGetNextState();

            if (nextState != null)
            {
                ChangeToNextState(nextState);
            }

            yield return null;
        }
    }

    private void SetState(State state)
    {
        _currentState = _startState;

        if (_currentState != null)
        {
            _currentState.Enter(_target, _startPoint);
        }
    }

    private void ChangeToNextState(State nextState)
    {
        if (_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = nextState;

        if (_currentState != null)
        {
            _currentState.Enter(_target, _startPoint);
        }
    }
}
