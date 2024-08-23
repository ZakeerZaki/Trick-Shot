using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.MultiplayerModels;
[System.Serializable]
public class LeaderboardEntry
{
    public string DisplayName;
    public int Score;
    public int Rank;


    public LeaderboardEntry(int rank, string displayName, int score)
    {
        Rank = rank;
        DisplayName = displayName;
        Score = score;
    }
    public override string ToString()
    {
        return $"{Rank}. {DisplayName}: {Score}";
    }
}
public class PlayfabManager : MonoBehaviour
{
    public int scoreGame;

    public List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();

    public string TitleId = "F7385";
    public static PlayfabManager Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        // DontDestroyOnLoad(gameObject);

    }
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = TitleId;
        }

        LoginwithCustromID();
    }
    public void LoginwithCustromID()
    {

        #region CustomID
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        #endregion
    }
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        Debug.Log(result);

        //  splashScreen.SetActive(false);
        //  MainGameMenue.SetActive(true);
        GetScore();
        GetLeaderboard();


    }
    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }


    #region
    public void UpdatePlayerStatistics(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlayerScore",
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnStatisticsUpdateSuccess, OnError);
    }

    public void GetScore()
    {
        var request = new GetPlayerStatisticsRequest();
        PlayFabClientAPI.GetPlayerStatistics(request, OnGetPlayerStatistics, OnPlayFabError);
    }

    private void OnGetPlayerStatistics(GetPlayerStatisticsResult result)
    {
        if (result.Statistics != null)
        {
            foreach (var stat in result.Statistics)
            {
                if (stat.StatisticName == "PlayerScore")
                {
                    int playerScore = stat.Value;
                    Debug.Log("Player's Score: " + playerScore);
                    scoreGame = playerScore;
                    break;
                }
            }
        }
        else
        {
            Debug.Log("No statistics available for the player.");
        }
    }

    private void OnStatisticsUpdateSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfully updated player statistics");
    }

    private void OnLeaderboardGet(GetLeaderboardResult result)
    {
        if (result.Leaderboard.Count > 0)
        {
            int playerScore = result.Leaderboard[0].StatValue;
            Debug.Log("Player's Score: " + playerScore);
            // Update GameManagerInstance with the retrieved score
            scoreGame = playerScore;
        }
        else
        {
            Debug.Log("No leaderboard entries found for the player.");
        }
    }

    private void OnDataSendSuccess(UpdateUserDataResult result)
    {
        Debug.Log("Successfully updated player data.");
    }

    private void OnPlayFabError(PlayFabError error)
    {
        Debug.LogError("PlayFab error: " + error.GenerateErrorReport());
    }


    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlayerScore",
            StartPosition = 0,
            MaxResultsCount = 6
        };
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboard, OnPlayFabError);
    }

    private void OnGetLeaderboard(GetLeaderboardResult result)
    {
        leaderboardEntries.Clear();

        if (result.Leaderboard.Count > 0)
        {
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                var item = result.Leaderboard[i];
                string displayName = item.PlayFabId;
                //  string displayName = string.IsNullOrEmpty(item.DisplayName) ? "Unknown User" : item.DisplayName;
                int rank = i + 1; // Calculate rank

                // Create a new leaderboard entry
                var leaderboardEntry = new LeaderboardEntry(rank, displayName, item.StatValue);

                // Add the leaderboard entry to the list
                leaderboardEntries.Add(leaderboardEntry);

                // Log the leaderboard entry
                Debug.Log(leaderboardEntry);
            }
        }
        else
        {
            Debug.Log("No leaderboard entries found.");
        }
    }
    void OnError(PlayFabError error)
    {
        Debug.LogError("Error: " + error.GenerateErrorReport());
    }
    #endregion
}
