using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class TracerBird : MonoBehaviour
{
    [SerializeField] private List<Sprite> idleFrames = new List<Sprite>();
    [SerializeField] private List<Sprite> flapFrames = new List<Sprite>();
    private List<Sprite>[] animations = new List<Sprite>[2];

    [SerializeField] private float previousAnimAffect = 0.1f;
    [SerializeField] private float framerate = 6.0f;
    
    private float timer = 0.0f;
    private int currentFrame = 0;
    private int currentAnim = 0;

    private SpriteRenderer spriteRender;
    private Image imageRender;
    private StudioParameterTrigger emitter;

    private void Awake() {
        spriteRender = GetComponent<SpriteRenderer>();
        imageRender = GetComponent<Image>();
        emitter = GetComponent<StudioParameterTrigger>();
    }

    private void Start() {
        animations[0] = idleFrames;
        animations[1] = flapFrames;
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer > (1.0f / framerate)) {
            timer = 0.0f;

            currentFrame ++;
            if (currentFrame >= animations[currentAnim].Count) {
                currentFrame = 0;
                currentAnim = Mathf.RoundToInt(Random.Range(0.0f, 1.0f) - previousAnimAffect);
            }
            if (spriteRender != null)
                spriteRender.sprite = animations[currentAnim][currentFrame];
            if(imageRender != null)
                imageRender.sprite = animations[currentAnim][currentFrame];
            
        }
    }
}
