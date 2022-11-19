using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCursor : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed = 10.0f;
    private Vector2 velocity;

    private void Update() {
        Vector3 pos = transform.position;
        pos.x += velocity.x* Time.deltaTime;
        pos.y += velocity.y * Time.deltaTime;

        transform.position = pos;
    }

    private void OnMove(InputValue movement) {
        Vector2 movementVector = movement.Get<Vector2>();
        velocity = movementVector * movementSpeed;
    }
}
