﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropertiesDoubleAiming : Properties
{
    public float xMovementSpeed;
    public float zMovementSpeed;
    public float destructionMargin;
    public float waveLenght;
    public float amplitude;
    ////not used
    //public Transform[] rightTargets;
    ////not used
    //public Transform[] leftTargets;
    public float xBulletSpeed;
    public float zBulletSpeed;
    public float fireRate;
    public float bulletAmplitude;
    public GameObject gameObjectPrefab;
    public GameObject bulletPrefab;
    public GameObject doubleAimingBulletPrefab;
}