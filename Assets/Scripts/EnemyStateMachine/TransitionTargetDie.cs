using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionTargetDie : Transition
{
    public override IEnumerator CheckTransition()
    {
        while (true)
        {
            if (Target == null)
            {
                NeedTransit = true;
            }

            yield return null;
        }
    }
}
