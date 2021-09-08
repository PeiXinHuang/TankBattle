using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject sendDataObj; //��������Ԥ����

    public GameObject playPrefab; //��ɫԤ����

    public bool isBeginPlay = false; //�Ƿ�ʼ��Ϸ

    private void Awake()
    {
        UIManager.Instance.SetBeginPlayScreen(true);
    }


    private void Start()
    {
        //��ʼ��Ϸ���������ݵ�������
        DataConnectMgr.Instance.SetData<string>("playerName", PhotonNetwork.NickName);
        DataConnectMgr.Instance.SetData<int>("hp", 100);
        DataConnectMgr.Instance.SetData<int>("killNum", 0);

        GameData.Instance.AddBeKillListen(UIManager.Instance.ShowDieScreen);
        GameData.Instance.AddBeKillListen(RemovePlayer);
    }

    //�Ƴ���Ϸ����
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
    /// ������ɫ
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
