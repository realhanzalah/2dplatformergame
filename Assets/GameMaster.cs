using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
	public static GameMaster gm;

    void Start()
    {
        if(gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;

    //Creating a delay between death and respawn
    public int spawningDelay = 3;

    public IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(spawningDelay);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
    
    public static void KillPlayer(Player player)
	{
		Destroy(player.gameObject);
        gm.StartCoroutine (gm.RespawnPlayer());
	}

    public static void KillEnemy(Enemy enemy)
    {
        Destroy(enemy.gameObject);
    }
}
