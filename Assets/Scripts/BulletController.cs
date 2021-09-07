using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviourPun
{

    private PlayerController player; //�������

    public void Init(PlayerController player)
    {
        this.player = player;
    }

    private void OnTriggerEnter(Collider other)
    {
        //�����ӵ�
        if (photonView.IsMine)
        {
 
            if (other.tag == "Player")
            {
                
                other.gameObject.GetComponent<PlayerController>().DownHP(this.player);
            }

            PhotonNetwork.Destroy(this.gameObject);
          
        }

        
        
 
    }





}
