using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviourPunCallbacks
{
    [SerializeField] private float speed;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private float bulletDamage;
    private Player owner;

    private Rigidbody2D rb;
    private Boundary boundary;
    private bool isDestroyed = false;

    public Player Owner => owner;
    public float Damage => bulletDamage;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
        boundary = new Boundary();
        boundary.CalculateScreenRestrictions();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InitializeValues(float damage, Player owner)
    {
        this.bulletDamage = damage;
        this.owner = owner;

        //  For every newly instantiated bullet to assign sprite
        AssignSprite();
    }

    public override void OnEnable(){
        // the object is already positioned from the instantiation, simply move it on y-axis
        rb.velocity = transform.up * speed;
        isDestroyed = false;

        PlayerNumbering.OnPlayerNumberingChanged += AssignSprite;
        if (!photonView.IsMine) return;
    }

    public override void OnDisable()
    {
        owner = null;
        PlayerNumbering.OnPlayerNumberingChanged -= AssignSprite;
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

    public void AssignSprite()
    {
        photonView.RPC("RPCAssignSprite", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void RPCAssignSprite()
    {
        // GetPlayerNumber is only accessible through Photon.Pun.UtilityScripts
        spriteRenderer.sprite = NetworkManager.Instance.GetPlayerBullet(photonView.Owner.GetPlayerNumber());
    }
}
