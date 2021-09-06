using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviourPun
{

    private void OnTriggerEnter(Collider other)
    {
        //�����ӵ�
        if (photonView.IsMine)
        {
 
            if (other.tag == "Player")
            {
        
                other.gameObject.GetComponent<PlayerController>().DownHP();
            }

            PhotonNetwork.Destroy(this.gameObject);
          
        }

        
        
 
    }





}
