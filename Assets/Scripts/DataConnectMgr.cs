using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/// <summary>
/// 客户端数据连接器,通过这个类来刷新数据，所有客户端都会同时刷新数据，以此来模拟服务器
/// </summary>

public class DataConnectMgr : MonoBehaviourPun
{


    private static DataConnectMgr instance;
    public static DataConnectMgr Instance {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<DataConnectMgr>();
                instance.Init();
            }
            return instance;
        }
    
    }

    private void Start()
    {
        Init();
       
    }

    private void Init()
    {
        SetData<string>("playerName", PhotonNetwork.NickName);
    }

    /// <summary>
    /// 从player的哈希表中读取数据
    /// </summary>
    /// <typeparam name="T">读取的数据类型</typeparam>
    /// <param name="key">读取数据的键</param>
    /// <param name="player">客户端玩家，null表示本地玩家</param>
    /// <returns>读取的数据</returns>
    public T ReadData<T>(string key,Player player = null)
    {
        if(player == null)
            player = PhotonNetwork.LocalPlayer;

        T value;
        Hashtable dictionaries = player.CustomProperties;

        if (dictionaries.ContainsKey(key))
            value = (T)dictionaries[key];
        else
        {
            Debug.LogError(player.NickName + " dose not contain key: " + key);
            return default(T);
        }
        return value;
    }


    /// <summary>
    /// 设置数据到客户端的哈希表之中
    /// </summary>
    /// <typeparam name="T">设置的数据类型</typeparam>
    /// <param name="key">哈希表键</param>
    /// <param name="value">哈希表值</param>
    /// <param name="player">客户端的玩家，null表示为本地客户端</param>
    public void SetData<T>(string key,T value,Player player = null)
    {
        if (player == null)
            player = PhotonNetwork.LocalPlayer;

        Hashtable hashtable = player.CustomProperties;
        hashtable[key] = value;
        player.SetCustomProperties(hashtable);

    }


}
