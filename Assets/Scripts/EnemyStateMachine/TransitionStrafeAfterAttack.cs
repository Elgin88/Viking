using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]

public class TransitionStrafeAfterAttack : Transition
{
    [SerializeField] private float _maxDistanditonForAttack;
    [SerializeField] private float _delayTransition;

    private float _timeAfterStartTransition;
    private Enemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    public override IEnumerator CheckTransition()
    {
        while (true)
        {
            if (Target == null)
            {
                yield return null;
            }

            if (_maxDistanditonForAttack < Vector2.Distance(transform.position, Target.transform.position))
            {
                _timeAfterStartTransition += Time.deltaTime;

                if (_timeAfterStartTransition > _delayTransition)
                {
                    NeedTransit = true;
                    _timeAfterStartTransition = 0;
                }                
            }

            yield return null;
        }
    }
}
