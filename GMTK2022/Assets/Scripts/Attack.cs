using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    public BasicAgent m_attachedAgent;
    public GenericTrigger m_attackTrigger;
    public NPCType m_type;
    public float m_attackRange = 5f;
    public float m_attackSpeed = 2f;
    private float m_nextAttack;
    public int m_attackDamage = 1;
    public float m_pushbackForce = 5f;
    public bool m_useKeyboardToAttack = false;
    public bool m_active = true;
    public KeyCode keyCode;

    public FMODUnity.StudioEventEmitter eventEmitter_hit;

    private bool m_multiAttackPrevention = false;
    public GameObject m_hitPrefab;
    // Start is called before the first frame update
    void Start () {
        //m_attackTrigger.triggerEntered.AddListener (TriggerDoDamage);
        m_nextAttack = m_attackSpeed;
    }

    // Attack directions: 0 up 1 down 2 right 3 left
    public bool AttackForward () {
        if (m_nextAttack <= 0f) {
            m_attachedAgent.animator.SetTrigger ("attack");
            m_nextAttack = m_attackSpeed;
            return true;
        }
        return false;
    }

    /* void OnCollisionEnter (Collision collision) {
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay (contact.point, contact.normal, Color.white);
        }
        Debug.Log ("Attack collided with something.", gameObject);
        if (!m_multiAttackPrevention) {
            m_multiAttackPrevention = true;
            TriggerDoDamage (collision.gameObject);
        };
    }
*/
    public void AnimTriggerDamage () {
        foreach (GameObject target in m_attackTrigger.contents) {
            TriggerDoDamage (target);
        }
    }
    public void TriggerDoDamage (GameObject target) {
        if (m_active) {
            BasicAgent enemyAgent = target.GetComponent<BasicAgent> ();
            if (enemyAgent != null) {
                if (enemyAgent == GameManager.instance.Player) { // damage player
                    GameManager.instance.DamagePlayer (m_attackDamage);
                    if (!enemyAgent.m_isDead) {
                        Vector3 moveDirection = Vector3.right;
                        if (m_attachedAgent.m_currentFacing == Facing.Left || m_attachedAgent.m_currentFacing == Facing.Down) {
                            moveDirection = Vector3.left;
                        }
                        enemyAgent.navMeshAgent.Move (moveDirection * m_pushbackForce);
                    };
                } else {
                    NPC enemyNPC = enemyAgent.GetComponent<NPC> ();
                    if (enemyNPC != null) {
                        enemyNPC.Damage (m_attackDamage);
                        eventEmitter_hit.Play ();
                        if (!enemyAgent.m_isDead) {
                            Vector3 moveDirection = Vector3.right;
                            if (m_attachedAgent.m_currentFacing == Facing.Left || m_attachedAgent.m_currentFacing == Facing.Down) {
                                moveDirection = Vector3.left;
                            }
                            enemyAgent.navMeshAgent.Move (moveDirection * m_pushbackForce);
                        };
                    } else {
                        enemyAgent.Kill ();
                    };

                }
                if (m_hitPrefab != null) {
                    Instantiate (m_hitPrefab, (enemyAgent.transform.position + ((Vector3) Random.insideUnitCircle * 2f)), Quaternion.identity);
                }
            }
        };
        m_multiAttackPrevention = false;
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
            m_nextAttack -= Time.deltaTime;
        }
        // If the attached agent is dead, we're no longer active
        m_active = !m_attachedAgent.m_isDead;
    }
}