﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2 funzioni movimento per le 2 modalità
//Sparo
//Melee in top down
[System.Serializable]
public class BoundarySideScroll {
	public float xMin, xMax, yMin, yMax;
}

[System.Serializable]
public class BoundaryTopDown {
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
	public BoundarySideScroll boundarySideScroll;
	public BoundaryTopDown boundaryTopDown;

    public float speed = 5.0f;
    private float controllerDeadZone = 0.1f;
    public Transform aim;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRatio = 0.10f;
	private float fireTimer;
    public CameraController cameraInstance;
    private Quaternion sideScrollerRotation;
	private const string playerBulletTag = "PlayerBullet";
	private RaycastHit hit;
	public float angle;
	public float meleeDistance;

	public bool canShoot = true;

    void Start()
    {
		
        fireTimer = fireRatio;
        sideScrollerRotation = transform.rotation;
    }

    void Update()
    {
        if (!cameraInstance.isLerpingCamera)
        {
            switch (cameraInstance.myState)
            {
			case CameraState.SIDESCROLL:
				Move (Vector3.up, "Vertical");
				Move (Vector3.right, "Horizontal");
				transform.rotation = sideScrollerRotation;
					
				transform.position = new Vector3 (
					Mathf.Clamp (transform.position.x, boundarySideScroll.xMin, boundarySideScroll.xMax), 
					Mathf.Clamp (transform.position.y, boundarySideScroll.yMin, boundarySideScroll.yMax),
					0.0f
				);

                    break;
                case CameraState.TOPDOWN:
                    Move(Vector3.forward, "Vertical");
                    Move(Vector3.right, "Horizontal");
                    TurnAroundPlayer();

					transform.position = new Vector3 (
						Mathf.Clamp (transform.position.x, boundaryTopDown.xMin, boundaryTopDown.xMax), 
						0.0f,
						Mathf.Clamp (transform.position.z, boundaryTopDown.zMin, boundaryTopDown.zMax)
					);

                    break;
            }
			if (Input.GetMouseButton(0) && canShoot)
            {
                if (fireTimer < fireRatio)
                {
                    fireTimer += Time.deltaTime;
                }
                else
                {
                    Shoot();
                    fireTimer = 0.00f;
                }
            }

			if (Input.GetMouseButtonDown (1)) {
				StartCoroutine (Melee ());
			}
        }
    }

    void Move(Vector3 moveVector, string axis)
    {
        if (Input.GetAxis(axis) < -controllerDeadZone || Input.GetAxis(axis) > controllerDeadZone)
        {
            transform.Translate(moveVector * Input.GetAxis(axis) * speed * Time.deltaTime, Space.World);
        }
    }

    void TurnAroundPlayer()
    {
        
        transform.LookAt(new Vector3(aim.position.x, transform.position.y, aim.position.z));
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation) as GameObject;
		bullet.tag = playerBulletTag;
    }

	IEnumerator Melee(){
		angle = 0;

		while (angle < 180) {
			canShoot = false;
			angle += 5;
			Vector3 initDir = -bulletSpawnPoint.right;
			Quaternion angleQ = Quaternion.AngleAxis(angle, Vector3.up);
			Vector3 newVector = angleQ * initDir;

			Ray ray = new Ray (transform.position, newVector);

			if (Physics.Raycast (ray, out hit, meleeDistance)) {
				Destroy (hit.transform.gameObject);
			}
			Debug.DrawRay (ray.origin, ray.direction * meleeDistance, Color.magenta);

			yield return null;
		}  
		canShoot = true;
	}


}
