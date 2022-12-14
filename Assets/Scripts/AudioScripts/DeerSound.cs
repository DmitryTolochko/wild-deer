using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerSound : MonoBehaviour
{
    private AudioSource audioSource;
    private Deer deer;
    private bool IsOn;

    private void Start()
    {
        audioSource = GameObject.Find("FallSoundSystem").GetComponent<AudioSource>();
        deer = GetComponent<Deer>();
    }

    private void Update() 
    {
        if (!deer.IsSpawned)
            IsOn = true;
        if (IsOn && deer.DistanceToTarget <= 0.65f && !audioSource.isPlaying)
        {
            audioSource.Play();
            IsOn = false;
        }
    }
}
