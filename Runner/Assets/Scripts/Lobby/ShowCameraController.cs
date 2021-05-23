using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCameraController : MonoBehaviour
{
    public Vector3 StartCameraPosition;
    private Vector3 touchStart;
    public Transform camParent;
    public Camera cam;
    public float groundZ = 0;
    public Vector4 cameraRect;


    private void Start()
    {
        StartCameraPosition = transform.position;
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = GetWorldPosition(groundZ);
        }
        if (Input.GetMouseButton(0))
        {
            Vector3 direction = touchStart - GetWorldPosition(groundZ);
            cam.transform.position += direction;
            Vector3 pos = cam.transform.position;
            cam.transform.position = new Vector3(Mathf.Clamp(pos.x, cameraRect.x, cameraRect.y), pos.y, Mathf.Clamp(pos.z, cameraRect.z, cameraRect.w));
        }
    }
    private Vector3 GetWorldPosition(float z)
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, new Vector3(0, 0, z));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}
