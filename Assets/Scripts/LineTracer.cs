using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineTracer : MonoBehaviour
{
    public int playerID = 0;

    [SerializeField]
    private ContactFilter2D filter;
    [SerializeField]
    private float speed = 1.0f;
    [SerializeField]
    private Transform tracer; 

    private LineRenderer render;
    private EdgeCollider2D col;
    public Star startStar;
    public Star endStar;

    private Vector2 velocity = Vector2.zero;
    private Vector2 direction = Vector2.zero;

    private void Awake() {
        render = GetComponent<LineRenderer>();
        col = GetComponent<EdgeCollider2D>();

        render.colorGradient = CreateGradient(GameManager.GetPlayerColorFromID(playerID));
    }

    private void Start() {
        direction = (endStar.transform.position - transform.position).normalized;
        startStar.OnConnect.AddListener(OnStarConnect);
        transform.right = direction;
    }

    private void FixedUpdate()
    {
        if (tracer != null) {
            RaycastHit2D[] hits = new RaycastHit2D[1];
            if (col.Raycast(direction, filter, hits, speed * Time.deltaTime) > 0) {
                Collider2D _col = hits[0].collider;
                Star hitStar = _col.GetComponent<Star>();
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
        
        render.SetPosition(0, Vector3.zero);
        render.SetPosition(1, Vector3.left * distance);

        Vector2[] points = {
            Vector2.zero,
            Vector2.left * distance
        };
        col.points = points;
    }

    private void StopLine() {
        // Can be done safely, no other object should be referencing this one
        Destroy(this.gameObject);
    }

    private void TryConnectLine() {
        if (CanConnectTo(endStar)) {
            startStar.Connect(endStar, playerID);
            // Reposition line
            transform.position = endStar.transform.position;
            //UpdateLine();
            // Remove the tracing visual
            Destroy(tracer.gameObject);
            tracer = null;
        } else {
            Debug.Log("Destroying line "+this+", can't connect to end star");
            StopLine();
        }
    }

    private Gradient CreateGradient(Color color) {
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[1];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[1];
        colorKeys[0].color = color;
        //alphaKeys[0].alpha

        return gradient;
    }

    public void OnStarConnect() {
        if (!CanConnectTo(startStar)) {
            Debug.Log("Destroying line "+this+", start star got taken over");
            StopLine();
        }
    }

    private bool CanConnectTo(Star star) {
        return (
            star.connectedStars.Count < star.connectedStarsMax &&
            (star.playerID < 0 || star.playerID == playerID)
        );
    }
}
