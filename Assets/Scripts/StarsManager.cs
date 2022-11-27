using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsManager : MonoBehaviour
{
    [Header("Generation")]
    [SerializeField] private Rect starsRect;
    [SerializeField] private Vector2 ColumnsAndRows = new Vector2(8, 5);

    [Header("Stars Management")]
    [SerializeField] private Star starPrefab;
    [SerializeField] private StarZone starZonePrefab;

    public List<Star> stars = new List<Star>();
    public List<StarZone> starZones = new List<StarZone>();

    private void Start() {
        GenerateStars();
    }

    private void GenerateStars() {
        Vector2 stepSize = starsRect.size / ColumnsAndRows;
        Vector2 offset = Vector2.zero;
        Vector2 finalPos = Vector2.zero;
        for (int x = 0; x < ColumnsAndRows.x; x++)
        {
            for (int y = 0; y < ColumnsAndRows.y; y++) {
                offset.x = Random.Range(0.0f, 1.0f);
                offset.y = Random.Range(0.0f, 1.0f);

                finalPos.x = x;
                finalPos.y = y;
                finalPos = starsRect.position + (finalPos + offset) * stepSize;

                if (starPrefab != null) {
                    Star newStar = Instantiate<Star>(starPrefab, finalPos, Quaternion.identity, transform);
                    stars.Add(newStar);

                    newStar.manager = this;
                }
            }
        }
    }

    public void TryCreateZone(Star start, Star next) {
        string logText = "Trying to create a new zone...\n";
        
        List<Star> linkedStars = new List<Star>();
        if (next.GetNext(start, start, linkedStars)) {
            linkedStars.Add(start);

            if (starZonePrefab != null) {
                StarZone newStarZone = Instantiate<StarZone>(starZonePrefab, Vector3.zero, Quaternion.identity, transform);
                starZones.Add(newStarZone);

                newStarZone.stars = linkedStars;
                newStarZone.playerID = start.playerID;
                newStarZone.manager = this;
            }

            logText += "...A zone was created !";
        } else logText += "...The zone can't be created.";
        logText += string.Format(" {0} stars were linked", linkedStars.Count);

        Debug.Log(logText);
    }

    public int GetPlayerScore(int playerID) {
        int score = 0;
        foreach (Star star in stars)
        {
            if (star.playerID == playerID) score++;
        }
        return score;
    }
}
