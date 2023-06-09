using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text titleText;
    public InputField nicknameInput;
    public GameObject loginPanel;
    public GameObject beforeConnect;
    public GameObject afterConnect;

    private void Awake()
    {
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }

    private IEnumerator Start()
    {
        yield return PhotonNetwork.ConnectUsingSettings();
        if (PhotonNetwork.IsConnected)
        {
            beforeConnect.SetActive(false);
            afterConnect.SetActive(true);
        }
        else
        {
            Application.Quit();
        }
    }

    public override void OnJoinedLobby()
    {
        if (PhotonNetwork.CountOfRooms == 0) PhotonNetwork.CreateRoom("room1");
        else PhotonNetwork.JoinRandomRoom();

    }

    public override void OnJoinedRoom()
    {
        loginPanel.SetActive(false);
        Vector3 pos = new Vector3(0, -4.3f, -8.56f);
        Quaternion lot = Quaternion.identity;
        lot.eulerAngles = new Vector3(0, 180, 0);
        PhotonNetwork.Instantiate("MobileMaleFree1", pos, lot);
    }

    public void ClickBtn()
    {
        if (nicknameInput.text.Length > 1)
        {
            PhotonNetwork.NickName = nicknameInput.text;
            PhotonNetwork.JoinLobby();
        }
    }

    private void Update()
    {
        if (loginPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            ClickBtn();
        }
    }
}