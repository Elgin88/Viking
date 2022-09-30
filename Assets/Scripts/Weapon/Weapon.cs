using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private float _delayBetweenBullets;

    public Bullet Bullet => _bullet;

    public float DelayBetweenBullets => _delayBetweenBullets;

    public abstract void Shoot(Transform shootPoint, Player player);
    
}
