using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    public Transform[] waypoints;
    public NavMeshAgent agent; 
    public int currentWaypointIndex;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0)
        {
            currentWaypointIndex = 0;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    void Update()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
}
