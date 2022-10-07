using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class StateCelebration : State
{
    private Animator _animator;

    private string _animationCelebration = "Celebration";

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override IEnumerator RunState()
    {
        if (_animator == null)
        {
            yield return null;
        }

        _animator.Play(_animationCelebration);

        yield return null;
    }
}
