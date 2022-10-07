using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : Bar
{
    [SerializeField] private Player _player;

    private Coroutine _changeValueBarWork;

    private void Start()
    {
        _changeValueBarWork = StartCoroutine(ChangeValueBar());
    }

    private IEnumerator ChangeValueBar()
    {
        while (true)
        {
            _player.ChangedHealth += OnValueChanged;

            yield return null;
        }
    }
}
