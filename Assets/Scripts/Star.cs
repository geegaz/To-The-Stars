using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Star : MonoBehaviour
{
    private const int MAX_CONNECTIONS = 2;
    
    public int playerID = -1;
    public bool inZone = false;

    public UnityEvent<Star, Star> OnConnect = new UnityEvent<Star, Star>();

    public List<Star> connectedStars;

    private SpriteRenderer render;
    [HideInInspector] public StarsManager manager;

    private void Awake() {
        render = GetComponent<SpriteRenderer>();
    }

    // Used by other classes
    public void Connect(Star other, int _playerID) {
        _Connect(other, _playerID);
        other._Connect(this, _playerID);

        // If the other star has reached the max number of connexions, it's possible the shape was just closed
        // Then, send an event to try to create a zone
        if (other.connectedStars.Count == MAX_CONNECTIONS && manager != null) {
            manager.TryCreateZone(this, other);
        }
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
        list.Add(this);
        foreach (Star star in connectedStars)
        {
            if (star != previous) {
                if (star == start) {
                    return true;
                } else {
                    return star.GetNext(start, this, list);
                }
            }
        }
        return false;
    }

    public bool CanConnect(int id) {
        /*
        * Conditions for connecting a star:
        * - there are less than 2 stars already connected to it
        * - the star is either not claimed yet, or is already claimed by the same player
        * - the star is not inside a zone
        */
        return (
            connectedStars.Count < MAX_CONNECTIONS &&
            (playerID < 0 || playerID == id) &&
            !inZone
        );
    }

    public bool CanConnectTo(int id, Star other = null) {
        /* 
        * Additional checks for connecting to another star, checks if:
        * - this star is different from the other
        * - this star is not already connected to the other
        */
        return (
            CanConnect(id) &&
            this != other && !connectedStars.Contains(other)
        );
    }
}
