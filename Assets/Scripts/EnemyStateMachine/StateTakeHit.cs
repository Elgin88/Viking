using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class StateTakeHit : State
{
    private Animator _animator;
    private string _takeHit = "Hit";

    public override IEnumerator RunState()
    {
        _animator = GetComponent<Animator>();

        while (true)
        {
            _animator.Play(_takeHit);            
            yield return null;            
        }
    }
}