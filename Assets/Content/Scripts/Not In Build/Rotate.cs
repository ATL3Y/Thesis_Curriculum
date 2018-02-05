using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour
{

    public bool rotateAroundX = false;
    public bool rotateAroundY = false;
    public bool rotateAroundZ = false;
    public int rotationSpeed = 1;

    // Update is called once per frame
    void Update()
    {
        if (rotateAroundX == true)
        {
            transform.Rotate(new Vector3(1, 0, 0) * (rotationSpeed * Time.deltaTime));
        }

        if (rotateAroundY == true)
        {
            transform.Rotate(new Vector3(0, 1, 0) * (rotationSpeed * Time.deltaTime));
        }

        if (rotateAroundZ == true)
        {
            transform.Rotate(new Vector3(0, 0, 1) * (rotationSpeed * Time.deltaTime));
        }

	}
}
