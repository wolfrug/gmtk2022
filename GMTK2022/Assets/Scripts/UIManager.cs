﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public static UIManager instance;
    public Canvas m_mainCanvas;

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

    // Update is called once per frame
    void Update () {

    }
}