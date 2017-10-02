﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public SphereCollider explosionCollider;
    public Properties properties;

    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag =="Env")
        {
            explosionCollider.enabled = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            Destroy(gameObject, properties.b_lifeTime);
        }

        if(other.gameObject.tag =="Player")
        {
            Destroy(other.gameObject);
            Destroy(gameObject, properties.b_lifeTime);
        }
    }
	
}
