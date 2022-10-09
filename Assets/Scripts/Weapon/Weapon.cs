using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _delayBetweenAttacks;

    public int Damage => _damage;
    public float DelayBetweenAttacks => _delayBetweenAttacks;

    public abstract void Shoot(Transform shootPoint, Player player);
    
}
