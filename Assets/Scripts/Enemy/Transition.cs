using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Transition : MonoBehaviour
{
    [SerializeField] private List<State> _nextStates;

    private State _nextState;

    public State NextState => _nextState;
}
