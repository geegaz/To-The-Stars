using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{
    
    public int playerID;
    private PlayerInput player;
    private SpriteRenderer render;
    private Rigidbody2D body;

    // Movement variables
    [Header("Movement variables")]
    [SerializeField] private float movementSpeed = 10.0f;
    [SerializeField] private float lerpSpeed = 10f;
    private Vector2 velocity;

    // Stars variables
    [Header("Stars variables")]
    [SerializeField] private List<Star> nearbyStars = new List<Star>();
    [SerializeField] private Star targetedStar;

    // Select variables
    [Header("Tracing variables")]
    [SerializeField] private SelectCursor selectCursorPrefab;
    [SerializeField] private SelectCursor select;
    
    [SerializeField] private LineTracer lineTracerPrefab;
    
    
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerInput>();
        render = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        render.color = GameManager.GetPlayerColorFromID(playerID);
    }

    private void Update() {
        Vector3 pos = transform.position;
        pos.x += velocity.x * Time.deltaTime;
        pos.y += velocity.y * Time.deltaTime;

        transform.position = pos;
        targetedStar = GetClosestStar();

        // Lerping position when on Gamepad
        if (player.currentControlScheme == "Gamepad" && targetedStar != null && velocity.sqrMagnitude <= 0.0) {
            transform.position = Vector3.Lerp(transform.position, targetedStar.transform.position, lerpSpeed * Time.deltaTime);
        }
        // Handling selection while a star is selected
        if (select != null) {
            select.transform.position = transform.position;
            if (!select.star.CanConnect(playerID)) {
                Destroy(select.gameObject);
                select = null;
            }
        }

        transform.position = ScreenBounds.LimitPosition(transform.position);
    }

    private Star GetClosestStar() {
        Vector3 pos = transform.position;
        
        Star selectedStar = null;
        if (select != null) selectedStar = select.star;
        
        Star closestStar = null;
        float closestDistance = 0.0f;
        foreach (Star nearbyStar in nearbyStars)
        {
            Vector3 starPos = nearbyStar.transform.position;
            float starDist = Vector3.Distance(starPos, pos);
            if ((closestStar == null || starDist < closestDistance) && nearbyStar.CanConnectTo(playerID, selectedStar)) {
                closestStar = nearbyStar;
                closestDistance = starDist;
            }
        }
        return closestStar;
    }

    private void TrySelect() {
        if (targetedStar != null) {
            select = Instantiate<SelectCursor>(
                selectCursorPrefab, 
                targetedStar.transform.position, 
                targetedStar.transform.rotation
            );
            select.star = targetedStar;
        }
    }

    private void TryStart() {
        if (select != null) {
            if (targetedStar != null) {
                LineTracer line = Instantiate(lineTracerPrefab, select.star.transform.position, select.star.transform.rotation);
                line.startStar = select.star;
                line.endStar = targetedStar;
                line.playerID = playerID;
            }
            Destroy(select.gameObject);
            select = null;
        }
    }

    private void OnMove(InputValue value) {
        Vector2 movementVector = value.Get<Vector2>();
        velocity = movementVector * movementSpeed;
    }

    private void OnActivate(InputValue value) {
        if (value.isPressed) {
            TrySelect();
        } else {
            TryStart();
        }
    }

    private void OnEndGame(InputValue value) {
        GameManager.AskForEndGame();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Star starComponent = other.GetComponent<Star>();
        if (!(starComponent == null || nearbyStars.Contains(starComponent))) {
            nearbyStars.Add(starComponent);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Star starComponent = other.GetComponent<Star>();
        if (starComponent != null && nearbyStars.Contains(starComponent)) {
            nearbyStars.Remove(starComponent);
        }
    }
}
