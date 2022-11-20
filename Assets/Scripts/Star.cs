using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Star : MonoBehaviour
{
    public int playerID = -1;
    public bool claimed = false;

    public UnityEvent OnConnect = new UnityEvent();

    public List<Star> connectedStars;
    public int connectedStarsMax = 2;

    private SpriteRenderer render;

    private void Awake() {
        render = GetComponent<SpriteRenderer>();
    }

    public void Connect(Star otherStar, int _playerID) {
        connectedStars.Add(otherStar);
        otherStar.connectedStars.Add(this);
        if (playerID < 0) {
            SetOwner(_playerID);
        }
        if (otherStar.playerID < 0) {
            otherStar.SetOwner(_playerID);
        }

        if (OnConnect != null) OnConnect.Invoke();
    }

    public void SetOwner(int id) {
        playerID = id;
        render.color = GameManager.GetPlayerColorFromID(playerID);
    }
}
