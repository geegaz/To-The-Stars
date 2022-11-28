using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarZone : MonoBehaviour
{
    public int playerID = -1;
    public List<Star> stars = new List<Star>();

    private MeshFilter filter;
    private MeshRenderer render;
    private PolygonCollider2D collide;

    [Header("Visuals")]
    [SerializeField] private ParticleSystem paint;
    [SerializeField, Range(0.0f, 1.0f)] private float paintPauseAt = 0.8f;
    [SerializeField] private float zoneTransparency = 0.25f;

    [HideInInspector] public StarsManager manager;
    
    private void Awake() {
        filter = GetComponent<MeshFilter>();
        render = GetComponent<MeshRenderer>();
        collide = GetComponent<PolygonCollider2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        GenerateZone();
        ClaimStarsInZone();

        if (paint != null) {
            float paintScale = collide.bounds.size.magnitude;
            var main = paint.main;
            main.startSizeMultiplier = paintScale;
            paint.Play();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (paint != null && paint.isPlaying) {
            if ((paint.time / paint.main.duration) >= paintPauseAt) {
                paint.Pause();
            }
        }
    }

    public void GenerateZone() {
        int count = stars.Count;
        
        // Create collider
        Vector2[] points = new Vector2[count];
        for (int i = 0; i < count; i++){
            points[i] = stars[i].transform.position;
        }
        collide.points = points;

        // Create mesh
        Mesh zoneMesh = collide.CreateMesh(false, false);
        if (zoneMesh != null) {
            Color playerColor = GameManager.GetPlayerColorFromID(playerID);
            playerColor.a = zoneTransparency;
            
            Color32[] zoneColors = new Color32[zoneMesh.vertexCount];
            for (int i = 0; i < zoneMesh.vertexCount; i++)
            {
                zoneColors[i] = playerColor;
            }
            zoneMesh.colors32 = zoneColors;
            filter.mesh = zoneMesh; 
        }
    }

    public void ClaimStarsInZone() {
        if (manager != null) {
            foreach (Star star in manager.stars)
            {
                if (collide.OverlapPoint(star.transform.position)) {
                    star.SetOwner(playerID);
                    star.inZone = true;
                }
            }
        }
    }

    public void RemoveZonesInZone() {
        if (manager != null) {
            foreach (StarZone zone in manager.starZones)
            {
                if (collide.IsTouching(zone.collide)) {
                    
                }
            }
        }
    }
}
