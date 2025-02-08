using UnityEngine;
using UnityEngine.AI;

public class FSM : MonoBehaviour
{
    public enum State { Patrol, Jump, SpeedBoost }
    public State currentState = State.Patrol;
    public float jumpHeight = 2f;
    public float speedBoostMultiplier = 2f;
    public float speedBoostDuration = 2f;
    public Transform[] waypoints;
    public GameOverManager gameOverManager;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool speedBoostActive = false;
    private float speedBoostEndTime;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        else
        {
            Debug.LogError("Waypoints array is empty. Please add waypoints to the array.");
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        float distanceCovered = Vector3.Distance(waypoints[0].position, transform.position);
        float totalDistance = Vector3.Distance(waypoints[0].position, waypoints[waypoints.Length - 1].position);

        if (distanceCovered >= totalDistance / 3)
        {
            if (Random.value < 0.5f)
            {
                currentState = State.Jump;
            }
            else
            {
                currentState = State.SpeedBoost;
                speedBoostEndTime = Time.time + speedBoostDuration;
                agent.speed *= speedBoostMultiplier;
                speedBoostActive = true;
            }
        }

        if (speedBoostActive && Time.time >= speedBoostEndTime)
        {
            agent.speed /= speedBoostMultiplier;
            speedBoostActive = false;
            currentState = State.Patrol;
        }

        switch (currentState)
        {
            case State.Patrol:
                Patrol();
                break;
            case State.Jump:
                Jump();
                currentState = State.Patrol;
                break;
            case State.SpeedBoost:
                Patrol();
                break;
        }
    }

    void Patrol()
    {
        if (agent.remainingDistance < agent.stoppingDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypointIndex].position);

            if (currentWaypointIndex == 0)
            {
                gameOverManager.GameOver(); 
            }
        }
    }

    void Jump()
    {
        agent.isStopped = true;
        transform.position += Vector3.up * jumpHeight;
        agent.isStopped = false;
    }
}
