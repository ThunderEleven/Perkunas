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

    // ���� ����
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(attackClip);
    }

    // �ǰ� ����
    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitClip);
    }
}
