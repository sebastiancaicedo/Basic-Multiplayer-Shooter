using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HealthPotion : NetworkBehaviour {

    public int restoreAmount = 30;

    [SyncVar]
    private bool wasCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;

        print("Entro");
        if(other.transform.root.tag == "Player")
        {
            if (!wasCollected)
            {
                print("recoge pocion");
                wasCollected = true;
                print(other.name);
                other.transform.root.GetComponent<Health>().RestoreHealth(restoreAmount);
                NetworkServer.Destroy(gameObject);
            }
        }
    }
}
