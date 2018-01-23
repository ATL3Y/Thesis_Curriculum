using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGame : MonoBehaviour
{
    public FillCircuit circuit;
    public bool fill = false;
    public float fillAmt = 0.0f;
    public bool flicker = false;

    // Use this for initialization
    void Start ( )
    {

    }

    // Update is called once per frame
    void Update ( )
    {
        if ( fill )
        {
            circuit.FillTo ( fillAmt );
            fill = false;
        }

        if ( flicker )
        {
            circuit.FlickerFor ( 10.0f );
            flicker = false;
        }
    }
}
