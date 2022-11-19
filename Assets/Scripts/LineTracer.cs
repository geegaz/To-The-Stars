using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineTracer : MonoBehaviour
{
    public int playerID = 0;

    [SerializeField]
    private float speed = 10.0f;
    [SerializeField]
    private Transform tracer;

    private LineRenderer renderer;
    private EdgeCollider2D collider;
    public Star startStar;
    public Star endStar;

    private void Awake() {
        renderer = GetComponent<LineRenderer>();
        collider = GetComponent<EdgeCollider2D>();
    }

    private void Start() {
        SetLinePosition(0, transform.position);
    }

    private void FixedUpdate()
    {
        if (startStar && endStar) {
            SetLinePosition(1, endStar.transform.position);
        }
    }

    // position is in world space
    public void SetLinePosition(int index, Vector3 position) {
        renderer.SetPosition(index, position);
        collider.points[index] = position - transform.position;
    }
}
