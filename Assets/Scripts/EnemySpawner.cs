using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviourPunCallbacks
{
    private const string ENEMY_PREFAB = "Enemy";
    [SerializeField] private float minSpawnInterval = 0.50f;
    [SerializeField] private float maxSpawnInterval = 2.50f;

    private float spawnInterval;
    private Boundary boundary;
    private Coroutine spawner;

    private void Start(){
        boundary = new();
        boundary.CalculateScreenRestrictions();
    }

    private void Update(){
        // Make sure that only the master client is responsible
        // for spawning enemies
        if (!PhotonNetwork.IsMasterClient) return;
        if(spawner != null){
            return;
        }
        spawner = StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine(){
        spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
        yield return new WaitForSeconds(spawnInterval);
        SpawnEnemy();
        spawner = null;
    }

    private void SpawnEnemy(){
        // Instantiate over the network
        PhotonNetwork.InstantiateRoomObject(ENEMY_PREFAB, GetSpawnPosition(),
            Quaternion.identity);
         //Instantiate(bullet, transform.position, transform.rotation);
        
        /*GameObject enemy = ObjectPoolManager.Instance.GetPooledObject("Enemy");
        if(enemy != null){
            enemy.transform.position = GetSpawnPosition();
            enemy.gameObject.SetActive(true);
        }*/
    }

    private Vector2 GetSpawnPosition(){
        // Generate a random position along an axis
        float xRandomPosition = Random.Range(-boundary.Bounds.x, boundary.Bounds.x);
        float yRandomPosition = Random.Range(-boundary.Bounds.y, boundary.Bounds.y);
        int areaToSpawn = Random.Range(0, 4);
        switch(areaToSpawn){
            // Upper part
            case 0:
                return new Vector2(xRandomPosition, boundary.Bounds.y);
            //Right part
            case 1:
                return new Vector2(boundary.Bounds.x, yRandomPosition);
            // Bottom part
            case 2:
                return new Vector2(xRandomPosition, -boundary.Bounds.y);
            //Left part
            case 3:
                return new Vector2(-boundary.Bounds.x, yRandomPosition);
        }
        return Vector2.zero;
    }
}
