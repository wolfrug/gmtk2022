using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu (fileName = "Data", menuName = "Player Health", order = 1)]
public class PlayerHealth : ScriptableObject {
    public int m_startingHealth = 1;
    public int m_currentHealth = 1;
    public string m_healthName = "Embarrassment";

    public void ResetHealth () {
        m_currentHealth = m_startingHealth;
    }

    public void ChangeHealth (int amount) {
        m_currentHealth = Mathf.Clamp (m_currentHealth + amount, 0, 999);
    }

}