using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScore : MonoBehaviour
{
    public int playerID = -1;

    [Header("Display")]
    [SerializeField] private Image birdImage;
    [SerializeField] private TMP_Text scoreText;

    private void Start() {
        if (birdImage != null) {
            birdImage.color = GameManager.GetPlayerColorFromID(playerID);
        }
        if (scoreText != null) {
            scoreText.text = GameManager.GetPlayerScoreFromID(playerID).ToString();
        }
    }
}
