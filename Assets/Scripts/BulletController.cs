using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviourPun
{



    private void OnCollisionEnter(Collision collision)
    {
        //·¢Éä×Óµ¯
        if (photonView.IsMine)
        {
            if (tag == "Player")
            {
                collision.gameObject.GetComponent<PlayerController>().DownHP();
            }

            PhotonNetwork.Destroy(this.gameObject);
            return;
        }
       
        
    }


 
}
