using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMZone : MonoBehaviour
{
    public AudioClip musicClip;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.ChangeBackGroundMusic(musicClip);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SoundManager.Instance.StopBackGroundMusic();
        }
    }
}
