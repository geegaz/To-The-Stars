using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCursor : MonoBehaviour
{
    public Star star;
    public float rotationSpeed = 20.0f;

    private void Update() {
        transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
    }
}
