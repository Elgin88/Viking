using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOverrun : State
{
    [SerializeField] private float _speed;

    private Coroutine _runStateWork;

    public override IEnumerator RunState()
    {
        while (true)
        {
            if (transform.position.x < Target.transform.position.x)
            {
                transform.Translate(Vector2.right * _speed * Time.deltaTime, Space.World);
            }
            else
            {
                transform.Translate(Vector2.left * _speed * Time.deltaTime, Space.World);
            }            

            yield return null;
        }
    }


}
