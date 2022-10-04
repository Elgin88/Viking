using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDistanceStrafe : Transition
{
    [SerializeField] private float _distanceRange;
    [SerializeField] private float _rangeSpred;

    private void Start()
    {
        _distanceRange += Random.Range(-_rangeSpred, _rangeSpred); ;
    }

    public override IEnumerator CheckTransition()
    {
        while (true)
        {
            if (_distanceRange < Vector2.Distance(transform.position, Target.transform.position))
            {
                NeedTransit = true;
            }

            yield return null;
        }
    }
}
