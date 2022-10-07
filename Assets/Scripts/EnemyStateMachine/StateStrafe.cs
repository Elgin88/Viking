using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateStrafe : State
{
    [SerializeField] private float _speed;

    private Enemy _enemy;
    private float _speedConversionFactor = 0.70709f;
    private Animator _animator;
    private string _walk = "Walk";

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }


    public override IEnumerator RunState()
    {
        while (true)
        {
            if (Target == null)
            {
                yield return null;
            }

            _animator.Play(_walk);

            if (Target.transform.position.x < transform.position.x & Target.transform.position.y > transform.position.y)
            {
                transform.Translate((Vector2.up + Vector2.left) * _speed * _speedConversionFactor * Time.deltaTime, Space.World);
            }
            else if (Target.transform.position.x < transform.position.x & Target.transform.position.y < transform.position.y)
            {
                transform.Translate((Vector2.down + Vector2.left) * _speed * _speedConversionFactor * Time.deltaTime, Space.World);
            }
            else if (Target.transform.position.x > transform.position.x & Target.transform.position.y < transform.position.y)
            {
                transform.Translate((Vector2.down + Vector2.right) * _speed * _speedConversionFactor * Time.deltaTime, Space.World);
            }
            else if (Target.transform.position.x > transform.position.x & Target.transform.position.y > transform.position.y)
            {
                transform.Translate((Vector2.up + Vector2.right) * _speed * _speedConversionFactor * Time.deltaTime, Space.World);
            }

            yield return null;
        }
    }
}
