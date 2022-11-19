using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public List<PlayerCursor> players;

    private void OnPlayerJoined(PlayerInput playerInput) {
        PlayerCursor player = playerInput.GetComponent<PlayerCursor>();
        if (player != null) {
            player.playerID = players.Count;
            players.Add(player);
        }
    }

    public int GetPlayerID(PlayerCursor player) {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == player) return i;
        }
        return -1;
    }

    public PlayerCursor GetPlayerFromID(int id) {
        if (Mathf.Abs(id) < players.Count) return players[id];
        else return null;
    }
}
