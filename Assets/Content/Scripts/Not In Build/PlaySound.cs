using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class PlaySound : MonoBehaviour
{
    public AudioClip soundToPlay;
    
    void OnTriggerEnter(Collider other)
    {
        //Play a sound
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(soundToPlay);
    }
}
