using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class TracerBird : MonoBehaviour
{
    public int playerID = 0;

    [SerializeField]
    private List<Sprite> idleFrames = new List<Sprite>();
    [SerializeField]
    private List<Sprite> flapFrames = new List<Sprite>();
    private List<Sprite>[] animations = new List<Sprite>[2];

    [SerializeField]
    private float previousAnimAffect = 0.1f;
    [SerializeField]
    private float framerate = 6.0f;
    
    private float timer = 0.0f;
    private int currentFrame = 0;
    private int currentAnim = 0;

    private SpriteRenderer render;
    private StudioParameterTrigger emitter;

    private void Awake() {
        render = GetComponent<SpriteRenderer>();
        emitter = GetComponent<StudioParameterTrigger>();
        animations[0] = idleFrames;
        animations[1] = flapFrames;

        // TODO: Set Player parameter in FMOD from playerID
    }

    private void Start() {
        render.color = GameManager.GetPlayerColorFromID(playerID);
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
            render.sprite = animations[currentAnim][currentFrame];
        }
    }
}
