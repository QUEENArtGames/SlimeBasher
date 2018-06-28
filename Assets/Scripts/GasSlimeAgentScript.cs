using UnityEngine;
using UnityEngine.AI;

public class GasSlimeAgentScript : MonoBehaviour
{

    public Transform target;

    private NavMeshAgent agent;

    // Use this for initialization
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
    }

    // Update is called once per frame
    private void Update()
    {

    }
}
