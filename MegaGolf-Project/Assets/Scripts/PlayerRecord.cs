using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRecord : MonoBehaviour
{
    public List<Player> playerList;
    public string[] level;
    public Color[] playerColours;
    [HideInInspector] public int levelIndex;

    private void OnEnable()
    {
        playerList = new List<Player>();
        DontDestroyOnLoad(gameObject);
    }

    public void AddPlayer(string name)
    {
        playerList.Add(new Player(name, playerColours[playerList.Count], level.Length));
    }

    public void AddShots(int playerIndex, int ShotCount)
    {
        playerList[playerIndex].shots[levelIndex] = ShotCount;
    }

    public class Player
    {
        public string name;
        public Color colour;
        public int[] shots;

        public Player (string newName, Color newColour, int levelCount)
        {
            name = newName;
            colour = newColour;
            shots = new int[levelCount];
        }
    }
}
