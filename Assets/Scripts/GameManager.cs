using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPun
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

    }

    public void OnClickBeginPlayButtn()
    {
        

        CreateCharacter();

        UIManager.Instance.SetBeginPlayScreen(false);
    }


    /// <summary>
    /// ������ɫ
    /// </summary>
    public void CreateCharacter()
    {
        float randomX = Random.Range(-10, 10);
        float randomY = Random.Range(1, 10);
        float randomZ = Random.Range(-10, 10); 
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
