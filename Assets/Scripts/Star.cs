using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Star : MonoBehaviour
{
    public int playerID = -1;
    public bool claimed = false;

    public UnityEvent<Star, Star> OnConnect = new UnityEvent<Star, Star>();

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

        if (OnConnect != null) OnConnect.Invoke(this, otherStar);
    }

    public void SetOwner(int id) {
        playerID = id;
        render.color = GameManager.GetPlayerColorFromID(playerID);
    }

    public bool GetNext(Star start, List<Star> list) {
        foreach (Star star in connectedStars)
        {
            if (star != this) {
                if (star == start) {
                    return true;
                } else {
                    list.Add(star);
                    return star.GetNext(start, list);
                }
            }
        }
        return false;
    }
}
