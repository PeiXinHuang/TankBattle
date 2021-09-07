using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏数据，从模拟服务器（所有客户端的哈希表组成）中获取数据
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

    private Dictionary<string,PlayerData> playerDatas = new Dictionary<string, PlayerData>(); //玩家数据列表


    /// <summary>
    /// 更新数据
    /// </summary>
    public void UpdateData()
    {
        //清空数据
        playerDatas.Clear();


        //添加数据
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
    /// 根据玩家去获取数据
    /// </summary>
    /// <param name="player">客户端玩家，null表示本地玩家</param>
    /// <returns>读取的数据</returns>
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
/// 玩家数据
/// </summary>
public class PlayerData
{
    //玩家名称
    private string playerName;
    public string PlayerName { get => playerName; set => playerName = value; }

    //血量值
    private int hp = 100;
    public int Hp { get => hp; set => hp = value; }

    //击杀数
    private int killNum = 0;
    public int KillNum { get => killNum; set => killNum = value; }
}