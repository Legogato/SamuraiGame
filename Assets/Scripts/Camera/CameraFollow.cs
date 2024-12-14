using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{

    public List<Transform> players;

    public Vector3 offset;

    private Vector3 velocity;

    private float smoothTime = 0.1f;

    public float minZoom = 40f;
    public float maxZoom = 10f;

    public float zoomLimiter = 50f;
    private Camera cam;


    public Vector2 minCampPos, maxCampPos;

    public GameObject borderBoxRigth;
    public GameObject borderBoxLeft;


    private void Start()
    {
        cam = GetComponentInChildren<Camera>();
    }

    private void LateUpdate()
    {
        Move();
        Zoom();
        BorderColliders();
    }

    Vector3 GetCenterPoint()
    {
        if(players.Count == 1)
        {
            return players[0].position;
        }

        var bounds = new Bounds(players[0].position, Vector3.zero);
        for(int i = 0; i<players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.center;
    }

   

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreaterDistance()/zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime*2);
    }

    float GetGreaterDistance()
    {
        var bounds = new Bounds(players[0].position, Vector3.zero);
        for(int i = 0; i< players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.size.x;
    }
    private void Move()
    {
        if (players.Count == 0)
        {
            return;
        }

        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;


        float posY = Mathf.SmoothDamp(transform.position.y, newPosition.y, ref velocity.x, smoothTime);
        float posX = Mathf.SmoothDamp(transform.position.x, newPosition.x, ref velocity.y, smoothTime);


        transform.position = new Vector3(Mathf.Clamp(posX, minCampPos.x, maxCampPos.x),Mathf.Clamp(posY, minCampPos.y, maxCampPos.y), transform.position.z);

     

    }

    private void BorderColliders()
    {
        borderBoxRigth.transform.position = GetCenterPoint()+ new Vector3(9f, 2.5f, 0);
        borderBoxLeft.transform.position = GetCenterPoint()+ new Vector3(-9f, 2.5f, 0);
    }

}
