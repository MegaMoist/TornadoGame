using UnityEngine;
using System.Collections;
[System.Serializable]

public class Wander : MonoBehaviour
{
    private Vector3 wayPoint;
    private NavMeshAgent agent;
    private int walkableMask;

    // Use this for initialization
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        walkableMask = NavMesh.GetAreaFromName("Walkable");
        wayPoint = transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        if ((transform.position - wayPoint).magnitude <= 2)
        {
            if (RandomPoint(transform.position, Random.Range(10.0f,20.0f), out wayPoint))
            {
                Debug.DrawRay(wayPoint, Vector3.up, Color.blue, 1.0f);
                agent.SetDestination(wayPoint);
            }

        }

    }


    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint =center +  ( Random.onUnitSphere * range);
            randomPoint.y = 1;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

}
