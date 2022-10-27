using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateAttack : State
{
    [SerializeField] private int _damage;
    [SerializeField] private float _durationAttack;
    [SerializeField] private int _maxNumberAttack;

    private EnemySounds _enemySounds;
    private Animator _animator;
    private string _attack = "Attack";
    private bool _isAttack = false;

    public bool IsAttack => _isAttack;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemySounds = GetComponent<EnemySounds>();
    }

    public override IEnumerator RunState()
    {
        _animator.Play(_attack);
        _enemySounds.PlayAttackSound();

        Target.ApplyDamage(_damage);
        yield return null;      
    }
}
