using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDistanceOverrun : Transition
{
    [SerializeField] private float _distanceOverrun;
    [SerializeField] private float _overrunSpred;

    public override IEnumerator CheckTransition()
    {
        _distanceOverrun += Random.Range(-_overrunSpred, _overrunSpred);

        while (true)
        {
            if (Vector2.Distance(transform.position, StartPoint.position) > _distanceOverrun)
            {
                NeedTransit = true;
            }

            yield return null;
        }
    }
}
