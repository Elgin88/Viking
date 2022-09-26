using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    private int _curretnHealth;

    private void Start()
    {
        _curretnHealth = _maxHealth;
    }

    public void ApplyDamage(int damage)
    {
        _curretnHealth -= damage;
        _curretnHealth = Mathf.Clamp(_curretnHealth, 0, _maxHealth);
    }

    public void ApplyHeal(int heal)
    {
        _curretnHealth += heal;
        _curretnHealth = Mathf.Clamp(_curretnHealth, 0, _maxHealth);
    }
}