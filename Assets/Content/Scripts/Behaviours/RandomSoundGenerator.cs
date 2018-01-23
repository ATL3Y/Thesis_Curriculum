using UnityEngine;
using System.Collections;

public class RandomSoundGenerator : MonoBehaviour
{
    public float waitAtLeast = 2.0f;
    public float waitAtMost = 10.0f;

    AudioSource audioSource;
    public AudioClip[] sounds;

    private float wait;
    private bool check;

    
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        check = true;
    }

    void Update()
    {
        if (check)
        {
            wait -= Time.deltaTime; //reverse count
        }
        else
        {
            wait -= Time.deltaTime; //reverse count
        }


        if (wait < 0.0f && check == true)
        { 

            check = false;

            //set wait to be a random length
            wait = Random.Range(waitAtLeast, waitAtMost);

            Debug.Log("Delay for " + wait + "seconds.");

        }

        if (wait < 0.0f && check == false)
        {
            // select and play a sound
            audioSource.clip = sounds[Random.Range(0, sounds.Length)];
            audioSource.pitch = 1.0f;
            audioSource.GetComponent<AudioSource>().Play();

            //set wait to be the sound's length
            wait = sounds[Random.Range(0, sounds.Length)].length; 
            check = true;

            Debug.Log("Play " + audioSource.clip + "for " + wait + "seconds.");

        }
    }

    void OnTriggerEnter(Collider other)
    {

    }
}
