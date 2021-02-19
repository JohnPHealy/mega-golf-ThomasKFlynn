using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GolfBallController golfBall;
    public TextMeshProUGUI labelPlayerName;

    private PlayerRecord playerRecord;
    private int playerIndex;


    void Start()
    {
        playerRecord = GameObject.Find("Player Record").GetComponent<PlayerRecord>();
        playerIndex = 0;
        SetupPlayer();
    }

    private void SetupPlayer()
    {
        golfBall.SetUpBall(playerRecord.playerColours[playerIndex]);
        labelPlayerName.text = playerRecord.playerList[playerIndex].name;
    }

    public void NextPlayer(int previousShots)
    {
        playerRecord.AddShots(playerIndex, previousShots);
        if (playerIndex < playerRecord.playerList.Count-1)
        {
            playerIndex++;
            SetupPlayer();
        }
        else
        {
            if (playerRecord.levelIndex == playerRecord.level.Length-1)
            {
                Debug.Log("ScoreBoard");
            }
            else
            {
                playerRecord.levelIndex++;
                SceneManager.LoadScene(playerRecord.level[playerRecord.levelIndex]);
            }
        }
    }

}
