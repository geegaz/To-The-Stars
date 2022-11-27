using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Star : MonoBehaviour
{
    public int playerID = -1;
    public bool inZone = false;

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

    public bool GetNext(Star start, Star previous, List<Star> list) {
        foreach (Star star in connectedStars)
        {
            if (star != previous) {
                list.Add(star);
                if (star == start) {
                    return true;
                } else {
                    return star.GetNext(start, this, list);
                }
            }
        }
        return false;
    }

    public bool CanConnect(int _playerID, Star other = null) {
        /*
        * Conditions for targeting a star:
        * - the star is different from the other given star
        * - there are less than 2 stars already connected to it
        * - the star is either not claimed yet, or is already claimed by the same player
        * - the star is not inside a zone
        */
        if (other == null) return (
            connectedStars.Count < connectedStarsMax &&
            (playerID < 0 || playerID == _playerID) &&
            !inZone
        );
        else return (
            connectedStars.Count < connectedStarsMax &&
            (playerID < 0 || playerID == _playerID) &&
            !inZone &&
            this != other && 
            !connectedStars.Contains(other)
        );
    }
}
