using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    [SerializeField]
    private GameObject beginPlayScreen; //��ʼ����

    public GameObject playPrefab; //��ɫԤ����

    private void Awake()
    {
        beginPlayScreen.SetActive(true);
    }


    public void OnClickBeginPlayButtn()
    {
        CreateCharacter();
        beginPlayScreen.SetActive(false);
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
    }
}
