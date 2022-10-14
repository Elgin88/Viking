using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Weapon : MonoBehaviour

{
    [SerializeField] private string _label;
    [SerializeField] private int _damage;
    [SerializeField] private float _durationAttack;
    [SerializeField] private float _duretionReload;

    public string  Label => _label;
    public int Damage => _damage;
    public float DelayBetweenAttacks => _durationAttack;
    public float DuretionReload => _duretionReload;


    public abstract void Attack(Transform shootPoint, Player player);

}
