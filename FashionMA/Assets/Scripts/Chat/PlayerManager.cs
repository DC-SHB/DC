using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView myView;
    public Text nickname;
    public float speed = 2.0f;
    public GameObject chatBox;
    public Text chatBoxText;
    private GameObject chatPanel;
    private InputField chatInput;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            nickname.text = PhotonNetwork.NickName;
            nickname.color = Color.white;
        }
        else
        {
            nickname.text = photonView.Owner.NickName;
            nickname.color = Color.red;
        }

        chatPanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        chatInput = chatPanel.transform.GetChild(0).GetComponent<InputField>();
    }

    private void Update()
    {
        if (photonView.IsMine && !chatPanel.activeSelf)
        {
            float x = Input.GetAxisRaw("Horizontal");
            transform.Translate(-x * Time.deltaTime, 0, 0);
        }

        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Return))
        {
            if (chatPanel.activeSelf)
            {
                chatPanel.SetActive(false);
                if (chatInput.text.Length > 0)
                {
                    myView.RPC("OpenChatBox", RpcTarget.AllBuffered);
                    chatInput.text = "";
                    StopAllCoroutines();
                    StartCoroutine(DelayCloseChatBox(3.0f));
                }
            }
            else
            {
                chatPanel.SetActive(true);
                chatInput.ActivateInputField();
            }
        }
    }

    IEnumerator DelayCloseChatBox(float t)
    {
        yield return new WaitForSeconds(t);
        myView.RPC("CloseChatBox", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void OpenChatBox()
    {
        nickname.enabled = false;
        chatBox.SetActive(true);
        chatBoxText.text = "<color=red>[" + PhotonNetwork.NickName + "]</color>" + "\n" + chatInput.text;
    }

    [PunRPC]
    public void CloseChatBox()
    {
        nickname.enabled = true;
        chatBox.SetActive(false);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(chatBoxText.text);
        }
        else
        {
            chatBoxText.text = (string)stream.ReceiveNext();
        }
    }
}
