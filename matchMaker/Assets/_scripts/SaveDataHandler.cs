using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStateData
{
    public int MoveCount;
    public int Score;
    public string PlayerName;

    public int RowCount;
    public int ColCount;
    public List<KeyValuePair<string, int>> Cards;

    public const string PlayerPrefsKeyName = "SavedGameState";

    public GameStateData(string playerName)
    {
        PlayerName = playerName;
        MoveCount = 0;
        Score = 0;
    }

    public void UpdatePlayerData(int multiplier)
    {
        MoveCount++;
        Score += 1 * multiplier;
    }

    public void SaveToPlayerPrefs()
    {
        // Convert this GameState instance to a JSON string
        string json = JsonUtility.ToJson(this);

        // Save the converted JSON into the PlayerPrefs
        PlayerPrefs.SetString(PlayerPrefsKeyName, json);
        PlayerPrefs.Save();
    }

    public static GameStateData CreateFromPlayerPrefs()
    {
        // If the game was never saved before, the key will not exist; in this case return null
        if (!PlayerPrefs.HasKey(PlayerPrefsKeyName))
            return null;

        // Retrieve the saved JSON string from the player prefs
        string json = PlayerPrefs.GetString(PlayerPrefsKeyName);

        // Deserialize the JSON string into a new GameState object and return it
        return JsonUtility.FromJson<GameStateData>(json);
    }
}

public class SaveDataHandler : MonoBehaviour
{
    public static GameStateData CurrentStateData;
    [SerializeField] private GridConfigSO _gridConfig;
    [SerializeField, Header("listening to"), Space(2)] private BoolEC_SO _evalChannel;
    [SerializeField, Header("broadcasting on"), Space(2)] private BoolEC_SO _loadDataChannel;
    public GameStateData StateData;

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

    public void LoadData()
    {
        StartCoroutine(LoadState());
    }

    private IEnumerator LoadState()
    {
        var state = GameStateData.CreateFromPlayerPrefs();

        if (state != null)
        {
            Debug.Log("save found. Player : " + state.PlayerName);
            CurrentStateData = state;

            _gridConfig.ColCount = CurrentStateData.ColCount;
            _gridConfig.RowCount = CurrentStateData.RowCount;

            _gridConfig.WriteToDict(CurrentStateData.Cards);
        }

        yield return null;

        StateData = CurrentStateData;

        _loadDataChannel.RaiseEvent(state != null);
    }

    public void CreateState()
    {
        Debug.Log("no saved data! creating...");

        CurrentStateData = new GameStateData("unknown" + Random.Range(0, 9) + Random.Range(0, 9));
        CurrentStateData.RowCount = _gridConfig.RowCount;
        CurrentStateData.ColCount = _gridConfig.ColCount;

        _gridConfig.AddElements();
        
        var cards = _gridConfig.ReadDict();

        if (cards != null)
        {
            CurrentStateData.Cards = cards;
        }

        else
        {
            Debug.Log("card dict not found!", this);
        }

        StateData = CurrentStateData;
    }

    [ContextMenu("Save State")]
    private void SaveState()
    {
        var cards = _gridConfig.ReadDict();
        if (cards != null)
        {
            CurrentStateData.Cards = cards;
            CurrentStateData.ColCount = _gridConfig.ColCount;
            CurrentStateData.RowCount = _gridConfig.RowCount;

            CurrentStateData.SaveToPlayerPrefs();

            Debug.Log("save successful");
        }
    }

    [ContextMenu("Delete SaveData")]
    public void DeleteSave()
    {
        if (PlayerPrefs.HasKey("SavedGameState"))
        {
            PlayerPrefs.DeleteKey("SavedGameState");
            Debug.Log("save delete successful");
        }
    }
}
