using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField] private float moveSpeed = 5.0f;

    private Transform target = null;
    private bool isDestroyed = false;
    public override void OnEnable(){
        base.OnEnable();
        LookAtTarget();
        isDestroyed = false;
    }

    public void SetTarget(Transform target){
        this.target = target;
    }

    private void Update(){
        if (isDestroyed) return;
        Move();
    }
    
    private void LookAtTarget(){
        Quaternion newRotation;
        Vector3 targetDirection = target == null ? transform.position : transform.position - target.transform.position;
        newRotation = Quaternion.LookRotation(targetDirection, Vector3.forward);
        newRotation.x = 0;
        newRotation.y = 0;
        transform.rotation = newRotation;
    }

    private void Move(){
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestroyed) return;
        if (collision.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
        {
            //Grant score to the player that destroyed the enemy
            ScoreManager.Instance.AddScore(100, bullet.Owner);
            //Got hit by bullet
            DestroyOverNetwork();
        }
    }

    public void DestroyOverNetwork()
    {
        // Since the enemy is Instantiated by the Master Client
        // Only the master client has authority over the object
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        // If we're not the master client,
        // We simply set the boolean flag
        else
        {
            isDestroyed = true;
        }
    }
}
