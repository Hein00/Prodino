﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2 funzioni movimento per le 2 modalità
//Sparo
//Melee in top down
[System.Serializable]
public class BoundarySideScroll
{
    public float xMin, xMax, yMin, yMax;
}

[System.Serializable]
public class BoundaryTopDown
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerControllerJump : MonoBehaviour
{
    public Vector3 startPosition;
    public float speed = 5.0f;
    public float jumpForce = 5.0f;
    public float rayLength;
    private float controllerDeadZone = 0.1f;
    [HideInInspector]
    public Transform aimTransform;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRatio = 0.10f;
    private float fireTimer;
    public float timerRespawn = 0.5f;
    public float gravity;
    public float glideSpeed;
    private Quaternion sideScrollerRotation;
    private const string playerBulletTag = "PlayerBullet";
    private RaycastHit hit;
    public float angle;
    public float meleeDistance;
    private Rigidbody rb;
    public LayerMask groundMask;

    public bool canShoot = true;
    public bool canJump = true;
    public bool isDead;

    private SkinnedMeshRenderer skinnedMeshRen;

    [Header("Boundaries")]
    public float sidexMin;
    public float sidexMax, sideyMin, sideyMax;
    public float topxMin, topxMax, topzMin, topzMax;

    [Header("Animation")]
    public Animator ani;
    private float horizontal;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        skinnedMeshRen = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    void Start()
    {
        transform.position = startPosition;
        fireTimer = fireRatio;
        sideScrollerRotation = transform.rotation;
    }

    void Update()
    {
        if (!isDead)
        {
            if (!GameManager.instance.transitionIsRunning)
            {
                canJump = CheckGround();
                switch (GameManager.instance.currentGameMode)
                {
                    case GameMode.SIDESCROLL:
                        if (transform.position.x > sidexMin && Input.GetAxis("Horizontal") < -controllerDeadZone)
                        {
                            Move(Vector3.right, speed, "Horizontal");
                        }
                        else if (transform.position.x < sidexMax && Input.GetAxis("Horizontal") > controllerDeadZone)
                        {
                            Move(Vector3.right, speed, "Horizontal");
                        }
                        if (Input.GetKeyDown(KeyCode.W) && canJump)
                        {
                            canJump = false;
                            Jump();
                        }
                        if (!canJump)
                        {
                            ApplyGravity();
                        }
                        if (Input.GetKey(KeyCode.W))
                        {
                            if (!canJump && rb.velocity.y < 0.0f)
                            {
                                if (rb.velocity.y < -2)
                                {
                                    StabilizeAcceleration();
                                }
                                else
                                {
                                    Glide();
                                }
                            }
                        }
                        if (transform.rotation != sideScrollerRotation)
                        {
                            transform.rotation = sideScrollerRotation;
                        }

                        ClampPosition(GameMode.SIDESCROLL);

                        break;
                    case GameMode.TOPDOWN:
                        Move(Vector3.forward, speed, "Vertical");
                        Move(Vector3.right, speed, "Horizontal");
                        TurnAroundPlayer();

                        ClampPosition(GameMode.TOPDOWN);

                        if (Input.GetMouseButtonDown(1))
                        {
                            StartCoroutine(Melee());
                        }

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
                PlayAnimation();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isDead && (other.gameObject.tag == "Enemy" || other.gameObject.tag == "EnemyBullet"))
        {
            StartCoroutine("BlinkMeshRen");
        }
    }

    void ApplyGravity()
    {
        rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
    }

    void StabilizeAcceleration()
    {
        rb.AddForce(Vector3.up * Mathf.Abs((rb.velocity.y / 3)), ForceMode.Impulse);
    }

    void Jump()
    {
        rb.velocity = new Vector3(0, jumpForce, 0);
    }

    void Glide()
    {
        rb.AddForce(Vector3.up * glideSpeed, ForceMode.Force);
    }

    bool CheckGround()
    {
        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), Vector3.down);
        Ray ray = new Ray(new Vector3 (transform.position.x, transform.position.y + 1, transform.position.z), Vector3.down);
        if (Physics.Raycast(ray, rayLength, groundMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Move(Vector3 moveVector, float speed, string moveAxis)
    {
        if (Input.GetAxis(moveAxis) < -controllerDeadZone || Input.GetAxis(moveAxis) > controllerDeadZone)
        {
            transform.Translate(moveVector * Input.GetAxis(moveAxis) * speed * Time.deltaTime, Space.World);
        }
    }

    void TurnAroundPlayer()
    {
        transform.LookAt(new Vector3(aimTransform.position.x, transform.position.y, aimTransform.position.z));
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as GameObject;
        bullet.tag = playerBulletTag;
    }

    public void ClampPosition(GameMode state)
    {
        switch (state)
        {
            case (GameMode.SIDESCROLL):
                transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, sidexMin, sidexMax),
                Mathf.Clamp(transform.position.y, sideyMin, sideyMax),
                0.0f
                );
                break;
            case (GameMode.TOPDOWN):
                transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, topxMin, topxMax),
                startPosition.y,
                Mathf.Clamp(transform.position.z, topzMin, topzMax)
                );
                break;
        }
    }

    void PlayAnimation()
    {
        horizontal = Input.GetAxis("Horizontal");
        ani.SetFloat("horizontal", horizontal);
    }

    IEnumerator Melee()
    {
        angle = 0;

        while (angle < 180)
        {
            canShoot = false;
            angle += 5;
            Vector3 initDir = -bulletSpawnPoint.forward;
            Quaternion angleQ = Quaternion.AngleAxis(angle, Vector3.up);
            Vector3 newVector = angleQ * initDir;

            Ray ray = new Ray(transform.position, newVector);

            if (Physics.Raycast(ray, out hit, meleeDistance))
            {
                Destroy(hit.transform.gameObject);
            }
            Debug.DrawRay(ray.origin, ray.direction * meleeDistance, Color.magenta);

            yield return null;
        }
        canShoot = true;
    }

    IEnumerator BlinkMeshRen()
    {
        isDead = true;
        skinnedMeshRen.enabled = false;

        yield return new WaitForSeconds(timerRespawn);

        skinnedMeshRen.enabled = true;
        transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        isDead = false;
    }
}