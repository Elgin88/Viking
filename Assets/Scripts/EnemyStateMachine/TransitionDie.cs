using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]

public class TransitionDie : Transition
{
    private Enemy _enemy;

    public override IEnumerator CheckTransition()
    {
        _enemy = GetComponent<Enemy>();

        while (true)
        {
            if (_enemy.CurrentHealth <= 0)
            {
                NeedTransit = true;
            }

            yield return null;
        }
    }
}
