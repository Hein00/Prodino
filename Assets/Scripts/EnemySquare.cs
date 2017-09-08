﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySquare : Enemy {
    //public float length;
    //public float distance;
    //   private Vector3 offset;
    //   //public float lifeTime = 5;
    //   private float timer = 0.0f;
    //   public float maxTimer = 2.0f;
    //   private float angleSquare = 0;
    //   private bool straightwayIsRunning=false;
    [HideInInspector]
    public Vector3[] moveVectors;
    public Vector3? actualDirection;
    public float pathLength;
    private float pathDone;
    public float stopTime;
    private float stopTimer;
    private int pathIndex;
    //private bool isFollowingPath;


 //   void Start()
	//{
	//	//length = Camera.main.orthographicSize;
	//	offset = transform.position;
	//	//Destroy(gameObject, lifeTime);
	//}

    protected override void Move()
    {
        if (actualDirection != null && pathDone < pathLength)
        {
            transform.Translate(actualDirection.Value * speed * Time.deltaTime, Space.World);
            pathDone += speed * Time.deltaTime;
        }
        else
        {
            if (actualDirection != null && stopTimer < stopTime)
            {
                stopTimer += Time.deltaTime;
            }
            else
            {
                actualDirection = moveVectors[pathIndex];
                if (pathIndex < moveVectors.Length - 1)
                {
                    pathIndex++;
                }
                else
                {
                    pathIndex = 0;
                }
                pathDone = 0.0f;
                stopTimer = 0.0f;
            }
        }

        //if (!isFollowingPath)
        //{
        //    isFollowingPath = true;
        //    FollowPath();
        //}
    }

    //IEnumerator FollowPath()
    //{
    //    if (actualDirection != null)
    //    {
    //        while (pathDone < pathLength)
    //        {
    //            transform.Translate(actualDirection.Value * speed * Time.deltaTime, Space.World);
    //            pathDone += speed * Time.deltaTime;
    //            yield return null;
    //        }
    //        yield return new WaitForSeconds(stopTime);
    //        actualDirection = null;
    //    }
    //    else
    //    {
    //        actualDirection = directions[pathIndex];
    //        pathIndex++;
    //    }
    //}

	//protected override void Move()
	//{
	//	switch (GameManager.instance.cameraState)
	//	{
	//	case State.SIDESCROLL:
 //               //if(!straightwayIsRunning)
 //               //{
 //               //    straightwayIsRunning = true;
 //               //    StartCoroutine("STRAIGHTWAY");
 //               //}
 //               //float x = (length * Mathf.Cos(Time.time * speed) / Mathf.Max(Mathf.Abs(Mathf.Sin(Time.time * speed)), Mathf.Abs(Mathf.Cos(Time.time * speed))) + offset.x);
 //               //transform.position = new Vector3(x, transform.position.y, 0);
 //               //distance += x;
 //               transform.position = new Vector3(
 //                   length* Mathf.Cos(Time.time * speed) / Mathf.Max(Mathf.Abs(Mathf.Sin(Time.time * speed)),Mathf.Abs(Mathf.Cos(Time.time * speed))) + offset.x, 
 //                   length* Mathf.Sin(Time.time * speed)/ Mathf.Max(Mathf.Abs(Mathf.Cos(Time.time * speed)),Mathf.Abs(Mathf.Sin(Time.time * speed))) + offset.y, 
 //                   0);
 //               break;
	//	case State.TOPDOWN:
	//		//transform.position = new Vector3(length * Mathf.Cos(Time.time * speed) + offset.x, 0, length * Mathf.Sin(Time.time * speed) + offset.z);
	//		break;
	//	}
	//}

 //   IEnumerator STRAIGHTWAY()
 //   {
 //           float x = (length * Mathf.Cos(Time.time * speed) / Mathf.Max(Mathf.Abs(Mathf.Sin(Time.time * speed)), Mathf.Abs(Mathf.Cos(Time.time * speed))) + offset.x);
 //           transform.position = new Vector3(x, transform.position.y, 0);
 //           distance += x;
 //           yield return null;
 //   }
}
