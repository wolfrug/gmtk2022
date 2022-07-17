using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameStateEvent : UnityEvent<GameState> { }

public enum GameStates {
    NONE = 0000,
    INIT = 1000,
    LATE_INIT = 1100,
    GAME = 2000,
    NARRATIVE = 3000,
    NARRATIVE_INGAME = 3001,
    LOADING = 4000,
    DEFEAT = 5000,
    WIN = 6000,
    PAUSE = 7000,

}

[System.Serializable]
public class GameState {
    public GameStates state;
    public GameStates nextState;
    public GameStateEvent evtStart;

}

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public bool initOnStart = true;
    [NaughtyAttributes.ReorderableList]
    public GameState[] gameStates;

    // Changes when we go to dark mode
    public bool m_gameIsNormal = true;
    public PlayerHealth m_normalHealth;
    public PlayerHealth m_darkHealth;

    public TypeWriterQueue m_thoughtWriter;
    public InkStringtableManager m_inkStringtableManager;

    public Transform m_normalWorldStart;
    public Transform m_darkWorldStart;

    [SerializeField]
    private GameState currentState;
    public float lateInitWait = 0.1f;
    private Dictionary<GameStates, GameState> gameStateDict = new Dictionary<GameStates, GameState> { };
    private BasicAgent player;
    private DiceRoller diceRoller;

    void Awake () {
        if (instance == null) {
            instance = this;
        } else {
            Destroy (gameObject);
        }
        foreach (GameState states in gameStates) {
            gameStateDict.Add (states.state, states);
        }
    }
    void Start () {
        if (initOnStart) {
            Invoke ("Init", 1f); // uncomment if not going via mainmenu
        };
        DiceRoller.m_diceRolledEvent.AddListener (WaitForDieRollKnot);
        //AudioManager.instance.PlayMusic ("MusicBG");
    }

    [NaughtyAttributes.Button]
    public void Init () {
        SetState (GameStates.INIT);
        //Invoke ("FixTerribleBug", 5f);
        //NextState ();
    }

    void Late_Init () {
        currentState = gameStateDict[GameStates.LATE_INIT];
        Debug.Log ("Invoking late init");
        currentState.evtStart.Invoke (currentState);
        if (currentState.nextState != GameStates.NONE) {
            NextState ();
        }
    }
    public void NextState () {
        if (currentState.nextState != GameStates.NONE) {
            if (gameStateDict[currentState.state].nextState == GameStates.LATE_INIT) { // late init inits a bit late and only works thru nextstate
                Invoke ("Late_Init", lateInitWait);
                // Debug.Log ("Invoking late init");
                return;
            } else {
                Debug.Log ("Invoking Next State " + "(" + gameStateDict[currentState.state].nextState.ToString () + ")");
                SetState (gameStateDict[currentState.state].nextState);
            };
        }
    }
    public void SetState (GameStates state) {
        if (state != GameStates.NONE) {
            GameState = state;
        };
    }
    public GameState GetState (GameStates state) {
        foreach (GameState getState in gameStates) {
            if (getState.state == state) {
                return getState;
            }
        }
        return null;
    }
    public GameStates GameState {
        get {
            if (currentState != null) {
                return currentState.state;
            } else {
                return GameStates.NONE;
            }
        }
        set {
            Debug.Log ("Changing state to " + value);
            currentState = gameStateDict[value];
            currentState.evtStart.Invoke (currentState);
            if (currentState.nextState != GameStates.NONE) {
                NextState ();
            };
        }
    }

    public void WinGame () {
        GameState = GameStates.WIN;
        Debug.Log ("Victory!!");
        SceneManager.LoadScene ("endscene");
    }
    public void Defeat () {

        currentState = gameStateDict[GameStates.DEFEAT];
        currentState.evtStart.Invoke (currentState);
        ActionWaiter (2f, new System.Action (() => SceneManager.LoadScene ("defeatscene")));

    }

    public void Restart () {
        Time.timeScale = 1f;
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name, LoadSceneMode.Single);
    }

    public static void LoadGameScene () {
        SceneManager.LoadScene ("SampleScene");
    }

    public void DualLoadScenes () {
        SceneManager.LoadScene ("ManagersScene", LoadSceneMode.Additive);
        SceneManager.LoadScene ("SA_Demo", LoadSceneMode.Additive);
    }

    [NaughtyAttributes.Button]
    public void BackToMenu () {
        Time.timeScale = 1f;
        SceneManager.LoadScene ("mainmenu");
    }
    public void Quit () {
        Application.Quit ();
    }

    /* [NaughtyAttributes.Button]
    public void SaveGame () {
        Debug.Log ("Saving game");
        SaveManager.instance.IsNewGame = false;
        InkWriter.main.SaveStory ();
        foreach (InventoryController ctrl in InventoryController.allInventories) {
            ctrl.SaveInventory ();
        }
        AudioManager.instance.SaveVolume ();
        SceneController.instance.SaveScene ();
        SaveManager.instance.SaveCache ();
    }

    [NaughtyAttributes.Button]
    public void LoadGame () {
        Debug.Log ("Loading game");
        Restart ();
    }
    public void Pause () {
        GameState oldState = currentState;
        GameState pauseState = gameStateDict[GameStates.PAUSE];
        GameState = GameStates.PAUSE;
        pauseState.evtStart.Invoke (gameStateDict[GameStates.PAUSE]);
        StartCoroutine (PauseWaiter (oldState.state));
        Time.timeScale = 0f;
    }
    public void UnPause () {
        GameState = GameStates.NONE;
        Time.timeScale = 1f;
    }
    IEnumerator PauseWaiter (GameStates continueState) {
        yield return new WaitUntil (() => GameState != GameStates.PAUSE);
        GameState = continueState;
    }
*/

    public void PlayWriterQueueFromKnot (string targetKnot) {
        // First we create a list of strings from the knot
        string[] knotStrings = m_inkStringtableManager.CreateStringArray (targetKnot);
        // Then we set it to play on the typewriter
        if (knotStrings.Length > 0) {
            PlayWriterQueue (knotStrings);
        } else {
            Debug.LogWarning ("Could not play writer queue from knot - no strings found! (" + targetKnot + ")");
        }
    }
    public void PlayWriterQueue (string[] targetStrings) {
        WriterAction[] newQueue = TypeWriterQueue.CreateTypeWriterQueue (targetStrings);
        m_thoughtWriter.SetQueue (newQueue);
        m_thoughtWriter.StartQueue (0);
        Debug.Log ("Starting new writer queue of length " + targetStrings.Length + " with contents starting with " + targetStrings[0]);
    }

    List<int> activationNumbers = new List<int> { };
    string ink_targetKnot = "";
    public void Ink_DamagePlayer (object[] input) {
        int activationNumber = (int) input[1];
        if (!activationNumbers.Contains (activationNumber)) {
            activationNumbers.Add (activationNumber);
            int damageAmount = (int) input[0];
            DamagePlayer (damageAmount);
        };
    }
    public void Ink_StartNarrative (object[] input) {
        Debug.Log ("Starting narrative from ink");
        SetState (GameStates.NARRATIVE_INGAME);
    }
    public void Ink_StopNarrative (object[] input) {
        Debug.Log ("Ending narrative from ink");
        SetState (GameStates.GAME);
    }

    public void TriggerRollDie (int targetNumber) {
        // Trigger from a trigger at the end of a chat
        SetState (GameStates.NARRATIVE_INGAME);
        ink_targetKnot = (string) InkWriter.main.story.variablesState["diceKnotTarget"];
        DiceRoller.RollD6 (targetNumber);
    }
    public void Ink_RollDie (object[] input) {
        int activationNumber = (int) input[2];
        if (!activationNumbers.Contains (activationNumber)) {
            SetState (GameStates.NARRATIVE_INGAME);
            activationNumbers.Add (activationNumber);
            int targetNumber = (int) input[0];
            ink_targetKnot = (string) input[1];
            DiceRoller.RollD6 (targetNumber);
        }
    }
    public void Ink_QueryDie (object[] input) {
        // Very lol
        InkWriter.main.story.variablesState["hasDieNr1"] = UIManager.instance.QueryDie (1);
        InkWriter.main.story.variablesState["hasDieNr2"] = UIManager.instance.QueryDie (2);
        InkWriter.main.story.variablesState["hasDieNr3"] = UIManager.instance.QueryDie (3);
        InkWriter.main.story.variablesState["hasDieNr4"] = UIManager.instance.QueryDie (4);
        InkWriter.main.story.variablesState["hasDieNr5"] = UIManager.instance.QueryDie (5);
        InkWriter.main.story.variablesState["hasDieNr6"] = UIManager.instance.QueryDie (6);
    }
    public void Ink_DestroyDie (object[] input) {
        int activationNumber = (int) input[1];
        if (!activationNumbers.Contains (activationNumber)) {
            activationNumbers.Add (activationNumber);
            int targetNumber = (int) input[0];
            UIManager.instance.DestroyDie (targetNumber);
        }
    }
    void WaitForDieRollKnot (int targetNumber) {
        if (ink_targetKnot != "") {
            InkWriter.main.story.variablesState["diceRollResult"] = targetNumber;
            UIManager.instance.SpawnNewRolledDie (targetNumber);
            ActionWaiter (1f, new System.Action (() => GoToKnotAfterDieRoll ()));
        }
    }
    void GoToKnotAfterDieRoll () {
        InkWriter.main.GoToKnot (ink_targetKnot);
        ink_targetKnot = "";
        DiceRoller.HideDie ();
    }

    public void DamagePlayer (int amount) {
        if (m_gameIsNormal) {
            m_normalHealth.ChangeHealth (-amount);
            if (m_normalHealth.m_currentHealth < 1) {
                DelayActionUntil (() => GameState == GameStates.GAME, new System.Action (() => Player.Kill ()));
                DelayActionUntil (() => GameState == GameStates.GAME, new System.Action (() => SwitchToDarkWorldWaiter ()));
            }
        } else {
            m_darkHealth.ChangeHealth (-amount);
            if (m_darkHealth.m_currentHealth < 1) {
                DelayActionUntil (() => GameState == GameStates.GAME, new System.Action (() => Player.Kill ()));
                DelayActionUntil (() => GameState == GameStates.GAME, new System.Action (() => SwitchToLightWorldWaiter ()));
            }
        }
        UIManager.instance.UpdatePlayerHealth ();
    }

    public void ChangeNormalHealth (int amount) { // e.g. in the dark world
        m_normalHealth.ChangeHealth (amount);
    }
    public void ChangeDarkHealth (int amount) { // e.g. in the dark world
        m_darkHealth.ChangeHealth (amount);
    }

    public void SwitchToDarkWorldWaiter () {
        SetState (GameStates.LOADING);
        ActionWaiter (3f, new System.Action (() => SwitchToDarkWorld ()));
    }
    public void SwitchToDarkWorld () {
        UIManager.instance.LoadDarkWorld ();
        if (m_darkHealth.m_currentHealth <= 0) {
            Defeat ();
            return;
        }
        m_gameIsNormal = false;
        m_normalWorldStart.transform.position = Player.transform.position;
        UIManager.instance.UpdatePlayerHealth ();
        UIManager.instance.FlipRolledDice ();
        Player.transform.Find ("Avatar").gameObject.SetActive (false);
        Player.transform.Find ("AvatarDark").gameObject.SetActive (true);
        Player.navMeshAgent.speed = 6f;
        Player.GetComponent<Attack> ().enabled = true;
        Player.Resurrect ();
        Player.navMeshAgent.Warp (m_darkWorldStart.position);
        SetState (GameStates.GAME);
    }
    public void SwitchToLightWorldWaiter () {
        SetState (GameStates.LOADING);
        ActionWaiter (3f, new System.Action (() => SwitchToLightWorld ()));
    }
    public void SwitchToLightWorld () {
        UIManager.instance.LoadDarkWorld ();
        if (m_normalHealth.m_currentHealth <= 0) {
            Defeat ();
            return;
        }
        m_gameIsNormal = true;
        m_darkWorldStart.transform.position = Player.transform.position;
        UIManager.instance.UpdatePlayerHealth ();
        UIManager.instance.FlipRolledDice ();
        Player.navMeshAgent.speed = 3.5f;
        Player.transform.Find ("Avatar").gameObject.SetActive (true);
        Player.transform.Find ("AvatarDark").gameObject.SetActive (false);
        Player.GetComponent<Attack> ().enabled = false;
        Player.Resurrect ();
        Player.navMeshAgent.Warp (m_normalWorldStart.position);
        SetState (GameStates.GAME);
    }

    public BasicAgent Player {
        get {
            if (player == null) {
                player = GameObject.FindGameObjectWithTag ("Player").GetComponent<BasicAgent> ();
                //mover.targetAgent = player.navMeshAgent;
            };
            return player;
        }
    }
    public DiceRoller DiceRoller {
        get {
            if (diceRoller == null) {
                diceRoller = GameObject.FindObjectOfType<DiceRoller> ();
                //mover.targetAgent = player.navMeshAgent;
            };
            return diceRoller;
        }
    }

    public void ActionWaiter (float timeToWait, System.Action callBack) {
        StartCoroutine (ActionWaiterCoroutine (timeToWait, callBack));
    }
    IEnumerator ActionWaiterCoroutine (float timeToWait, System.Action callBack) {
        yield return new WaitForSeconds (timeToWait);
        callBack.Invoke ();
    }

    public void DelayActionUntil (System.Func<bool> condition, System.Action callBack) {
        StartCoroutine (DelayActionCoroutine (condition, callBack));
    }
    IEnumerator DelayActionCoroutine (System.Func<bool> condition, System.Action callBack) {

        bool outvar = false;
        while (!outvar) {
            outvar = condition ();
            yield return new WaitForSeconds (0.1f);
        };
        Debug.Log ("Delayed action finished?!");
        callBack.Invoke ();
    }

}