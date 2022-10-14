using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]

public class TransitionTakeHit : Transition
{
    private Enemy _enemy;

    public override IEnumerator CheckTransition()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.IsAttacked += OnEnemyAttacked;

        yield return null;
    }

    private void OnEnemyAttacked()
    {
        NeedTransit = true;
        _enemy.IsAttacked += OnEnemyAttacked;
    }
}
