using UnityEngine;
using System.Collections;

public class DialogSequence : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip[] lines;

    private float wait;
    public float pause;
    private bool check;

    int line = 0;

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

            // set wait to be a random length
            wait = pause;

           //Debug.Log("Pause for " + wait + "seconds.");

        }

        if (wait < 0.0f && check == false)
        {
            if (line < lines.Length)
            {
                // play the next line
                audioSource.clip = lines[line];
                audioSource.pitch = 1.0f;
                audioSource.GetComponent<AudioSource>().Play();

                // set wait to be the line's length
                wait = lines[line].length; 
                check = true;

                Debug.Log("Line: " + line);
                line++;

            }

            //Debug.Log("Play " + audioSource.clip + "for " + wait + "seconds.");

        }
    }
}
