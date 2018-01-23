using UnityEngine;
using System.Collections;

public class SwapOtherMaterial : MonoBehaviour
{
    public GameObject otherObject;
    public Material materialA;
    public Material materialB;

    void OnTriggerEnter(Collider other)
    {
        //Detect if the material of the script parent is A or B. 

        if (materialA == otherObject.GetComponent<Renderer>().sharedMaterial)
        {
            //Debug.Log("Material A");
            otherObject.GetComponent<Renderer>().material = materialB;
        }
        else if (materialB == otherObject.GetComponent<Renderer>().sharedMaterial)
        {
            //Debug.Log("Material B");
            otherObject.GetComponent<Renderer>().material = materialA;
        }
    }
}
