using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[System.Serializable]
public class BasicAgentDie : UnityEvent<BasicAgent> { }

public enum Facing {
    None = 0000,
    Right = 1000,

    Left = 2000,

    Down = 3000,

    Up = 4000,
}

public class BasicAgent : MonoBehaviour {
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public float speed = 0f;
    public bool useKeyboardForMovement = false;

    public Facing m_currentFacing = Facing.None;

    public bool m_isDead = false;

    public BasicAgentDie m_deathEvent;

    public FMODUnity.StudioEventEmitter m_normalDeathSound;
    public FMODUnity.StudioEventEmitter m_darkDeathSound;
    private Coroutine autoAction;
    // Start is called before the first frame update
    void Start () {
        if (animator == null) {
            animator = GetComponent<Animator> ();
        }
    }

    public void StartWalk () {
        animator.SetFloat ("speedx", navMeshAgent.velocity.x);
        animator.SetFloat ("speedy", navMeshAgent.velocity.z);
        if (navMeshAgent.velocity.z > navMeshAgent.velocity.magnitude / 2f) {
            animator.SetBool ("forceup", true);
        } else {
            animator.SetBool ("forceup", false);
        }
        if (navMeshAgent.velocity.z < -(navMeshAgent.velocity.magnitude / 2f)) {
            animator.SetBool ("forcedown", true);
        } else {
            animator.SetBool ("forcedown", false);
        }
        animator.SetFloat ("speed", navMeshAgent.velocity.magnitude);
        //Debug.Log ("Navmeshagent velocity: " + navMeshAgent.velocity);
    }
    public void StopWalk () {
        animator.SetFloat ("speed", 0f);
        animator.SetFloat ("speedx", 0f);
        animator.SetFloat ("speedy", 0f);
    }

    public void ActivateAction (string animation, float duration, bool dropCarry = true) {
        navMeshAgent.isStopped = true;
        navMeshAgent.SetDestination (navMeshAgent.transform.position);
    }

    public void CancelAutoTask () {
        if (autoAction != null) {
            StopCoroutine (autoAction);
        };
    }
    public IEnumerator WalkWatching (Transform target, float activateDistance, System.Action callback) {

        Debug.Log ("Distance to target: " + Vector3.Distance (transform.position, target.position));
        while (Vector3.Distance (transform.position, target.position) > activateDistance) {
            Debug.Log ("Waiting to reach target, remaining distance: " + Vector3.Distance (transform.position, target.position));
            yield return new WaitForSeconds (0.1f);
            if (!navMeshAgent.hasPath) {
                Debug.Log ("Navmesh agent has no path, cancelling");
                break;
            }
        }
        //       Debug.Log ("Reached destination or cancelled");
        if (Vector3.Distance (target.position, transform.position) <= activateDistance) {
            //           Debug.Log ("Attempting pick-up!");
            callback.Invoke ();
        } else {
            Debug.Log ("Failed to invoke carry walk due to distance (" + Vector3.Distance (target.position, transform.position) + ")");
        }
        autoAction = null;
    }

    public void Interact (bool interact) {
        if (interact) {
            CancelAutoTask ();
            navMeshAgent.SetDestination (navMeshAgent.transform.position);
        }
    }

    public void Kill () {
        StopWalk ();
        animator.SetBool ("dead", true);
        animator.SetTrigger ("die");
        navMeshAgent.enabled = false;
        m_isDead = true;
        m_deathEvent.Invoke (this);
        if (GameManager.instance.m_gameIsNormal) {
            if (m_normalDeathSound != null) {
                m_normalDeathSound.Play ();
            };
        } else {
            if (m_darkDeathSound != null) {
                m_darkDeathSound.Play ();
            };
        }
    }
    public void Resurrect () {
        animator.SetBool ("dead", false);
        animator.SetTrigger ("resurrect");
        navMeshAgent.enabled = true;
        m_isDead = false;
    }

    // Update is called once per frame
    void Update () {
        if (navMeshAgent.velocity.magnitude > 0.1f) {
            StartWalk ();
        } else {
            StopWalk ();
        }
        if (useKeyboardForMovement && GameManager.instance.GameState == GameStates.GAME) {
            float horInput = Input.GetAxis ("Horizontal");
            float verInput = Input.GetAxis ("Vertical");
            if (horInput != 0f || verInput != 0f) {
                Vector3 movement = new Vector3 (horInput, 0f, verInput) * navMeshAgent.speed;
                navMeshAgent.velocity = movement;
                // Vector3 moveDestination = transform.position + movement;
                // navMeshAgent.destination = moveDestination;
            };
        }
        // Set the facing...sort of 
        if (navMeshAgent.velocity.magnitude > 0.1f) {
            if (navMeshAgent.velocity.x > 0f) {
                m_currentFacing = Facing.Right;
            } else {
                m_currentFacing = Facing.Left;
            }
            /* if (navMeshAgent.velocity.z > navMeshAgent.velocity.magnitude / 2f) {
                 m_currentFacing = Facing.Up;
             }
             if (navMeshAgent.velocity.z < -(navMeshAgent.velocity.magnitude / 2f)) {
                 m_currentFacing = Facing.Down;
             }
             */ // cannot attack up or down lol
        }
    }
}