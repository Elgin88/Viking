using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateAttack : State
{
    [SerializeField] private int _damage;

    private Animator _animator;
    private string _attack = "Attack";

    public event UnityAction IsAttack;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public override IEnumerator RunState()
    {
        _animator.Play(_attack);
        Target.ApplyDamage(_damage);
        IsAttack?.Invoke();

        yield return null;                            
    }
}
