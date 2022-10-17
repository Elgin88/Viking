using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private float _speedOfChangeValue;

    private Coroutine _changeHealthWork;

    private void OnEnable()
    {
        _player.ChangedHealth += OnHealthChanged;        
    }

    private void OnDisable()
    {
        _player.ChangedHealth -= OnHealthChanged;
    }

    private void OnHealthChanged(int currentHealth, int maxHealth)
    {
        if (_changeHealthWork != null)
        {
            StopCoroutine(_changeHealthWork);
        }

        _changeHealthWork = StartCoroutine(ChangeHealth(currentHealth, maxHealth));
    }

    private IEnumerator ChangeHealth(int currentHealth, int maxHealth)
    {
        while (true)
        {
            _healthBar.value = Mathf.MoveTowards(_healthBar.value, (float)currentHealth/maxHealth, _speedOfChangeValue * Time.deltaTime);

            if (_healthBar.value == currentHealth)
            {
                StopCoroutine(_changeHealthWork);
                _changeHealthWork = null;
            }

            yield return null;
        }
    }
}
