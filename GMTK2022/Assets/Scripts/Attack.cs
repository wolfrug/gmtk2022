using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    public BasicAgent m_attachedAgent;
    public GenericTrigger m_attackTrigger;
    public NPCType m_type;
    public float m_attackRange = 5f;
    public int m_attackDamage = 1;
    public float m_pushbackForce = 50f;
    public bool m_useKeyboardToAttack = false;
    public bool m_active = true;
    public KeyCode keyCode;
    // Start is called before the first frame update
    void Start () {
        m_attackTrigger.triggerEntered.AddListener (TriggerDoDamage);
    }

    // Attack directions: 0 up 1 down 2 right 3 left
    public void AttackForward () {
        m_attachedAgent.animator.SetTrigger ("attack");
    }

    void OnCollisionEnter (Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay (contact.point, contact.normal, Color.white);
        }
        Debug.Log ("Attack collided with something.", gameObject);
        TriggerDoDamage (collision.gameObject);
    }

    public void TriggerDoDamage (GameObject target) {
        if (m_active) {
            BasicAgent enemyAgent = target.GetComponent<BasicAgent> ();
            if (enemyAgent != null) {
                if (enemyAgent == GameManager.instance.Player) { // damage player
                    GameManager.instance.DamagePlayer (m_attackDamage);
                } else {
                    NPC enemyNPC = enemyAgent.GetComponent<NPC> ();
                    if (enemyNPC != null) {
                        enemyNPC.Damage (m_attackDamage);
                    } else {
                        enemyAgent.Kill ();
                    };
                    enemyNPC.GetComponent<Rigidbody> ().AddForce (m_attachedAgent.transform.forward * m_pushbackForce, ForceMode.Impulse);
                }
            }
        };
    }
    int GetAttackAnimDirection () {
        switch (m_attachedAgent.m_currentFacing) {
            case Facing.Up:
                return 0;
            case Facing.Down:
                return 1;
            case Facing.Right:
                return 2;
            case Facing.Left:
                return 3;
            default:
                return -1;
        }
    }

    // Update is called once per frame
    void Update () {

        if (m_active) {
            if (GameManager.instance.GameState == GameStates.GAME) {
                m_attachedAgent.animator.SetInteger ("attack_direction", GetAttackAnimDirection ());
                if (m_useKeyboardToAttack) {
                    if (Input.GetKeyDown (keyCode)) {
                        AttackForward ();
                    }
                }
            };
        }
        // If the attached agent is dead, we're no longer active
        m_active = !m_attachedAgent.m_isDead;
    }
}