using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private float endGameTime = 20.0f;
    private bool endGame = false;
    private float remainingTime = 0.0f;

    [SerializeField]
    private GameObject endGameScreen;
    [SerializeField]
    private StarsManager starsManager;


    public List<PlayerCursor> players;
    public List<Color> playerColors;
    
    private static GameManager _instance;
    public static GameManager instance {
        get {
            if (_instance == null) Debug.LogError("GameManager is NULL");
            return _instance;
        }
    }

    private void Awake() {
        _instance = this;
    }

    private void Update() {
        if (endGame && remainingTime > 0.0f) {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0.0f) {
                EndGame();
            }
        }
    }

    private void OnPlayerJoined(PlayerInput playerInput) {
        PlayerCursor player = playerInput.GetComponent<PlayerCursor>();
        if (player != null) {
            player.playerID = players.Count;
            players.Add(player);
        }
    }

    public static int GetPlayerID(PlayerCursor player) {
        for (int i = 0; i < instance.players.Count; i++)
        {
            if (instance.players[i] == player) return i;
        }
        return -1;
    }

    public static PlayerCursor GetPlayerFromID(int id) {
        if (Mathf.Abs(id) < instance.players.Count) return instance.players[id];
        else return null;
    }

    public static Color GetPlayerColor(PlayerCursor player) {
        int id = GetPlayerID(player);
        return GetPlayerColorFromID(id);
    }

    public static Color GetPlayerColorFromID(int id) {
        if (Mathf.Abs(id) < instance.playerColors.Count) {
            return instance.playerColors[id];
        }
        else return Color.white;
    }

    public static void AskForEndGame() {
        if (!instance.endGame) {
            Debug.Log("Asked for end game");
            instance.endGame = true;
            instance.remainingTime = instance.endGameTime;
        }
    }

    private void EndGame() {
        Debug.Log("It's the end !");
        ScreenCapture.CaptureScreenshot("ToTheStars_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")+".png");
        if (starsManager != null) {
            foreach (PlayerCursor player in players)
            {
                Debug.Log("Player "+player.playerID+": "+starsManager.GetPlayerScore(player.playerID));
            }
        }
    }
}
