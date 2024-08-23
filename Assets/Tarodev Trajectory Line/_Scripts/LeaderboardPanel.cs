using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardPanel : MonoBehaviour
{
    public GameObject playerScoreBarPrefab;
    public Transform contentPanel;

    private void OnEnable()
    {
        PopulateLeaderboard();
    }

    public void PopulateLeaderboard()
    {
        ClearLeaderboard();

        // Use the list from PlayfabManager to populate the leaderboard
        var leaderboardEntries = PlayfabManager.Instance.leaderboardEntries;

        for (int i = 0; i < leaderboardEntries.Count; i++)
        {
            var playerInfo = leaderboardEntries[i];
            GameObject newPlayerScoreBar = Instantiate(playerScoreBarPrefab, contentPanel);
            LeaderboardPreFab scoreBar = newPlayerScoreBar.GetComponent<LeaderboardPreFab>();
            scoreBar.SetPlayerScore(playerInfo.Rank, playerInfo.DisplayName, playerInfo.Score); // Assuming SetPlayerScore takes (rank, name, score) parameters
        }
    }

    private void ClearLeaderboard()
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }
    }
}