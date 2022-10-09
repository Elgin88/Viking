using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    [SerializeField] private Bullet _bullet;  
    
    public override void Shoot(Transform shootPoint, Player player)
    {
        Bullet bullet = Instantiate(_bullet, shootPoint.transform.position, Quaternion.identity);
        bullet.Init(player);
        bullet.SetDirection(player.IsTurnRight);
    }
}
