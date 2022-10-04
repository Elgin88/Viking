using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private State _nextStates;

    private Coroutine _checkTransitionWork;
    private Transform _startPoint;

    public Transform StartPoint => _startPoint;
    public Player Target { get; private set; }
    public State NextState => _nextStates;
    public bool NeedTransit { get; protected set; }

    public void InitTarget(Player target)
    {
        Target = target;
    }

    public void InitStartPoint(Transform starPoint)
    {
        _startPoint = starPoint;
    }

    public void StartCheckTransition()
    {
        NeedTransit = false;
        _checkTransitionWork = StartCoroutine(CheckTransition());
    }

    public void StopCheckTransition()
    {
        StopCoroutine(_checkTransitionWork);
        NeedTransit = false;
    }

    public abstract IEnumerator CheckTransition();
}
