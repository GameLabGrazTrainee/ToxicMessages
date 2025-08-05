using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyMovement : MonoBehaviour
{
    private Transform currentTarget;
    public Transform player;
    private NavMeshAgent navMeshAgent;
    public bool isFollowingPlayer = true;
    public List<Transform> target = new List<Transform>();
    private float RunAwayTimer = 0f;
    private float RunAwayInterval = 20f;

    private Animator animator;

    public AudioSource audioSource;
    public AudioResource RunningAudio;
    public AudioResource FollowingAudio;
    public AudioResource CoinPickupAudio;
 
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        animator.SetBool("IsRunning", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && isFollowingPlayer == true)
        {
            navMeshAgent.SetDestination(player.position);
        }
        else {
            navMeshAgent.SetDestination(currentTarget.position);
        }

        if (isFollowingPlayer == false)
        {
            RunAwayTimer = RunAwayTimer + Time.deltaTime;
            if (RunAwayTimer > RunAwayInterval)
            {
                RunAwayTimer = 0;
                StartChasingPlayer();
            }
        }
    }
    public void chooseTarget()
    {
        int newTargetIndex = Random.Range(0, target.Count);
        Transform NewTarget = target[newTargetIndex];
        if (NewTarget == currentTarget)
        {
            chooseTarget();
        }
        else { currentTarget = NewTarget; }
    }


    public void StopChasingPlayer() 
    {
        audioSource.resource = RunningAudio;
        audioSource.Play();
        chooseTarget();
        isFollowingPlayer = false;
        navMeshAgent.speed = 10f;
    }
    public void StartChasingPlayer()
    {
        audioSource.resource = FollowingAudio;
        audioSource.Play();

        isFollowingPlayer = true;
        navMeshAgent.speed = 3.5f;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ToxiTarget") && isFollowingPlayer == false)
        {
            chooseTarget(); 
        } 
    }

    public void Die()
    {
        navMeshAgent.speed = 0;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        animator.SetTrigger("FallDown");
        Destroy(this.gameObject, 5f);
        
        
    }


    }
