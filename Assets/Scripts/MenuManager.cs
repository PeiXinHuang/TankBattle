using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject userNameScreen, connectScreen;

    [SerializeField]
    private InputField userNameInput, createRoomInput, joinRoomInput;

    [SerializeField]
    private Button createCharacterButton;

    private void Awake()
    {
        Application.runInBackground = true; //支持后台运行
    }

    void Start()
    {
        //Srep1: 连接服务器 
        PhotonNetwork.ConnectUsingSettings();
    }





    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("连接到服务器");

        //Step2: 加入大厅
        PhotonNetwork.JoinLobby(TypedLobby.Default);



    }

    public override void OnJoinedLobby()
    {
        Debug.Log("加入大厅");
        base.OnJoinedLobby();

        userNameScreen.SetActive(true); //加入大厅后显示用户面板
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("创建房间");
        base.OnCreatedRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(1); //加载另外一个场景
        Debug.Log("加入房间");
    }

    /// <summary>
    /// 创建角色按钮按钮
    /// </summary>
    public void OnClickCreateNameButton()
    {
        //设置玩家名称到PhotonNetwork
        PhotonNetwork.NickName = userNameInput.text;
        userNameScreen.SetActive(false);
        connectScreen.SetActive(true);
    }

    /// <summary>
    /// 角色名称长度大于2时候，创建角色按钮启用
    /// </summary>
    public void OnNameFieldChanged()
    {
        if(userNameInput.text.Length >= 2)
        {
            createCharacterButton.gameObject.SetActive(true);
        }
        else
        {
            createCharacterButton.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 创建房间
    /// </summary>
    public void OnClickCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.CreateRoom(createRoomInput.text, roomOptions, TypedLobby.Default);
    }

    /// <summary>
    /// 加入房间
    /// </summary>
    public void OnClickJoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom(joinRoomInput.text, roomOptions, null);
    }
}
