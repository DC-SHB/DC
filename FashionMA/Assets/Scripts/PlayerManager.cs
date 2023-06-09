using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView myView;
    public Text myNick;
    public float speed = 2.0f;
    public GameObject ChatBox;
    public Text ChatBox_Text;
    private GameObject MainChatPanel;
    private InputField Chat_Input;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            myNick.text = PhotonNetwork.NickName;
            myNick.color = Color.white;
        }
        else
        {
            myNick.text = photonView.Owner.NickName;
            myNick.color = Color.red;
        }

        MainChatPanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        Chat_Input = MainChatPanel.transform.GetChild(0).GetComponent<InputField>();
    }

    private void Update()
    {
        if (photonView.IsMine && !MainChatPanel.activeSelf)
        {
            float x = Input.GetAxisRaw("Horizontal");
            transform.Translate(-x * Time.deltaTime, 0, 0);
        }


        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Return))
        {
            if (MainChatPanel.activeSelf)
            {
                MainChatPanel.SetActive(false);
                if (Chat_Input.text.Length > 0)
                {
                    myView.RPC("OpenChatBox", RpcTarget.AllBuffered);
                    Chat_Input.text = "";
                    StopAllCoroutines();
                    StartCoroutine(DelayCloseChatBox(3.0f));
                }
            }
            else
            {
                MainChatPanel.SetActive(true);
                Chat_Input.ActivateInputField();
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
        ChatBox.SetActive(true);
        ChatBox_Text.text = "<color=red>" + PhotonNetwork.NickName + "</color>" + " :\n" + Chat_Input.text;
    }

    [PunRPC]
    public void CloseChatBox()
    {
        ChatBox.SetActive(false);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ChatBox_Text.text);
        }
        else
        {
            ChatBox_Text.text = (string)stream.ReceiveNext();
        }
    }
}
