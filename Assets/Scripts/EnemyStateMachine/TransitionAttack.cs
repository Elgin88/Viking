using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAttack : Transition
{
    private StateAttack _stateAttack;
    private float _timeAfterAttack;

    public override IEnumerator CheckTransition()
    {
        _stateAttack = GetComponent<StateAttack>();        

        while (true)
        {
            _stateAttack.IsAttack += OnEnemyAttack;
            yield return null;
        }
    }

    private void OnEnemyAttack()
    {
        NeedTransit = true;
        _stateAttack.IsAttack -= OnEnemyAttack;
    }
}
