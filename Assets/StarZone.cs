using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarZone : MonoBehaviour
{
    public int playerID = -1;
    public List<Star> zoneStars = new List<Star>();

    private MeshFilter filter;
    private MeshRenderer render;
    private PolygonCollider2D collide;
    
    private void Awake() {
        filter = GetComponent<MeshFilter>();
        render = GetComponent<MeshRenderer>();
        collide = GetComponent<PolygonCollider2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        GenerateZone();
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void GenerateZone() {
        int count = zoneStars.Count;
        
        Vector2[] points = new Vector2[count];
        for (int i = 0; i < count; i++){
            points[i] = zoneStars[i].transform.localPosition;
        }
        collide.points = points;

        Mesh zoneMesh = collide.CreateMesh(false, false);
        filter.mesh = zoneMesh;
    }
}
