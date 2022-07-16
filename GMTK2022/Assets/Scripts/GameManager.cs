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
    INVENTORY = 4000,
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

    [SerializeField]
    private GameState currentState;
    public float lateInitWait = 0.1f;
    private Dictionary<GameStates, GameState> gameStateDict = new Dictionary<GameStates, GameState> { };
    private BasicAgent player;

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
    }

    public void Restart () {
        Time.timeScale = 1f;
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name, LoadSceneMode.Single);
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
    public void Ink_DamagePlayer (object[] input) {
        int activationNumber = (int) input[1];
        if (!activationNumbers.Contains (activationNumber)) {
            activationNumbers.Add (activationNumber);
            int damageAmount = (int) input[0];
            DamagePlayer (damageAmount);
        };
    }
    public void DamagePlayer (int amount) {
        if (m_gameIsNormal) {
            m_normalHealth.ChangeHealth (-amount);
            if (m_normalHealth.m_currentHealth < 1) {
                Player.Kill ();
            }
        } else {
            m_darkHealth.ChangeHealth (-amount);
            if (m_darkHealth.m_currentHealth < 1) {
                Player.Kill ();
            }
        }
        UIManager.instance.UpdatePlayerHealth ();
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

}