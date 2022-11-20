using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineTracer : MonoBehaviour
{
    public int playerID = 0;

    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private Transform tracer; 

    private LineRenderer renderer;
    private EdgeCollider2D collider;
    public Star startStar;
    public Star endStar;

    private Vector2 velocity = Vector2.zero;
    private Vector2 direction = Vector2.zero;

    private void Awake() {
        renderer = GetComponent<LineRenderer>();
        collider = GetComponent<EdgeCollider2D>();
    }

    private void Start() {
        SetLinePosition(0, transform.position);
        SetLinePosition(1, transform.position);

        if (endStar != null) {
            direction = (endStar.transform.position - transform.position).normalized;
            endStar.OnConnect.AddListener(OnStarConnect);
        }
        if (startStar != null) {
            startStar.OnConnect.AddListener(OnStarConnect);
        }

        if (tracer != null) {
            tracer.transform.right = direction;
        }
    }

    private void FixedUpdate()
    {
        if (tracer != null) {
            // TODO: Fix raycast not detecting stars
            /* 
            What happens is that the raycast hits the LineTracer's own collider first,
            which stops the line from detecting the star.
            Best solution would be to ignore selectively the LineTracer's collider, but
            it doesn't seem to be an option...
            Another one would be to add a script and a trigger to the tracer and detect
            when it enters the correct star, but it's kinda heavy.
            Last one is an ugly hack which consists of switching the collider to the 
            "Ignore Raycast" layer before doing the raycast, or disabling it temporarily. 
            Will probably work fine though, so I think I'll go with it :')
            */
            RaycastHit2D hit = Physics2D.Raycast(tracer.transform.position, direction, speed * Time.deltaTime);
            Collider2D col = hit.collider;
            if (col != null && col != collider) {
                Star hitStar = col.GetComponent<Star>();
                if (hitStar != null) {
                    if (endStar && hitStar == endStar) {
                        TryConnectLine();
                    }
                } else {
                    StopLine();
                }
            }
            Vector3 pos = tracer.transform.position;
            velocity = direction * speed;
            pos.x += velocity.x * Time.deltaTime;
            pos.y += velocity.y * Time.deltaTime;
            tracer.transform.position = pos;

            SetLinePosition(1, tracer.transform.position);
        }
    }

    private void StopLine() {
        // Can be done safely, no other object is referencing this one
        Destroy(this.gameObject);
    }

    private void TryConnectLine() {
        if (CanConnectLine()) {
            Debug.Log("Connecting line");
            startStar.Connect(endStar, playerID);
            SetLinePosition(1, endStar.transform.position);

            Destroy(tracer.gameObject);
            tracer = null;
        } else {
            StopLine();
        }
    }

    private bool CanConnectLine() {
        return (
            endStar.connectedStars.Count < endStar.connectedStarsMax &&
            (endStar.playerID < 0 || endStar.playerID == playerID)
        );
    }

    public void OnStarConnect() {
        if (startStar != null) {
            if (!(startStar.playerID < 0 || startStar.playerID == playerID)) {
                StopLine();
            }
        }
    }

    // position is in world space
    private void SetLinePosition(int index, Vector3 position) {
        renderer.SetPosition(index, position);

        Vector2[] points = collider.points;
        points[index] = position - transform.position;
        collider.points = points;
    }
}
