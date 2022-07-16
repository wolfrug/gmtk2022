using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WarpAgent : MonoBehaviour {
    public WarpAgent endPoint;
    public Transform landPoint;
    public GenericTrigger enterTrigger;
    public bool warping = false;

    void Start () {
        if (landPoint == null) {
            landPoint = transform;
        }
        if (enterTrigger == null) {
            enterTrigger = GetComponent<GenericTrigger> ();
        }
        enterTrigger.triggerEntered.AddListener (WarpStartToEnd);
        enterTrigger.triggerExited.AddListener (ActivateTrigger);
    }

    public void WarpStartToEnd (GameObject target) {
        if (!warping) {
            NavMeshAgent agent = target.GetComponent<NavMeshAgent> ();
            if (agent != null) {
                endPoint.warping = true;
                if (!agent.Warp (endPoint.landPoint.position)) {
                    agent.transform.position = endPoint.landPoint.position;
                }
            }
        };
    }
    public void WarpEndToStart (GameObject target) {
        NavMeshAgent agent = target.GetComponent<NavMeshAgent> ();
        if (agent != null) {
            warping = true;
            if (!agent.Warp (landPoint.position)) {
                agent.transform.position = landPoint.position;
            }
        }
    }
    void ActivateTrigger (GameObject target) {
        // Re-activate trigger on exit
        warping = false;
    }

#if UNITY_EDITOR
    void OnDrawGizmos () {
        if (endPoint != null) {
            Vector3 start;
            Vector3 end = Vector3.zero;
            Color color = Color.blue;
            start = enterTrigger.transform.position;
            end = endPoint.landPoint.position;
            Gizmos.color = color;
            Gizmos.DrawLine (start, end);
        };
    }
#endif
}