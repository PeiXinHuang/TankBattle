using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviourPun
{

    private PlayerController hitPlayer = null; 


    private void OnCollisionEnter(Collision collision)
    {
        //·¢Éä×Óµ¯
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
            return;
        }
       
        if (collision.gameObject.tag == "Player")
        {
            //zhotonNetwork.Destroy(collision.gameObject);
            //txt.SetActive(true);
            collision.gameObject.GetComponent<PlayerController>().DropHP();
            


        }
    }


 
}
