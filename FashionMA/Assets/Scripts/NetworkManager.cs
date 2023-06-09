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
        Screen.SetResolution(960, 540, false); //창모드
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
        if (PhotonNetwork.CountOfRooms == 0) PhotonNetwork.CreateRoom("1대1뜨실분");
        else PhotonNetwork.JoinRandomRoom();

    }

    public override void OnJoinedRoom()
    {
        loginPanel.SetActive(false);
        PhotonNetwork.Instantiate("MobileMaleFree1", Vector2.zero, Quaternion.identity);
    }

    public void ClickBtn()
    {
        if (nicknameInput.text.Length > 1)
        {
            PhotonNetwork.NickName = nicknameInput.text;
            PhotonNetwork.JoinLobby();
        }
        else titleText.text = "닉네임은 2글자 이상!";
    }

    private void Update()
    {
        if (loginPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            ClickBtn();
        }
    }
}