using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{
    [SerializeField] private float _distanceAttack;

    private RaycastHit2D _hit;

    public override void Attack(Transform attackPoint, Player player)
    {
        if (player.IsTurnRight == true)
        {
            _hit = Physics2D.Raycast(attackPoint.position, Vector2.right);
        }
        else if(player.IsTurnRight == false)
        {
            _hit = Physics2D.Raycast(attackPoint.position, Vector2.left);
        }

        if (_hit.collider.TryGetComponent<Enemy>(out Enemy enemy) & _hit.distance < _distanceAttack)
        {
            enemy.ApplyDamage(Damage);
        }        
    }
}
