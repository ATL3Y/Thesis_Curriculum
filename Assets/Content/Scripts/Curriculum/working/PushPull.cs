using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushPull : MonoBehaviour
{
    public float speed;
    public float followDistThreshold;
    public float fleeDistThreshold;

    private List<CollisionTestSphere> attractees;
    [SerializeField]
    PlayerCurriculum playerCurriculum;
    private bool IsPulling = false;

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
            float followDist = CalculateDistance( attractees[i].transform.position, playerCurriculum.GetLeftHand().position );
            float fleeDist = CalculateDistance( attractees[i].transform.position, playerCurriculum.GetRightHand().position );

            if ( playerCurriculum.GetLeftTriggerDown() )
            {
                if ( followDist < followDistThreshold )
                {
                    
                    Vector3 currentPos = attractees [ i ].transform.position;
                    Vector3 targetDir = playerCurriculum.GetLeftHand().position - currentPos;
                    targetDir = targetDir.normalized;
                    Vector3 targetPos = currentPos + targetDir;
                    attractees [ i ].transform.position = Vector3.Lerp ( currentPos, targetPos, Time.deltaTime * speed );

                    attractees [ i ].GetComponent<Renderer> ( ).material = followCol;
                    
                }
            }
            else if ( playerCurriculum.GetRightTriggerDown( ) )
            {
                if ( fleeDist < fleeDistThreshold )
                {
                    Vector3 currentPos = attractees [ i ].transform.position;
                    Vector3 targetDir = currentPos - playerCurriculum.GetRightHand().transform.position;
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
