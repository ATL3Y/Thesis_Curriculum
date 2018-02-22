using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPull : MonoBehaviour
{
    private List<CollisionTestSphere> attractees;

    // dropped in from prefab in inspector
    public VRNodeMinion leftHand;
    public VRNodeMinion rightHand;

    private bool IsPulling = false;

    public float speed;
    public float followDistThreshold;
    public float fleeDistThreshold;

    [SerializeField] Material followCol;
    [SerializeField] Material fleeCol;
    [SerializeField] Material baseCol;

    private float CalculateDistance ( Vector3 posA, Vector3 posB )
    {
        float deltaX = posA.x - posB.x;
        float deltaY = posA.y - posB.y;
        float deltaZ = posA.z - posB.z;
        float d = Mathf.Sqrt( Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaY, 2) + Mathf.Pow(deltaZ, 2) );
        return d;
    }

    // Use this for initialization
    void Start ()
    {
        attractees = gameObject.GetComponent<SpawnPrefab>().colTestSpheres;
    }

    // Update is called once per frame
    void Update()
    {
        for ( int i = 0; i < attractees.Count; i++ )
        {
            float followDist = CalculateDistance( attractees[i].transform.position, leftHand.transform.position );
            float fleeDist = CalculateDistance( attractees[i].transform.position, rightHand.transform.position );

            if ( leftHand.Trigger > 0.8f )
            {
                if ( followDist < followDistThreshold )
                {
                    
                    Vector3 currentPos = attractees [ i ].transform.position;
                    Vector3 targetDir = leftHand.transform.position - currentPos;
                    targetDir = targetDir.normalized;
                    Vector3 targetPos = currentPos + targetDir;
                    attractees [ i ].transform.position = Vector3.Lerp ( currentPos, targetPos, Time.deltaTime * speed );

                    attractees [ i ].GetComponent<Renderer> ( ).material = followCol;
                    
                }
            }
            else if ( rightHand.Trigger > 0.8f )
            {
                if ( fleeDist < fleeDistThreshold )
                {
                    // BREAK BY omitting some of this
                    Vector3 currentPos = attractees [ i ].transform.position;
                    Vector3 targetDir = currentPos - rightHand.transform.position; // BREAK BY flipping this line
                    targetDir = targetDir.normalized;
                    Vector3 targetPos = currentPos + targetDir;
                    attractees [ i ].transform.position = Vector3.Lerp ( currentPos, targetPos, Time.deltaTime * speed );

                    attractees [ i ].GetComponent<Renderer> ( ).material = fleeCol;
                }
            }
            else
            {
                attractees [ i ].GetComponent<Renderer> ( ).material = baseCol;
            }
        }
    }
}
