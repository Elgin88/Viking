using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Animator))]

public class StateOverrun : State
{
    [SerializeField] private float _speed;

    private Animator _animator;
    private Coroutine _runStateWork;
    private string _walk = "Walk";

    public override IEnumerator RunState()
    {
        _animator = GetComponent<Animator>();
        _animator.Play(_walk);

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
