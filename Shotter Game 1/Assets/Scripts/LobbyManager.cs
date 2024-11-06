using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text ChatText;
    [SerializeField] TMP_InputField InputText;
    [SerializeField] GameObject startButton;
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(false);
        }
        // PlayerPrefs'te "Winner" anahtarı altında kaydedilmiş bir değerimiz varsa ve oyuncu master clientsa
        if (PlayerPrefs.HasKey("Winner") && PhotonNetwork.IsMasterClient)
        {
            // Kazananın takma adını saklayacak geçici bir değişken oluşturma
            string winner = PlayerPrefs.GetString("Winner");
            // Son maçı kazanan oyuncunun adını görüntülemek için sohbet mesajı fonksiyonunu çağırma
            photonView.RPC("ShowMessage", RpcTarget.All, "The last match was won by: " + winner);
            // Aynı mesajın tekrarlanmaması için PlayerPrefs'ten her şeyi silme
            PlayerPrefs.DeleteAll();
        }
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Log(string msg)
    {
        ChatText.text += msg + "\n";
        // \n alt satıra geçmesini sağlar
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Log(otherPlayer.NickName + " odayı terk etti");
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.SetActive(true);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Log(newPlayer.NickName + " odaya hoş geldi");
    }

    [PunRPC]
    public void ShowMessage(string msg)
    {
        Log(msg);
    }

    public void Send()
    {
        if (string.IsNullOrWhiteSpace(InputText.text)) { return; }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            photonView.RPC("ShowMessage", RpcTarget.All, PhotonNetwork.NickName + ": " + InputText.text);
            InputText.text = string.Empty;
        }

    }
}
