using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Star : MonoBehaviour
{
    public int playerID = -1;

    public UnityEvent OnConnect = new UnityEvent();

    public List<Star> connectedStars;
    public int connectedStarsMax = 2;

    public void Connect(Star otherStar, int _playerID) {
        connectedStars.Add(otherStar);
        otherStar.connectedStars.Add(this);
        if (playerID < 0) {
            playerID = _playerID;
        }
        if (otherStar.playerID < 0) {
            otherStar.playerID = _playerID;
        }

        if (OnConnect != null) OnConnect.Invoke();
    }
}
