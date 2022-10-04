using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public abstract class State : MonoBehaviour
{
    [SerializeField] private List<Transition> _transitions;

    private Player _target;
    private Transform _startPoint;    
    private Coroutine _runStateWork;

    public Player Target => _target;

    public void Enter(Player target, Transform startPoint)
    {
        _target = target;
        _startPoint = startPoint;     

        _runStateWork = StartCoroutine(RunState());

        foreach (var transition in _transitions)
        {
            transition.InitTarget(_target);
            transition.InitStartPoint(_startPoint);

            transition.StartCheckTransition();
        }
    }

    public void Exit()
    {
        foreach (var transition in _transitions)
        {
            transition.StopCheckTransition();
        }

        StopCoroutine(_runStateWork);
    }

    public State TryGetNextState()
    {
        foreach (var transition in _transitions)
        {
            if (transition.NeedTransit)
            {
                return transition.NextState;
            }            
        }

        return null;
    }

    public abstract IEnumerator RunState();
}
