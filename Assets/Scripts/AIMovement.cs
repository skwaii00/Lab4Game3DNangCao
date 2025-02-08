using UnityEngine;
using UnityEngine.AI;

public class AIMovement : MonoBehaviour
{
    public enum State { Idle, Patrol, Chase }
    public State currentState = State.Idle;

    public Transform target;
    public float detectionRange = 5f;

    private Navigation navigation;

    void Start()
    {
        navigation = GetComponent<Navigation>();
    }

    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        switch (currentState)
        {
            case State.Idle:
                if (distanceToTarget < detectionRange)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Patrol:
                if (distanceToTarget < detectionRange)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
                navigation.agent.SetDestination(target.position);
                if (distanceToTarget > detectionRange)
                {
                    currentState = State.Patrol;
                    navigation.agent.SetDestination(navigation.waypoints[navigation.currentWaypointIndex].position);
                }
                break;
        }
    }
}
