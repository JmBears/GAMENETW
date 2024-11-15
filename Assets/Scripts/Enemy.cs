using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] FloatingHealthBar healthBar;

    private float currentHealth, maxHealth;

    private Transform target = null;
    private bool isDestroyed = false;

    private void Awake()
    {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    public override void OnEnable(){
        base.OnEnable();
        LookAtTarget();
        isDestroyed = false;

        maxHealth = Random.Range(50f, 100f);
        currentHealth = maxHealth;
        healthBar.UpdateEnemyHealthBar(currentHealth, maxHealth);
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

        if (collision != null && collision.gameObject != null)
        {
            if (collision.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
            {
                if (bullet != null && bullet.Owner != null)
                {
                    int damagedealt = Random.Range(10, 50);
                    TakeDamage(damagedealt);

                    //Grant score to the player that hits the enemy
                    ScoreManager.Instance.AddScore(damagedealt, bullet.Owner);

                    if (currentHealth <= 0)
                    {
                        //  Whoever got the last hit on the enemy gets the 100 points
                        ScoreManager.Instance.AddScore(100, bullet.Owner);

                        DestroyOverNetwork();
                    }
                }
            }
        }
        
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        //  Reflects the enemy's current HP on the slider
        healthBar.UpdateEnemyHealthBar(currentHealth, maxHealth);
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
