using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RestartAtY : MonoBehaviour
{

    public float killY = -100.0f;

    void Update()
    {
        if (transform.position.y < killY)
        {
            // Use this for stuff that you want to delete below a certain level
            // NOTE: If you destroy the player, there won't be anything to control
            SceneManager.LoadScene("02 Terrain Topography");
        }
    }
}