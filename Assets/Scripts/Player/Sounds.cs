using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Player))]

public class Sounds : MonoBehaviour
{
    [SerializeField] private AudioSource _run;
    [SerializeField] private AudioSource _attackGun;
    [SerializeField] private AudioSource _attackAxe;
    [SerializeField] private AudioSource _reloadGun;
    [SerializeField] private AudioSource _applyDamage;
    [SerializeField] private AudioSource _die;

    private AudioSource _audioSourse;

    public void Run()
    {
        _run.Play();
    }

    public void AttackGun()
    {
        _attackGun.Play();
    }

    public void AttackAxe()
    {
        _attackAxe.Play();
    }

    public void ReloadGun()
    {
        _reloadGun.Play();
    }

    public void ApplyDamage()
    {
        _applyDamage.Play();
    }

    public void Die()
    {
        _die.Play();
    }
}
