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
        Application.runInBackground = true; //֧�ֺ�̨����
    }

    void Start()
    {
        //Srep1: ���ӷ����� 
        PhotonNetwork.ConnectUsingSettings();
    }





    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("���ӵ�������");

        //Step2: �������
        PhotonNetwork.JoinLobby(TypedLobby.Default);



    }

    public override void OnJoinedLobby()
    {
        Debug.Log("�������");
        base.OnJoinedLobby();

        userNameScreen.SetActive(true); //�����������ʾ�û����
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("��������");
        base.OnCreatedRoom();
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.LoadLevel(1); //��������һ������
        Debug.Log("���뷿��");
    }

    /// <summary>
    /// ������ɫ��ť��ť
    /// </summary>
    public void OnClickCreateNameButton()
    {
        //����������Ƶ�PhotonNetwork
        PhotonNetwork.NickName = userNameInput.text;
        userNameScreen.SetActive(false);
        connectScreen.SetActive(true);
    }

    /// <summary>
    /// ��ɫ���Ƴ��ȴ���2ʱ�򣬴�����ɫ��ť����
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
    /// ��������
    /// </summary>
    public void OnClickCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.CreateRoom(createRoomInput.text, roomOptions, TypedLobby.Default);
    }

    /// <summary>
    /// ���뷿��
    /// </summary>
    public void OnClickJoinRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom(joinRoomInput.text, roomOptions, null);
    }
}
