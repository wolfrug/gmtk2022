using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerMethodHelper : MonoBehaviour {
    // Just to let triggers be triggers and not have to drag and drop things!
    // Start is called before the first frame update

    public void TriggerDiceRoll (int targetNumber) {
        GameManager.instance.TriggerRollDie (targetNumber);
    }
    public void TriggerDeterministicDiceRoll () {
        int targetNumber = (int) InkWriter.main.story.variablesState["nextDiceTarget"];
        TriggerDiceRoll (targetNumber);
    }
}