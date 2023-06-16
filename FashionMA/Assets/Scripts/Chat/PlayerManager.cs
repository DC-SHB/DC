using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView myView;
    public Text nickname;
    public float speed = 2.0f;
    public GameObject chatBox;
    public Text chatBoxText;
    private GameObject chatPanel;
    private Button chatBtn, closeBtn, sendBtn;
    private EventTrigger rightBtn, leftBtn;
    private InputField chatInput;
    private bool rightMove, leftMove;

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

        chatPanel = GameObject.Find("Canvas").transform.GetChild(2).gameObject;
        chatInput = chatPanel.transform.GetChild(0).GetComponent<InputField>();
        closeBtn = chatPanel.transform.GetChild(1).GetComponent<Button>();
        closeBtn.onClick.AddListener(CloseBtnEvent);
        sendBtn = chatPanel.transform.GetChild(2).GetComponent<Button>();
        sendBtn.onClick.AddListener(ChatBtnEvent);

        GameObject btnGroup = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        chatBtn = btnGroup.transform.GetChild(0).transform.GetComponent<Button>();
        rightBtn = btnGroup.transform.GetChild(1).transform.GetComponent<EventTrigger>();
        leftBtn = btnGroup.transform.GetChild(2).transform.GetComponent<EventTrigger>();

        chatBtn.onClick.AddListener(ChatBtnEvent);
        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        rightBtn.triggers.Add(pointerDown);
        leftBtn.triggers.Add(pointerDown);
        EventTrigger.Entry pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        rightBtn.triggers.Add(pointerUp);
        leftBtn.triggers.Add(pointerUp);

    }

    private void OnPointerUp(PointerEventData data)
    {
        if (photonView.IsMine)
        {
            if (data.selectedObject.name == "rightBtn") rightMove = false;
            if (data.selectedObject.name == "leftBtn") leftMove = false;
        }
    }

    private void OnPointerDown(PointerEventData data)
    {
        if(photonView.IsMine)
        {
            if (data.selectedObject.name == "rightBtn") rightMove = true;
            if (data.selectedObject.name == "leftBtn") leftMove = true;
        }
    }

    private void Update()
    {
        if (photonView.IsMine && !chatPanel.activeSelf)
        {
            float x = Input.GetAxisRaw("Horizontal");
            transform.Translate(-x * Time.deltaTime, 0, 0);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChatBtnEvent();
        }

        if (rightMove) RightMoveEvent();
        if (leftMove) LeftMoveEvent();
    }

    private void ChatBtnEvent()
    {
        if(photonView.IsMine)
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

    private void CloseBtnEvent()
    {
        if (photonView.IsMine) chatPanel.SetActive(false);
    }

    private void RightMoveEvent()
    {
        if (photonView.IsMine)
        {
            transform.Translate(-1.0f * Time.deltaTime, 0, 0);
            float x = Mathf.Clamp(transform.position.x, -1.02f, 1.02f);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
    }

    private void LeftMoveEvent()
    {
        if (photonView.IsMine)
        {
            transform.Translate(1.0f * Time.deltaTime, 0, 0);
            float x = Mathf.Clamp(transform.position.x, -1.02f, 1.02f);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
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
