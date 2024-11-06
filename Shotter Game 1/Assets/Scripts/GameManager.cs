using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] List<Transform> spawns = new List<Transform>();
    [SerializeField] List<Transform> walkSpawns = new List<Transform>();
    [SerializeField] List<Transform> turretSpawns = new List<Transform>();

    [SerializeField] TMP_Text playersText, youWonText;
    GameObject[] players;
    List<string> activePlayers = new List<string>();
    int checkPlayers = 0;
    int prevPlayerCount;

    // Start is called before the first frame update
    void Start()
    {
        int randSpawn = Random.Range(0, spawns.Count);
        PhotonNetwork.Instantiate("Character", spawns[randSpawn].position, spawns[randSpawn].rotation);
        Invoke("SpawnEnemy", 5f);
        prevPlayerCount = PhotonNetwork.PlayerList.Length;
    }

    void Update()
    {
        if (PhotonNetwork.PlayerList.Length < prevPlayerCount)
        {
            ChangePlayerList();
        }
        prevPlayerCount = PhotonNetwork.PlayerList.Length;
    }

    void SpawnEnemy()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        if (walkSpawns.Count > 0)
        {
            for (int i = 0; i < walkSpawns.Count; i++)
            {
                PhotonNetwork.Instantiate("WalkEnemy", walkSpawns[i].position, walkSpawns[i].rotation);
            }
        }

        if (turretSpawns.Count > 0)
        {
            for (int i = 0; i < turretSpawns.Count; i++)
            {
                PhotonNetwork.Instantiate("turret_1_1", turretSpawns[i].position, turretSpawns[i].rotation);
            }
        }
    }

    public void ChangePlayerList()
    {
        photonView.RPC("PlayerList", RpcTarget.All);
    }

    [PunRPC]
    public void PlayerList()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        activePlayers.Clear();

        foreach(GameObject player in players)
        {
            if (player.GetComponent<PlayerController>().health > 0)
            {
                activePlayers.Add(player.GetComponent<PhotonView>().Owner.NickName);
            }
        }

        if (activePlayers.Count <= 1 && checkPlayers > 0)
        {
            // Düşmanları deaktif etme
            //5 saniye sonra oyunu bitir

            /* // YOU WON YAZISI ÇIKMALI
            foreach(var player in players)
            {
                player.GetComponent<PlayerController>().YouWonWrapper(activePlayers[0]);
            } */
            PlayerPrefs.SetString("Winner", activePlayers[0]);
            var enemies = GameObject.FindGameObjectsWithTag("enemy");
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Enemy>().ChangeHealth(1000);
            }
            Invoke("EndGame", 5f);
        }

        checkPlayers++;
        playersText.text = "Players in game: " + activePlayers.Count.ToString();
    }

    void EndGame()
    {
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public void ExitGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        ChangePlayerList();
        SceneManager.LoadScene(0);
    }
}
