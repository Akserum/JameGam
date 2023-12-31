using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementNPC : MonoBehaviour
{
    public NavMeshAgent agent;

    [SerializeField]
    private Transform destination = null;

    [SerializeField]
    private List<Transform> destinations;

    [SerializeField]
    private float timerStandingStill = 4f;

    //anims
    private Animator _animator;

    //+ ajouter liste d'animations
    void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        GameObject[] destGo = GameObject.FindGameObjectsWithTag("Destination");
        for (int i = 0; i < destGo.Length; i++)
        {
            destinations.Add(destGo[i].transform);
        }
        RandomizeDestination();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    private IEnumerator waiter()
    {
        RandomizeDestination();
        yield return new WaitForSeconds(timerStandingStill);
        agent.isStopped = false;
    }
    private void RandomizeDestination()
    {
        Transform tmpDest = null;
        while (tmpDest == destination || tmpDest == null)
        {
            tmpDest = destinations[Random.Range(0, destinations.Count)];
        }
        destination = tmpDest;
        agent.destination = destination.position;
        print(destination.transform.name);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.position == destination.transform.position)
        {
            agent.isStopped = true;
            print("arriv�");
            StartCoroutine(waiter());
        }
    }

    private void UpdateAnimations()
    {
        //_animator.SetBool("NpcMoving", !agent.isStopped);
    }
}