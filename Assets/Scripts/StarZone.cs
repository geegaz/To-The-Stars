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

    [SerializeField] private ParticleSystem paint;
    [SerializeField] private float paintPauseTime = 0.8f;

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
            paint.Play();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (paint != null && paint.isPlaying) {
            if ((paint.time / paint.main.duration) >= paintPauseTime) {
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
            playerColor.a = 0.5f;
            
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
}
