using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : State
{
    private Animator _animator;
    private string _idle = "Idle";

    private void Start()
    {
        _animator = GetComponent<Animator>();        
    }

    public override IEnumerator RunState()
    {
        while (true)
        {
            _animator.Play(_idle);
        }
    }
}
