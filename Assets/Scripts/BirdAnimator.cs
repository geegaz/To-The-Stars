using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimator : MonoBehaviour
{
    public List<Sprite> animations = new List<Sprite>();
    public int[][] patterns;
    public float framerate = 6.0f;
    
    private float timer = 0.0f;
    private int currentAnim = 0;
    private int currentFrame = 0;

    private SpriteRenderer renderer;

    private void Awake() {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > (1.0f / framerate)) {
            timer = 0.0f;
        }
    }
}
