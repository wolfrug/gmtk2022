using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogSnippet {
    public TypeWriterQueue talker;
    public string inkknot;
    private int index = -1;
    public int Index {
        set {
            index = value;
        }
        get {
            return index;
        }
    }
}

public class InkStringtableDialog : MonoBehaviour {
    [NaughtyAttributes.ReorderableList]
    public List<DialogSnippet> m_orderedDialog = new List<DialogSnippet> { };
    public bool m_running = false;
    public int m_currentIndex = -1;
    // Start is called before the first frame update
    void Start () {
        int indexStart = 0;
        foreach (DialogSnippet snippet in m_orderedDialog) {
            snippet.talker.m_queueEndedEvent.AddListener ((x) => ContinueDialog (snippet));
            snippet.Index = indexStart;
            indexStart++;
        }
    }

    public void StartDialog (int index = 0) {
        if (m_running) {
            Debug.LogWarning ("Cannot interrupt or start a new stringtable dialog!", gameObject);
            return;
        }
        if (m_orderedDialog.Count < 1) {
            Debug.LogWarning ("No dialog snippets assigned!", gameObject);
            return;
        }
        m_running = true;
        m_orderedDialog[index].talker.CreateAndPlayTypeWriterQueueFromKnot (m_orderedDialog[index].inkknot);
        m_currentIndex = index;

    }
    void ContinueDialog (DialogSnippet writer) {
        if (m_running) {
            // So any one of the typewriters has just finished...
            if (writer.Index >= m_orderedDialog.Count) {
                // Finished!
                EndDialog ();
                return;
            }
            m_currentIndex = writer.Index + 1;
            if (m_currentIndex < m_orderedDialog.Count) {
                m_orderedDialog[m_currentIndex].talker.CreateAndPlayTypeWriterQueueFromKnot (m_orderedDialog[m_currentIndex].inkknot);
            } else {
                Debug.LogWarning ("Err, what? Index " + m_currentIndex + " count: " + m_orderedDialog.Count + " writer index: " + writer.Index);
            }
        };
    }

    public void EndDialog () {
        m_running = false;
    }
}