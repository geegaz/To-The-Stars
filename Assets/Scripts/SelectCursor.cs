using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCursor : MonoBehaviour
{
    public int playerID;
    private float rotationSpeed = 10.0f;

    private void Update() {
        transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
    }
}
