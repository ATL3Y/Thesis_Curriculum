using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPull : MonoBehaviour
{
    private List<GameObject> attractees;

    // dropped in from prefab in inspector
    public VRNodeMinion leftHand;
    public VRNodeMinion rightHand;
    
    public bool GlovesOn { get; set; }

    private bool IsPulling = false;

    public float baseSpeed = 1.0f;

    // Use this for initialization
    void Start ()
    {
        attractees = gameObject.GetComponent<SpawnPrefab>().prefabs;
    }

    // Update is called once per frame
    void Update()
    {
        if (leftHand.Trigger > 0.8f)
        {
            StartCoroutine(follow());
        }

        if (leftHand.Bumper > 0.8f)
        {
            StartCoroutine(flee());
        }

    }



    IEnumerator follow()
    {
        for (int i = 0; i < attractees.Count; i++)
        {
            float currentDistance = Vector3.Distance(attractees[i].transform.position, leftHand.transform.position);
            float speed = Mathf.Lerp(0.1f, 2.0f, currentDistance);

            Pull(attractees[i], speed);
        }
        yield return null;
    }

    void Pull(GameObject pullee, float speed)
    {
        pullee.transform.position = Vector3.Lerp(pullee.transform.position, leftHand.transform.position, Time.deltaTime * speed);
    }



    IEnumerator flee()
    {
        for (int i = 0; i < attractees.Count; i++)
        {
            //
        }
        yield return null;
    }

    void Push(GameObject pushee, float speed)
    {
        //
    }

}
