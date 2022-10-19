using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Player))]
[RequireComponent(typeof(AudioSource))]

public class Sounds : MonoBehaviour
{
    [SerializeField] private AudioSource _idle;
    [SerializeField] private AudioSource _runGun;
    [SerializeField] private AudioSource _runAxe;
    [SerializeField] private AudioSource _attackGun;
    [SerializeField] private AudioSource _attackAxe;
    [SerializeField] private AudioSource _reloadGun;
    [SerializeField] private AudioSource _applyDamage;
    [SerializeField] private AudioSource _die;

    private AudioSource _audioSourse;

    public void IdlePlayAudio()
    {
        _idle.Play();
    }

    public void RunGunPlayAudio()
    {
        _runGun.Play();
    }

    public void RunAxelayAudio()
    {
        _runAxe.Play();
    }

    public void AttackGunPlayAudio()
    {
        _attackGun.Play();
    }

    public void AttackAxePlayAudio()
    {
        _attackAxe.Play();
    }

    public void ReloadGunPlayAudio()
    {
        _reloadGun.Play();
    }

    public void ApplyDamagePlayAudio()
    {
        _applyDamage.Play();
    }

    public void DiePlayAudio()
    {
        _die.Play();
    }
}
