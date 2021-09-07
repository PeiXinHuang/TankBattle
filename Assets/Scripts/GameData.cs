using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��Ϸ���ݣ���ģ������������пͻ��˵Ĺ�ϣ����ɣ��л�ȡ����
/// </summary>
public class GameData
{

    private static GameData instance;
    public  static GameData Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameData();
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
        foreach (PlayerController playerController in GameObject.FindObjectsOfType<PlayerController>())
        {
            Player player = playerController.photonView.Owner;
            PlayerData playerData = new PlayerData();
            playerData.PlayerName = DataConnectMgr.Instance.ReadData<string>("playerName", player);
            playerData.Hp = DataConnectMgr.Instance.ReadData<int>("hp", player);
            playerData.KillNum = DataConnectMgr.Instance.ReadData<int>("killNum", player);
            playerDatas.Add(player.NickName,playerData);
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