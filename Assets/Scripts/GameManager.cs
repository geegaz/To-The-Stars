using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
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
}
