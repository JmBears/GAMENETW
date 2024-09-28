using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviourPunCallbacks
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private Boundary boundary;
    private bool isDestroyed = false;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        boundary = new Boundary();
        boundary.CalculateScreenRestrictions();
    }

    public override void OnEnable(){
        // the object is already positioned from the instantiation, simply move it on y-axis
        rb.velocity = transform.up * speed;
        isDestroyed = false;
    }

    private void Update()
    {
        if (isDestroyed) return;
        CheckIfOutOfBounds();
    }

    private void CheckIfOutOfBounds()
    {
        if(transform.position.x > boundary.Bounds.x ||
            transform.position.y > boundary.Bounds.y ||
            transform.position.x < -boundary.Bounds.x ||
            transform.position.y < -boundary.Bounds.y)
        {
            DestroyOverNetwork();
        }
    }

    public void DestroyOverNetwork()
    {
        // Only the player that spawned the object can destroy it
        // Because the bullet is spawned by the player
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
        else
        {
            isDestroyed = true;
        }
    }
}
