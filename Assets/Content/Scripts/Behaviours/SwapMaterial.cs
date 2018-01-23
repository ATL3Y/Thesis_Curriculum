using UnityEngine;
using System.Collections;

public class SwapMaterial : MonoBehaviour
{
    public Material materialA;
    public Material materialB;

    void OnTriggerEnter(Collider other)
    {
        //Detect if the material of the script parent is A or B. 

        if (materialA == GetComponent<Renderer>().sharedMaterial)
        {
            //Debug.Log("Material A");
            GetComponent<Renderer>().material = materialB;
        }
        else if (materialB == GetComponent<Renderer>().sharedMaterial)
        {
            //Debug.Log("Material B");
            GetComponent<Renderer>().material = materialA;
        }
    }
}
