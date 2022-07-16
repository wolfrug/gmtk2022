using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDie : MonoBehaviour {
    public int m_value;
    public Image m_targetImage;
    public List<Sprite> m_diceNumbersNormal = new List<Sprite> { };
    public List<Sprite> m_diceNumbersDark = new List<Sprite> { };

    // Start is called before the first frame update
    void Init () {
        SetValue (m_value);
    }

    public void SetValue (int value) { // Note, actual dice value
        if (value > 0 && value < m_diceNumbersNormal.Count + 1) {
            if (GameManager.instance.m_gameIsNormal) {
                m_targetImage.sprite = m_diceNumbersNormal[value - 1];
            } else {
                m_targetImage.sprite = m_diceNumbersDark[value - 1];
            }
            m_value = value;
        }
    }
}