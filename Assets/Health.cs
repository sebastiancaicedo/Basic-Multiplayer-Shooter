using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour {

    public const int MAX_HEALTH = 100;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = MAX_HEALTH;
    public Slider healtBar;
    public bool destroyOnDeath;

    private NetworkStartPosition[] spawnPoints;

    private void Start()
    {
        if (!isLocalPlayer) return;

        spawnPoints = FindObjectsOfType<NetworkStartPosition>();
    }

    public void TakeDamage(int amount)
    {
        if (!isServer) return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = MAX_HEALTH;

                // called on the Server, but invoked on the Clients
                RpcRespawn();
            }
        }
    }

    //Hook
    void OnChangeHealth(int currentHealth)
    {
        print(currentHealth);
        healtBar.value = (float)currentHealth / MAX_HEALTH;
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
            Vector3 spawnPoint = Vector3.zero;

            if(spawnPoints != null && spawnPoints.Length > 0)
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;


            transform.position = spawnPoint;
        }
    }

    public void RestoreHealth(int restoreAmount)
    {
        if (!isServer) return;

        if (currentHealth + restoreAmount > MAX_HEALTH)
        {
            currentHealth = MAX_HEALTH;
        }
        else
        {
            currentHealth += restoreAmount;
        }
    }

}
