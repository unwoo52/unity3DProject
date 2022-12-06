using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform targetTransform = null;
    public new Camera camera = null;

    public Vector2 zoomLimit = new(3.0f, 10.0f);//¡‹ √÷º“ √÷¥Î ∞™

    public float zoomSpeed = 5.0f;//¡‹ Ω∫««µÂ

    private void Update()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0.0f)
        {
            camera.orthographicSize -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSpeed;
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, zoomLimit.x, zoomLimit.y);
        }

        transform.position = targetTransform.position + -Vector3.forward;
    }
}
