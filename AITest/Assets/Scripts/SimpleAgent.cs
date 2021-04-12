using UnityEngine;
using UnityEngine.AI;

public class SimpleAgent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject target;
    private float minDist = 4f;

    private void Start()
    {
        agent.SetDestination(target.transform.position);
    }

    private void Update()
    {
        if(Vector3.Distance(target.transform.position, transform.position) < minDist)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
        }
    }
}