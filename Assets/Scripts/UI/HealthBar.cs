using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private Slider _slider;
    [SerializeField] private float _speedOfChange;

    private Coroutine _valueChangeWork;

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
        if (_valueChangeWork != null)        
            StopCoroutine(_valueChangeWork);        

        _valueChangeWork = StartCoroutine(ChangeHealth(currentHealth, maxHealth));
    }

    private IEnumerator ChangeHealth(int currentHealth, int maxHealth)
    {
        while (true)
        {
            _slider.value = Mathf.MoveTowards(_slider.value, (float) currentHealth/maxHealth, _speedOfChange * Time.deltaTime);
            yield return null;
        }
    }
}
