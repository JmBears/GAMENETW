using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviourPunCallbacks
{
    [SerializeField] StaticHealthBar healthBar;
    [SerializeField] GameObject gameOver;
    [SerializeField] Leaderboard leaderboard;

    private float currentHealth, maxHealth;
    private bool isGameOver = false;

    private void Awake()
    {
        healthBar = GameObject.Find("PlanetHealthBar").GetComponent<StaticHealthBar>();
    }

    private void Start()
    {
        maxHealth = 1000f;
        currentHealth = maxHealth;
        healthBar.UpdatePlanetHealthBar(currentHealth, maxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            float EnemyHP = 100f;
            TakeDamage(EnemyHP);
        }
    }

    public void TakeDamage(float damage)
    {
        photonView.RPC("RPCTakeDamage", RpcTarget.AllBuffered, damage);

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    [PunRPC]
    private void RPCTakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        //  Reflects the planet's current HP on the slider
        healthBar.UpdatePlanetHealthBar(currentHealth, maxHealth);
    }
    private void GameOver()
    {
        photonView.RPC("RPCGameOver", RpcTarget.AllBuffered);
    }

    [PunRPC]

    private void RPCGameOver()
    {
        Time.timeScale = 0;
        gameOver.SetActive(true);
        leaderboard.Refresh();
    }



    public void DestroyOverNetwork()
    {
        // Since the enemy is Instantiated by the Master Client
        // Only the master client has authority over the object
        if (PhotonNetwork.IsMasterClient)
        {
            //  NOTHING
        }
        // If we're not the master client,
        // We simply set the boolean flag
        else
        {
            isGameOver = true;
        }
    }
}

