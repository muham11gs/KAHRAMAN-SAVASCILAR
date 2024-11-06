using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text logText;
    [SerializeField] TMP_InputField inputField;

    // Start is called before the first frame update
    void Start()
    {
        // Player_123 ~9998
        PhotonNetwork.NickName = "Arcade_" + Random.Range(1000, 10000);
        Log("Oyuncunun adı: " + PhotonNetwork.NickName);
        PhotonNetwork.AutomaticallySyncScene = true; // Pencereler arasındaki otomatik geçiş
        PhotonNetwork.GameVersion = "1"; // Oyun versiyonunu ayarlama
        PhotonNetwork.ConnectUsingSettings(); // Photon sunucusuna bağlanma
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Log(string msg)
    {
        logText.text += msg + "\n";
        // \n alt satıra geçmesini sağlar
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 15 });
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        Log("Server'a bağlandı");
    }

    public override void OnJoinedRoom()
    {
        Log("Odaya katıldın");
        // lobi sahnesini yükle
        PhotonNetwork.LoadLevel("LobbyScene");
    }

    public void ChangeName()
    {
        //InputField alanına yazılan yazıyı okumak
        if (inputField != null)
        {
            PhotonNetwork.NickName = inputField.text;
            //Yeni kullanıcı adını çıktı vermek:
            Log("New Player name: " + PhotonNetwork.NickName);
        }
    }
}
