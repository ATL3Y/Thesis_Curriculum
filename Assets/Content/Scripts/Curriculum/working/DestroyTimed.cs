using UnityEngine;
using System.Collections;

public class DestroyTimed : MonoBehaviour
{
    public float DestructionTimer = 2.0f;

    void Start()
    {
        Destroy(gameObject, DestructionTimer);
    }
}
