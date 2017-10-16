﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropertiesLaserDiagonal : Properties
{
    public float xMovementSpeed;
    public float yMovementSpeed;
    public float yMovementSpeedShot;
    public float destructionMargin;
    public float waveLenght;
    public float amplitude;
    public Transform[] rightTargets;
    public Transform[] leftTargets;
    public float laserHeight;
    public float laserDepth;
    public float speed;
    public float waitingTime;
    public float loadingTime;
    public float shootingTime;
    public GameObject gameObjectPrefab;
    public GameObject laserPrefab;
}