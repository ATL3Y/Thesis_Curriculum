using UnityEngine;
using System.Collections;

public class SwapMaterialRandom : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        int randColor = Mathf.FloorToInt(Random.Range(0.0f, 6.0f));

        switch (randColor)
        {
            case 0:
                GetComponent<Renderer>().material.color = Color.red;
                Debug.Log("red");
                break;

            case 1:
                GetComponent<Renderer>().material.color = Color.black;
                Debug.Log("black");
                break;

            case 2:
                GetComponent<Renderer>().material.color = Color.yellow;
                Debug.Log("yellow");
                break;

            case 3:
                GetComponent<Renderer>().material.color = Color.green;
                Debug.Log("green");
                break;

            case 4:
                GetComponent<Renderer>().material.color = Color.blue;
                Debug.Log("blue");
                break;

            case 5:
                GetComponent<Renderer>().material.color = Color.white;
                Debug.Log("white");
                break;

            default:
                GetComponent<Renderer>().material.color = Color.gray;
                Debug.Log("gray");
                break;
        }
    }
}
