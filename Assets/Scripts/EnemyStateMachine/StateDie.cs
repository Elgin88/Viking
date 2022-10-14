using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class StateDie : State
{
    [SerializeField] private float _duration;

    private CapsuleCollider2D _collider;
    private Animator _animator;
    private Enemy _enemy;

    private string _animationDie = "Die";

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
        _collider = GetComponent<CapsuleCollider2D>();
    }

    public override IEnumerator RunState()
    {
        float duration = 0;

        if (_enemy == null)
            yield return null;        

        Target.IncreaaseNumberKills();
        _enemy.StopSetDirection();

        while (true)
        {
            _animator.Play(_animationDie);
            _collider.enabled = false;

            duration += Time.deltaTime;

            if (duration > _duration)
            {
                
                Destroy(gameObject);           
            }

            yield return null;
        }
    }
}
