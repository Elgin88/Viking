using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDurationHit : Transition
{
    [SerializeField] private float _delay;

    private WaitForSeconds _delayWork;

    public override IEnumerator CheckTransition()
    {
        _delayWork = new WaitForSeconds(_delay);

        bool isEndDelay = false;

        while (true)
        {
            if (isEndDelay == false)
            {
                isEndDelay = true;
                yield return _delayWork;
            }
            else if (isEndDelay)
            {
                NeedTransit = true;
            }

            yield return null;
        }
    }
}
