using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject sendDataObj; //发送数据预制体

    public GameObject playPrefab; //角色预制体

    public bool isBeginPlay = false; //是否开始游戏

    private void Awake()
    {
        UIManager.Instance.SetBeginPlayScreen(true);
    }


    private void Start()
    {
        //开始游戏，输入数据到服务器
        DataConnectMgr.Instance.SetData<string>("playerName", PhotonNetwork.NickName);
        DataConnectMgr.Instance.SetData<int>("hp", 100);
        DataConnectMgr.Instance.SetData<int>("killNum", 0);

        GameData.Instance.AddBeKillListen(UIManager.Instance.ShowDieScreen);
        GameData.Instance.AddBeKillListen(RemovePlayer);
    }

    //移除游戏对象
    private void RemovePlayer()
    {
        foreach (PlayerController playerController in GameObject.FindObjectsOfType<PlayerController>())
        {
            if (playerController.photonView.IsMine)
            {
                PhotonNetwork.Destroy(playerController.gameObject);
                isBeginPlay = false;
            }
            
        }
        
    }

    public void OnClickBeginPlayButtn()
    {
        

        CreateCharacter();

        UIManager.Instance.SetBeginPlayScreen(false);
    }

    public void OnClickRestartButton()
    {
        CreateCharacter();
        DataConnectMgr.Instance.SetData<int>("killNum", 0);
        UIManager.Instance.SetDieScreen(false);
    }


    /// <summary>
    /// 创建角色
    /// </summary>
    public void CreateCharacter()
    {
        float randomX = Random.Range(-50, 50);
        float randomY = Random.Range(8, 10);
        float randomZ = Random.Range(-50, 50); 
        PhotonNetwork.Instantiate(playPrefab.name, new Vector3(randomX, randomY, randomZ), playPrefab.transform.rotation);

        isBeginPlay = true;
    }


    private void LateUpdate()
    {
        if (isBeginPlay)
        {
            GameData.Instance.UpdateData();
            UIManager.Instance.UpdateUI();
        }
      

    }


}
