using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsManager : MonoBehaviour
{
    [SerializeField] private Rect starsRect;
    [SerializeField] private Vector2 ColumnsAndRows = new Vector2(8, 5);
    [SerializeField] private Star starPrefab;
    [SerializeField] private StarZone starZonePrefab;

    public List<Star> stars = new List<Star>();

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
                }
            }
        }
    }

    public void ClaimStarsInZone(PolygonCollider2D poly, int playerID) {
        foreach (Star star in stars)
        {
            if (poly.OverlapPoint(star.transform.position)) {
                star.inZone = true;
                star.SetOwner(playerID);
            }
        }
    }

    public int GetPlayerScore(int playerID) {
        int score = 0;
        foreach (Star star in stars)
        {
            if (star.playerID == playerID) score++;
        }
        return score;
    }

    private void OnStarConnect(Star from, Star to) {
        List<Star> stars = new List<Star>();

    }
}
