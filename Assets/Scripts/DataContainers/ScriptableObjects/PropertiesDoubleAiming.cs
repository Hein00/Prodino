﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PropertiesDoubleAiming : Properties
{
    public float xMovementSpeed;
    public float zMovementSpeed;
    public float destructionMargin;
    public Transform[] rightTargets;
    public Transform[] leftTargets;
    public float bulletSpeed;
    public float fireRate;
    public GameObject gameObjectPrefab;
    public GameObject bulletPrefab;
}
