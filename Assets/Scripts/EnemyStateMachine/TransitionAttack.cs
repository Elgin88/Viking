using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionAttack : Transition
{
    private StateAttack _stateAttack;
    private float _timeAfterAttack;

    public override IEnumerator CheckTransition()
    {
        yield return null;
    }
}
