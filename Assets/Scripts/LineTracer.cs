using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineTracer : MonoBehaviour
{
    public int playerID = 0;

    [SerializeField] private ContactFilter2D filter;
    [SerializeField] private float margin = 0.05f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private TracerBird tracer; 

    private LineRenderer render;
    private EdgeCollider2D collide;
    public Star startStar;
    public Star endStar;

    private Vector2 velocity = Vector2.zero;
    private Vector2 direction = Vector2.zero;

    private void Awake() {
        render = GetComponent<LineRenderer>();
        collide = GetComponent<EdgeCollider2D>();
    }

    private void Start() {
        Color playerColor = GameManager.GetPlayerColorFromID(playerID);
        Color playerLineColor = Color.Lerp(playerColor, new Color(1.0f, 1.0f, 1.0f, 0.0f), 0.5f);
        tracer.render.color = playerColor;
        render.startColor = playerLineColor;
        render.endColor = playerLineColor;

        direction = (endStar.transform.position - transform.position).normalized;
        transform.right = direction;

        startStar.OnConnect.AddListener(OnStarConnect);
    }

    private void FixedUpdate()
    {
        // Only runs when the line is being traced
        if (tracer != null) {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            if (collide.Raycast(direction, filter, hits, speed * Time.deltaTime) > 0) {
                Collider2D col = hits[0].collider;
                Star hitStar = col.GetComponent<Star>();
                if (hitStar != null) {
                    if (hitStar == endStar) {
                        TryConnectLine();
                    }
                } else {
                    Debug.Log("Destroying line "+this+", hit something");
                    StopLine();
                }
            }
            velocity = direction * speed;
            Vector3 pos = transform.position;
            pos.x += velocity.x * Time.deltaTime;
            pos.y += velocity.y * Time.deltaTime;
            transform.position = pos;

            UpdateLine();
        }
    }

    private void UpdateLine() {
        float distance = Vector3.Distance(startStar.transform.position, transform.position);
        
        render.SetPosition(0, Vector3.left * margin);
        render.SetPosition(1, Vector3.left * (distance - margin));

        Vector2[] points = {
            Vector2.left * margin,
            Vector2.left * (distance - margin)
        };
        collide.points = points;
    }

    private void StopLine() {
        // Can be done safely, no other object should be referencing this one
        Destroy(this.gameObject);
    }

    private void TryConnectLine() {
        // If the end line was already taken over, stop the line
        if (endStar.CanConnect(playerID)) {
            startStar.Connect(endStar, playerID);

            // Reposition line
            transform.position = endStar.transform.position;
            UpdateLine();

            // Remove the tracing visual
            Destroy(tracer.gameObject);
            tracer = null;
        } else {
            Debug.Log("Destroying line "+this+", can't connect to end star");
            StopLine();
        }
    }

    public void OnStarConnect(Star from, Star to) {
        // If the starting star was connected by another player, stop the line
        // Needs an additional condition for when the LineTracer connects its own stars
        if (!(to == endStar || from == startStar || from.CanConnect(playerID))) {
            Debug.Log("Destroying line "+this+", start star got taken over");
            StopLine();
        }
    }
}
