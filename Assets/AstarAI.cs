﻿using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;
public class AstarAI : MonoBehaviour
{
    //The point to move to
    public Vector3 targetPosition;

    private Seeker seeker;
    private CharacterController controller;

    //The calculated path
    public Path path;

    //The AI's speed per second
    public float speed = 10;

    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWaypointDistance = 3;

    //The waypoint we are currently moving towards
    private int currentWaypoint = 0;

    public float repathRate = 0.5f;
    private float lastRepath = -9999;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        controller = GetComponent<CharacterController>();

        //Start a new path to the targetPosition, return the result to the OnPathComplete function
        //seeker.StartPath (transform.position,targetPosition, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        p.Claim(this);
        if (!p.error)
        {
            if (path != null) path.Release(this);
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
        }
        else {
            p.Release(this);
            Debug.Log("Oh noes, the target was not reachable: " + p.errorLog);
        }

        //seeker.StartPath (transform.position,targetPosition, OnPathComplete);
    }

    public void Update()
    {
        PointOfClick();
        if (Time.time - lastRepath > repathRate && seeker.IsDone())
        {
            lastRepath = Time.time + Random.value * repathRate * 0.5f;
            seeker.StartPath(transform.position, targetPosition, OnPathComplete);
        }

        if (path == null)
        {
            //We have no path to move after yet
            return;
        }

        if (currentWaypoint > path.vectorPath.Count) return;
        if (currentWaypoint == path.vectorPath.Count)
        {
            Debug.Log("End Of Path Reached");
            currentWaypoint++;
            return;
        }

        //Direction to the next waypoint
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed;// * Time.deltaTime;
        //transform.Translate (dir);
        controller.SimpleMove(dir);

        //if (Vector3.Distance (transform.position,path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
        if ((transform.position - path.vectorPath[currentWaypoint]).sqrMagnitude < nextWaypointDistance * nextWaypointDistance)
        {
            currentWaypoint++;
            return;
        }
    }

    public void PointOfClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {

                if (hit.collider.tag != "Wall")
                {
                    targetPosition = hit.point;
                }
            }

        }
        
        
    }

    public void PointOfTouch()
    {

    }
}