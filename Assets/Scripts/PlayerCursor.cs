using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{
    
    public int playerID;
    private PlayerInput player;
    private Rigidbody2D body;
    
    // Movement variables
    [SerializeField]
    private float movementSpeed = 10.0f;
    [SerializeField]
    private float lerpSpeed = 0.1f;
    private Vector2 velocity;
    private float lerpTime = 1.0f;
    
    // Stars variables
    [SerializeField]
    private List<Star> nearbyStars = new List<Star>();
    private Star targetedStar;

    // Select variables
    [SerializeField]
    private SelectCursor selectCursorPrefab;
    private SelectCursor select;
    
    
    private void Awake() {
        body = GetComponent<Rigidbody2D>();
        player = GetComponent<PlayerInput>();
    }

    private void Update() {
        Vector3 pos = transform.position;
        pos.x += velocity.x * Time.deltaTime;
        pos.y += velocity.y * Time.deltaTime;

        transform.position = pos;
        targetedStar = GetClosestStar();

        if (velocity.sqrMagnitude <= 0.0 && targetedStar != null) {
            lerpTime = Mathf.Max(lerpTime - lerpSpeed * Time.deltaTime, 0.0f);
            transform.position = Vector3.Lerp(transform.position, targetedStar.transform.position, lerpTime);
        } else {
            lerpTime = 1.0f;
        }

        transform.position = ScreenBounds.LimitPosition(transform.position);
    }
    
    /*
     Worked fine with a gamepad, 
     but gave jittery movement with a mouse
    */
    // private void FixedUpdate() {
    //     Vector2 pos = body.position;
    //     pos.x += velocity.x * Time.deltaTime;
    //     pos.y += velocity.y * Time.deltaTime;

    //     body.MovePosition(pos);
    // }

    private Star GetClosestStar() {
        Vector3 pos = transform.position;
        
        Star closestStar = null;
        float closestDistance = 0.0f;
        foreach (Star nearbyStar in nearbyStars)
        {
            Vector3 starPos = nearbyStar.transform.position;
            float starDist = Vector3.Distance(starPos, pos);
            if (closestStar == null || starDist < closestDistance) {
                closestStar = nearbyStar;
                closestDistance = starDist;
            }
        }
        return closestStar;
    }

    private void SelectStar() {
        bool canSelectStar = !(
            targetedStar == null ||
            (
                targetedStar.playerID >= 0 &&
                targetedStar.playerID != playerID
            ) ||
            (
                targetedStar.star1 != null && 
                targetedStar.star2 != null
            )
        );
        if (canSelectStar) {
            select = Instantiate<SelectCursor>(
                selectCursorPrefab, 
                targetedStar.transform.position, 
                targetedStar.transform.rotation
            );
            Debug.Log("Selected star");
        }
    }

    private void StartLine() {
        Destroy(select.gameObject);
        select = null;
        Debug.Log("Started line");
    }

    private void OnMove(InputValue value) {
        Vector2 movementVector = value.Get<Vector2>();
        velocity = movementVector * movementSpeed;
    }

    private void OnActivate(InputValue value) {
        if (!value.isPressed) return;

        if (select != null) {
            StartLine();
        } else {
            SelectStar();
        }
    }



    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other + " entered the cursor");
        Star starComponent = other.GetComponent<Star>();
        if (!(starComponent == null || nearbyStars.Contains(starComponent))) {
            nearbyStars.Add(starComponent);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log(other + " exited the cursor");
        Star starComponent = other.GetComponent<Star>();
        if (starComponent != null && nearbyStars.Contains(starComponent)) {
            nearbyStars.Remove(starComponent);
        }
    }
}
