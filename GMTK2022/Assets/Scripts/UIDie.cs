using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDie : MonoBehaviour {
    public int m_value;
    public Image m_targetImage;
    public List<Sprite> m_diceNumbers = new List<Sprite> { };

    // Start is called before the first frame update
    void Init () {
        SetValue (m_value);
    }

    public void SetValue (int value) { // Note, actual dice value
        value--; // we reduce it by one
        if (value >= 0 && value < m_diceNumbers.Count) {
            m_targetImage.sprite = m_diceNumbers[value];
        }
    }
}