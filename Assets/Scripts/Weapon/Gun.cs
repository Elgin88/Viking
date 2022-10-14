using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : Weapon
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private int _numberBullets;

    public event UnityAction<int> ChangedNumberBullets;

    public override void Attack(Transform shootPoint, Player player)
    {
        _numberBullets--;
        ChangedNumberBullets?.Invoke(_numberBullets);

        Bullet bullet = Instantiate(_bullet, shootPoint.transform.position, Quaternion.identity);
        bullet.Init(player);
        bullet.SetDirection(player.IsTurnRight);
    }
}
