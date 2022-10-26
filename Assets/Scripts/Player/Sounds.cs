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

    public void PlayRun()
    {
        _run.Play();
    }

    public void PlayAttackGun()
    {
        _attackGun.Play();
    }

    public void PlayAttackAxe()
    {
        _attackAxe.Play();
    }

    public void PlayReloadGun()
    {
        _reloadGun.Play();
    }

    public void ApplyDamage()
    {
        _applyDamage.Play();
    }

    public void PlayDie()
    {
        _die.Play();
    }
}
