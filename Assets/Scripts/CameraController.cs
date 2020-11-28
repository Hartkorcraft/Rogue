using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform target = null;
    [SerializeField] private float cameraSpeed = 0.125f;    

    void LateUpdate()
    {
        Vector3 smoothPosition = Vector3.Lerp(transform.position, target.position, cameraSpeed * Time.deltaTime);
        transform.position = new Vector3(smoothPosition.x, smoothPosition.y, -10);
    }
}
