using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _bullet;

    public Bullet Bullet => _bullet;

    public abstract void Shoot(Transform shootPoint, Player player);
    
}
