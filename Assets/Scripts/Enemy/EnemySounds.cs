using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    [SerializeField] private AudioSource _attack;
    [SerializeField] private AudioSource _takeDamage;
    [SerializeField] private AudioSource _die;

    public void PlayAttackSound()
    {
        _attack.Play();
    }

    public void PlayTakeHit()
    {
        _takeDamage.Play();
    }

    public void PlayDieSound()
    {
        _die.Play();
    }
}
