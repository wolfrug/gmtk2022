using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCType {
    NONE = 0000,
    HOSTILE = 1000,
}
public class NPC : MonoBehaviour {
    public BasicAgent targetAgent;
    public Attack npcattack;
    public NPCType type;
    public int hitPoints = 1;
    public Transform currentFollowTarget;
    public float followDistance = 20f;
    public bool randomWander = true;
    public float randomWanderDistance = 5f;
    public Transform randomWanderCenter;
    public bool IsFollowing = false;
    public Vector2 randomWanderWaitTime = new Vector2 (0f, 5f);
    public bool followPlayer = true;

    public bool active = true;
    private float randomWanderWaitTimeLeft = 0f;
    //private BasicAgent player;

    // Start is called before the first frame update
    void Start () {
        if (followPlayer) {
            SetFollowTarget (GameManager.instance.Player.transform);
        }
    }

    public void SetFollowTarget (Transform target) {
        currentFollowTarget = target;
    }
    public void SetWanderCenter (Transform center) {
        randomWanderCenter = center;
    }

    public void SetForgetPlayer (float time) { // forgets the player (or any follow target) for x seconds
        SetFollowTarget (null);
        if (followPlayer) {
            CancelInvoke ("FollowPlayer");
            followPlayer = false;
            Invoke ("FollowPlayer", time);
        }
    }

    public void StopWalk (bool stop) {
        if (stop) {
            targetAgent.navMeshAgent.enabled = false;
            active = false;
        } else {
            targetAgent.navMeshAgent.enabled = true;
            active = true;
        }
    }
    public void LookAt (Transform target) {
        if (target.position.z > transform.position.z) {
            // Look up

        }
    }
    void FollowPlayer () {
        followPlayer = true;
        SetFollowTarget (GameManager.instance.Player.transform);
    }

    public void Damage (int amount) {
        hitPoints -= amount;
        if (hitPoints <= 0) {
            targetAgent.Kill ();
            active = false;
        }
    }

    public void NPCAttack () {
        // If they're hostile
        if (type == NPCType.HOSTILE) {
            if (npcattack != null) {
                if (Vector3.Distance (currentFollowTarget.position, transform.position) < npcattack.m_attackRange) {
                    npcattack.AttackForward ();
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (active) {
            if (randomWander && (currentFollowTarget == null || Vector3.Distance (currentFollowTarget.position, transform.position) > followDistance)) {
                if (randomWanderWaitTimeLeft <= 0f) {
                    if (randomWanderCenter == null) {
                        randomWanderCenter = transform;
                    }
                    Vector3 randomPos = ((Vector3) Random.insideUnitCircle * randomWanderDistance) + (Vector3) randomWanderCenter.position;
                    targetAgent.navMeshAgent.SetDestination (randomPos);
                    IsFollowing = false;
                    randomWanderWaitTimeLeft = Random.Range (randomWanderWaitTime.x, randomWanderWaitTime.y);
                } else {
                    randomWanderWaitTimeLeft -= Time.deltaTime;
                }
            } else if (currentFollowTarget != null && Vector3.Distance (currentFollowTarget.position, transform.position) <= followDistance) {
                targetAgent.navMeshAgent.SetDestination (currentFollowTarget.position);
                IsFollowing = true;
                NPCAttack ();
            }
        }
    }
}