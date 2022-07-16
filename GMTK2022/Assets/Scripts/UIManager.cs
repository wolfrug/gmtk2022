using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;
    public Canvas m_mainCanvas;
    public GameObject m_loadingScreen;
    public Transform m_rolledDiceParent;
    public GameObject m_UIDiePrefab;

    public List<UIDie> m_spawnedUIDie = new List<UIDie> { };

    public TextMeshProUGUI m_healthObjectText;
    public List<GameObject> m_healthObjectsNormal = new List<GameObject> { };
    public PlayerHealth m_normalHealthObject;
    public List<GameObject> m_healthObjectsDark = new List<GameObject> { };
    public PlayerHealth m_darkHealthObject;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy (gameObject);
        }
    }
    // Start is called before the first frame update
    void Start () {
        m_darkHealthObject.ResetHealth ();
        m_normalHealthObject.ResetHealth ();
        UpdatePlayerHealth ();
    }

    public void SpawnNewRolledDie (int value) {
        GameObject newRolledDie = Instantiate (m_UIDiePrefab, m_rolledDiceParent);
        UIDie script = newRolledDie.GetComponent<UIDie> ();
        script.SetValue (value);
        m_spawnedUIDie.Add (script);
    }

    public bool QueryDie (int dieNumber) {
        foreach (UIDie die in m_spawnedUIDie) {
            if (die.m_value == dieNumber) {
                return true;
            }
        }
        return false;
    }
    public void DestroyDie (int dieNumber) {
        UIDie targetDie = null;
        foreach (UIDie die in m_spawnedUIDie) {
            if (die.m_value == dieNumber) {
                targetDie = die;
                break;
            }
        }
        if (targetDie != null) {
            m_spawnedUIDie.Remove (targetDie);
            Destroy (targetDie.gameObject);
        }
    }

    public void FlipRolledDice () { // Flips the rolled dice to the 'upside down' versions
        foreach (UIDie die in m_spawnedUIDie) {
            switch (die.m_value) {
                case 1:
                    {
                        die.SetValue (6);
                        break;
                    }
                case 2:
                    {
                        die.SetValue (5);
                        break;
                    }
                case 3:
                    {
                        die.SetValue (4);
                        break;
                    }
                case 4:
                    {
                        die.SetValue (3);
                        break;
                    }
                case 5:
                    {
                        die.SetValue (2);
                        break;
                    }
                case 6:
                    {
                        die.SetValue (1);
                        break;
                    }
            }
        }

    }

    public void UpdatePlayerHealth () {
        foreach (GameObject trg in m_healthObjectsNormal) {
            trg.SetActive (false);
        }
        foreach (GameObject trg in m_healthObjectsDark) {
            trg.SetActive (false);
        }
        if (GameManager.instance.m_gameIsNormal) {
            for (int i = 0; i < m_normalHealthObject.m_currentHealth; i++) {
                m_healthObjectsNormal[i].SetActive (true);
            }
            m_healthObjectText.SetText (m_normalHealthObject.m_healthName);
        } else {
            for (int i = 0; i < m_darkHealthObject.m_currentHealth; i++) {
                m_healthObjectsDark[i].SetActive (true);
            }
            m_healthObjectText.SetText (m_darkHealthObject.m_healthName);
        }
    }

    public void LoadDarkWorld () {
        m_loadingScreen.SetActive (true);
        GameManager.instance.ActionWaiter (1f, new System.Action (() => m_loadingScreen.SetActive (false)));
    }

    // Update is called once per frame
    void Update () {

    }
}