using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    [SerializeField] private List<Transition> _transitions;

    private Player _target;
    private Coroutine _runStateWork;

    public Player Target => _target;

    public void Enter(Player target)
    {
        _target = target;
        _runStateWork = StartCoroutine(RunState());
    }

    public void Exit()
    {
        StopCoroutine(_runStateWork);
    }

    public State TryGetNextState()
    {
        foreach (var transition in _transitions)
        {
            return transition.NextState;
        }

        return null;
    }

    public abstract IEnumerator RunState();
}
