using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ��Ϸ���ݣ���ģ������������пͻ��˵Ĺ�ϣ����ɣ��л�ȡ����
/// </summary>
public class GameData:MonoBehaviourPun
{

    private static GameData instance;
    public  static GameData Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<GameData>();
            }
            return instance;
        }
    }

    private Dictionary<string,PlayerData> playerDatas = new Dictionary<string, PlayerData>(); //��������б�


    /// <summary>
    /// ��������
    /// </summary>
    public void UpdateData()
    {
        //�������
        playerDatas.Clear();


        //�������
        PlayerData playerData;
        foreach (PlayerController playerController in GameObject.FindObjectsOfType<PlayerController>())
        {
            Player player = playerController.photonView.Owner;
            playerData = new PlayerData();
            playerData.PlayerName = DataConnectMgr.Instance.ReadData<string>("playerName", player);
            playerData.Hp = DataConnectMgr.Instance.ReadData<int>("hp", player);
            playerData.KillNum = DataConnectMgr.Instance.ReadData<int>("killNum", player);
            playerDatas.Add(player.NickName,playerData);
        }

        playerData = ReadData();
        if (BeKillAction != null && playerData.Hp <= 0)
        {
            BeKillAction();
        }
    }


    /// <summary>
    /// �������ȥ��ȡ����
    /// </summary>
    /// <param name="player">�ͻ�����ң�null��ʾ�������</param>
    /// <returns>��ȡ������</returns>
    public PlayerData ReadData(Player player = null)
    {
        if (player == null)
            player = PhotonNetwork.LocalPlayer;

        PlayerData playerData;
        if (playerDatas.ContainsKey(player.NickName))
        {
            playerData = playerDatas[player.NickName];
        }
        else
        {
            Debug.LogError("PlayerDatas don't contain player:" + player.NickName);
            return null;
        }
        return playerData;
    }

    private event UnityAction BeKillAction;
    public void AddBeKillListen(UnityAction fun)
    {
        BeKillAction += fun;
    }
    public void RemoveBeKillListen(UnityAction fun)
    {
        BeKillAction -= fun;
    }


}

/// <summary>
/// �������
/// </summary>
public class PlayerData
{
    //�������
    private string playerName;
    public string PlayerName { get => playerName; set => playerName = value; }

    //Ѫ��ֵ
    private int hp = 100;
    public int Hp { get => hp; set => hp = value; }

    //��ɱ��
    private int killNum = 0;
    public int KillNum { get => killNum; set => killNum = value; }
}