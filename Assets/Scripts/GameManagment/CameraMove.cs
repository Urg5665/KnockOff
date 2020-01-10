using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Camera cam;

    public Transform player1;
    public Transform player2;

    public List<Transform> targets;

    public Vector3 offset;

    private Vector3 velocity;

    public float smoothTime = .5f;

    public float minZoom = 30f;
    public float maxZoom = 55f;

    public bool player1Hit;
    public bool player2Hit;

    public int hitTimer;

    public Bounds bounds;

    public float zDif;
    public float xDif;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        bounds = new Bounds(targets[0].position, Vector3.zero);
    }

    Vector3 GetCenterPoint()
    {
       

        if (player1Hit)
        {
            //bounds = new Bounds(targets[0].position, Vector3.zero);
            //bounds.Encapsulate(targets[0].position);
            bounds.center = player1.transform.position;
            return bounds.center;
        }
        if (player2Hit)
        {
            //bounds = new Bounds(targets[1].position, Vector3.zero);
            //bounds.Encapsulate(targets[1].position);
            bounds.center = player2.transform.position;
            return bounds.center;
        }
        else
        {
            bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            return bounds.center;
        }

    }
    void LateUpdate()
    {
        Move();
        if ( !player1Hit && !player2Hit)
        {
            Zoom();
        }
        zDif = Mathf.Abs(player1.position.z - player2.position.z);
        xDif = Mathf.Abs(player1.position.x - player2.position.x);
        //Debug.Log(zDif);
        if ( xDif < 15 && zDif >= 20 && zDif < 30)
        {
            minZoom = 40;
            maxZoom = 65;
        }
        else if (xDif < 15 && zDif >= 30 && zDif < 40)
        {
            minZoom = 50;
            maxZoom = 75;
        }
        else if (xDif < 15 && zDif >= 40 && zDif < 50)
        {
            minZoom = 60;
            maxZoom = 85;
        }
        else if (xDif < 15 && zDif >= 50 && zDif < 60)
        {
            minZoom = 70;
            maxZoom = 95;
        }
        else if (xDif < 15 && zDif >= 60 && zDif < 80)
        {
            minZoom = 80;
            maxZoom = 115;
        }
        else
        {
            minZoom = 30;
            maxZoom = 55;
        }
        /*
        if ( zDif < 30)
        {
            maxZoom = 55;
        }
        if (zDif >= 30)
        {
            maxZoom = 75;
        }
        if (zDif >= 45)
        {
            maxZoom = 95;
        }
        if (zDif >= 50)
        {
            maxZoom = 110;
        }
        if (zDif >= 55)
        {
            maxZoom = 120;
        }
        */

        if (Input.GetKeyDown(KeyCode.G)){
            //StartCoroutine(Shake(.15f, .4f));
        } 

        if ( player1Hit || player2Hit)
        {
            hitTimer++;
            if (hitTimer > 10 && hitTimer < 30)
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, cam.fieldOfView -(Vector3.Distance(player1.position, player2.position) * .25f), Time.deltaTime);
            if (hitTimer > 30)
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, cam.fieldOfView + (Vector3.Distance(player1.position, player2.position)*.25f), Time.deltaTime);
        }
        if (hitTimer == 50)
        {
            hitTimer = 0;
            player2Hit = false;
            player1Hit = false;
        }

    }

    private void Zoom()
    {
        float newZoom = Mathf.Lerp(minZoom, maxZoom, GetGreatestDistance() / 50f); // 50f
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
        GetGreatestDistance();
    }

    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for( int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }


    private void Move() 
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPostion = centerPoint + offset;
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, newPostion, ref velocity, smoothTime);
    }

    public IEnumerator Shake (float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while( elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z + z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;

    }


}
