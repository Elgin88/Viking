using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : State
{
    [SerializeField] private int _damage;

    public override IEnumerator RunState()
    {
        while (true)
        {
            yield return null;
        }
    }
}
