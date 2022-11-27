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

    // Used by other classes
    public void Connect(Star other, int _playerID) {
        _Connect(other, _playerID);
        other._Connect(this, _playerID);
    }

    // Used internally
    private void _Connect(Star other, int _playerID){
        connectedStars.Add(other);
        if (playerID < 0) {
            SetOwner(_playerID);
        }
        if (OnConnect != null) OnConnect.Invoke(this, other);
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

    public bool CanConnect(int _playerID) {
        /*
        * Conditions for connecting a star:
        * - there are less than 2 stars already connected to it
        * - the star is either not claimed yet, or is already claimed by the same player
        * - the star is not inside a zone
        */
        return (
            connectedStars.Count < connectedStarsMax &&
            (playerID < 0 || playerID == _playerID) &&
            !inZone
        );
    }

    public bool CanConnectTo(int _playerID, Star other = null) {
        /* 
        * Additional checks for connecting to another star, checks if:
        * - this star is different from the other
        * - this star is not already connected to the other
        */
        return (
            CanConnect(_playerID) &&
            this != other && !connectedStars.Contains(other)
        );
    }
}
