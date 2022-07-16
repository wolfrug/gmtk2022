using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DiceRolled : UnityEvent<int> { }

public class DiceRoller : MonoBehaviour {
    public GameObject m_d6;
    public Die_d6 m_script;
    public Animator m_diceAnimator;
    public Transform m_followTransform;
    public Vector3 m_originPoint = new Vector3 (0, 2, 2);
    public Vector3 m_force = new Vector3 (5, 20, 5);

    public int m_lastResult = -1;

    public DiceRolled m_diceRolledEvent;
    private Coroutine m_diceRollCoroutine = null;
    // Start is called before the first frame update
    void Start () {
        m_diceRolledEvent.AddListener ((x) => m_lastResult = x);
    }

    public void RollD6 (int targetNumber = -1) { // -1 or 0 for a real physics roll
        m_lastResult = -1;
        if (m_diceRollCoroutine == null) {
            m_diceRollCoroutine = StartCoroutine (RollDie (targetNumber));
        } else {
            StopCoroutine (m_diceRollCoroutine);
            m_diceRollCoroutine = StartCoroutine (RollDie (targetNumber));
        }
    }

    public void HideDie () {
        m_diceAnimator.enabled = false;
        m_d6.GetComponent<MeshRenderer> ().enabled = false;
    }

    IEnumerator RollDie (int targetNumber = -1) {
        if (targetNumber < 1) {
            RollD6_Physics ();
            yield return new WaitForSeconds (1f);
            yield return new WaitUntil (() => !m_script.rolling);
            Debug.Log ("Rolled dice, result: " + m_script.value);
            m_diceRolledEvent.Invoke (m_script.value);
        } else {
            m_diceAnimator.enabled = true;
            m_diceAnimator.SetInteger ("rollNumber", targetNumber);
            yield return new WaitForSeconds (2f);
            m_diceRolledEvent.Invoke (targetNumber);
        }

    }

    [NaughtyAttributes.Button]
    void RollD6_Physics_Button () {
        RollD6 ();
    }
    void RollD6_Physics () {
        m_diceAnimator.enabled = false;
        m_d6.GetComponent<MeshRenderer> ().enabled = true;
        m_d6.transform.position = m_followTransform.position + m_originPoint;
        m_d6.transform.rotation = Random.rotation;

        m_d6.GetComponent<Rigidbody> ().AddForce (m_force, ForceMode.Impulse);
        // apply a random torque
        m_d6.GetComponent<Rigidbody> ().AddTorque (new Vector3 (-70 * Random.value * m_d6.transform.localScale.magnitude, -70 * Random.value * m_d6.transform.localScale.magnitude, -70 * Random.value * m_d6.transform.localScale.magnitude), ForceMode.Impulse);
    }

    [NaughtyAttributes.Button]
    void Roll1 () {
        RollD6 (1);
    }

    [NaughtyAttributes.Button]
    void Roll2 () {
        RollD6 (2);
    }

    [NaughtyAttributes.Button]
    void Roll3 () {
        RollD6 (3);
    }

    [NaughtyAttributes.Button]
    void Roll4 () {
        RollD6 (4);
    }

    [NaughtyAttributes.Button]
    void Roll5 () {
        RollD6 (5);
    }

    [NaughtyAttributes.Button]
    void Roll6 () {
        RollD6 (6);
    }

    // Update is called once per frame
    void Update () {

    }
}