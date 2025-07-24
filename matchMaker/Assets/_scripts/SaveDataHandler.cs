using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameState
{
    public int MoveCount;
    public int Score;
    public string PlayerName;

    public int RowCount;
    public int ColCount;
    public List<KeyValuePair<string, int>> Cards;

    public const string PlayerPrefsKeyName = "SavedGameState";

    public GameState(string playerName)
    {
        PlayerName = playerName;
        MoveCount = 0;
        Score = 0;
    }

    public void SetPlayerData(int moveCount, int score)
    {
        MoveCount = moveCount;
        Score = score;
    }

    public void SaveToPlayerPrefs()
    {
        // Convert this GameState instance to a JSON string
        string json = JsonUtility.ToJson(this);

        // Save the converted JSON into the PlayerPrefs
        PlayerPrefs.SetString(PlayerPrefsKeyName, json);
        PlayerPrefs.Save();
    }

    public static GameState CreateFromPlayerPrefs()
    {
        // If the game was never saved before, the key will not exist; in this case return null
        if (!PlayerPrefs.HasKey(PlayerPrefsKeyName))
            return null;

        // Retrieve the saved JSON string from the player prefs
        string json = PlayerPrefs.GetString(PlayerPrefsKeyName);

        // Deserialize the JSON string into a new GameState object and return it
        return JsonUtility.FromJson<GameState>(json);
    }
}

public class SaveDataHandler : MonoBehaviour
{
    public static GameState CurrentState;
    [SerializeField] private GridConfigSO _gridConfig;
    [SerializeField] private BoolEC_SO _evalChannel;

    public GameState GameState;

    private void OnEnable()
    {
        _evalChannel.OnEventRaised += EvalChannel_OnEventRaised;
    }

    private void OnDisable()
    {
        _evalChannel.OnEventRaised -= EvalChannel_OnEventRaised;
    }

    private void EvalChannel_OnEventRaised(bool obj)
    {
        if (obj)
        {
            SaveState();
        }
    }

    private void Start()
    {
        StartCoroutine(LoadState());
    }

    private IEnumerator LoadState()
    {
        var state = GameState.CreateFromPlayerPrefs();

        if (state != null)
        {
            Debug.Log("save found. Player : " + state.PlayerName);
            CurrentState = state;
            
            _gridConfig.Cards = CurrentState.Cards;

            _gridConfig.ColCount = CurrentState.ColCount;
            _gridConfig.RowCount = CurrentState.RowCount;

            _gridConfig.WriteToDict();
        }

        else
        {
            Debug.Log("no saved data! creating...");
            CurrentState = new GameState("unknown");

            _gridConfig.AddElements();
            _gridConfig.ReadDict();
        }

        yield return null;

        GameState = CurrentState;
    }

    [ContextMenu("Save State")]
    private void SaveState()
    {
        if (_gridConfig.ReadDict())
        {
            CurrentState.Cards = _gridConfig.Cards;
            CurrentState.ColCount = _gridConfig.ColCount;
            CurrentState.RowCount = _gridConfig.RowCount;

            CurrentState.SaveToPlayerPrefs();

            Debug.Log("save successful");
        }
    }

    [ContextMenu("Delete SaveData")]
    private void DeleteSave()
    {
        if (PlayerPrefs.HasKey("SavedGameState"))
        {
            PlayerPrefs.DeleteKey("SavedGameState");
        }
    }
}
