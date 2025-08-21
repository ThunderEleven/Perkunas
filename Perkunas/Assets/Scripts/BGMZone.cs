using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMZone : MonoBehaviour
{
    public AudioSource audioSource;
    public float fadeTime;
    public float maxVolume;
    [SerializeField] private float targetVolume;
    private bool isPlaying = false; // 플레이어가 들어왔을때 재생시키자

    void Start()
    {
        targetVolume = 0;
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = targetVolume;
        audioSource.Play();

    }

    void Update()
    {
        if (!Mathf.Approximately(audioSource.volume, targetVolume))
        {
            audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, (maxVolume / fadeTime) * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = maxVolume;

            if (!isPlaying)
            {
                audioSource.Play();
                isPlaying = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = 0;
        }
    }
}
