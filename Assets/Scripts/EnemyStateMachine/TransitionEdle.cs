using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEdle : Transition
{
    [SerializeField] private float _duration;

    private float _durationState;

    public override IEnumerator CheckTransition()
    {
        while (true)
        {
            _durationState += Time.deltaTime;

            if (_durationState >= _duration)
            {
                NeedTransit = true;
                _durationState = 0;
            }

            yield return null;
        }
    }
}
