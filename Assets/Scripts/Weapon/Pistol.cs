using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    public override void Shoot(Transform shootPoint, Player player)
    {
        Bullet bullet = Instantiate(Bullet, shootPoint.transform.position, Quaternion.identity);
        bullet.Init(player);
        bullet.SetDirection(player.IsTurnRight);
    }
}
