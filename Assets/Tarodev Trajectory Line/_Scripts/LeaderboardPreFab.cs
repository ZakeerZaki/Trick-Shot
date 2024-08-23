using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardPreFab : MonoBehaviour
{
    public TMP_Text rankText;
    public TMP_Text usernameText;
    public TMP_Text scoreText;
    public void SetPlayerScore(int rank, string username, int score)
    {
        rankText.text = rank.ToString();
        usernameText.text = username;
        scoreText.text = score.ToString();
    }
}