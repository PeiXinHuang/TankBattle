using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


/// <summary>
/// �ͻ�������������,ͨ���������ˢ�����ݣ����пͻ��˶���ͬʱˢ�����ݣ��Դ���ģ�������
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
    /// ��player�Ĺ�ϣ���ж�ȡ����
    /// </summary>
    /// <typeparam name="T">��ȡ����������</typeparam>
    /// <param name="key">��ȡ���ݵļ�</param>
    /// <param name="player">�ͻ�����ң�null��ʾ�������</param>
    /// <returns>��ȡ������</returns>
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
    /// �������ݵ��ͻ��˵Ĺ�ϣ��֮��
    /// </summary>
    /// <typeparam name="T">���õ���������</typeparam>
    /// <param name="key">��ϣ���</param>
    /// <param name="value">��ϣ��ֵ</param>
    /// <param name="player">�ͻ��˵���ң�null��ʾΪ���ؿͻ���</param>
    public void SetData<T>(string key,T value,Player player = null)
    {
        if (player == null)
            player = PhotonNetwork.LocalPlayer;

        Hashtable hashtable = player.CustomProperties;
        hashtable[key] = value;
        player.SetCustomProperties(hashtable);

    }


}
