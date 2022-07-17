using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavmeshGate : MonoBehaviour {
    public NavMeshObstacle m_targetGate;
    public Animator m_animator;
    public string m_openAnimTrigger = "open";
    public string m_closeAnimTrigger = "close";
    public float m_animTime = 1f;
    public bool m_open = false;
    // Start is called before the first frame update
    void Start () {

    }

    public void OpenGate () {
        m_animator.SetTrigger (m_openAnimTrigger);
        StartCoroutine (Waiter (false));
    }
    public void CloseGate () {
        m_animator.SetTrigger (m_closeAnimTrigger);
        StartCoroutine (Waiter (false));
    }
    IEnumerator Waiter (bool close) {
        yield return new WaitForSeconds (m_animTime);
        m_targetGate.enabled = close;
    }

    // Update is called once per frame
    void Update () {

    }
}