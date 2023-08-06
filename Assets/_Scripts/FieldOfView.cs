using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    public bool sawPlayer;
    private float timingLose = 2f;

    private readonly static float INITIAL_TIME_NEEDED = 2f;
    private float timer = 0;

    public GameObject indicatorPlayerSeen;
    //anims 
    [SerializeField]
    //private GameObject _worldSpaceCanvas;

    private MovementNPC _movementNpc;
    //private Animator _animator;
    private float _basedRadius;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
        //set the based radius
        _basedRadius = radius;
        //anims
        //_animator = GetComponentInChildren<Animator>();
        _movementNpc = GetComponent<MovementNPC>();
        //_worldSpaceCanvas.SetActive(false);
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    SpottedPlayer();
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    public void SpottedPlayer()
    {
        if (canSeePlayer)
        {
            return;
        }
        StartCoroutine(SpottedPlayerCoroutine());

    }
    private IEnumerator SpottedPlayerCoroutine()
    {
        indicatorPlayerSeen.SetActive(true);
        _movementNpc.agent.isStopped = true;
        GameManager.Instance.RemoveTime();
        print("appel coroutine");
        yield return new WaitForSeconds(INITIAL_TIME_NEEDED);
        if (canSeePlayer)
        {
            GameManager.Instance.PlayerHasBeenSeen = true;
            GameManager.Instance.EndGame();
        }
        else
        {
            indicatorPlayerSeen.SetActive(false);
            _movementNpc.agent.isStopped = false;
        }
    }

    // fonction dans le script pour les anims events
    public void ResetNpcFov()
    {
        sawPlayer = false;
        //_animator.ResetTrigger("Spotted");
        //_worldSpaceCanvas.SetActive(false);
        radius = _basedRadius;
        _movementNpc.agent.isStopped = false;
    }
}