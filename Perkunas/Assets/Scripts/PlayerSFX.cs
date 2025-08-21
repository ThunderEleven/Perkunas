using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip attackClip;
    public AudioClip hitClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // 공격 사운드
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackClip);
    }

    // 피격 사운드
    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitClip);
    }
}
