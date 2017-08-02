﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
	public Transform[] enemySpawnPoints;
	public bool canSpawnHorde = true;
	public float enemySpawnDelay;
	public GameObject []enemyPrefab;
    private int enemyType;
	
	// Update is called once per frame
	void Update () {
		if (canSpawnHorde) {
			StartCoroutine (EnemySpawn ());
		}
	}

	IEnumerator EnemySpawn(){
		canSpawnHorde = false;
		yield return new WaitForSeconds (enemySpawnDelay);

		//Spawn dei nemici in ordine di spawn possibile cambiare per farlo essere random o deciso da script senza dover
		//cambiare gli spawnpoint in sceneview
		for (int i = 0; i < enemySpawnPoints.Length; i++) {
            enemyType = Random.Range(0, enemyPrefab.Length);
            if(enemyPrefab[enemyType].layer==10)
            {
                GameObject enemy = Instantiate(enemyPrefab[enemyType], enemySpawnPoints[0].transform.position, enemyPrefab[enemyType].transform.rotation) as GameObject;
            }
            else
            {
                GameObject enemy = Instantiate(enemyPrefab[enemyType], enemySpawnPoints[i].transform.position, enemyPrefab[enemyType].transform.rotation) as GameObject;
            }
		}
		canSpawnHorde = true;
		yield return null;
	}
}
