using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviourPun, IPunObservable
{

    

    private void Update()
    {

        this.transform.localScale = new Vector3(1, 1, NUM);

        if (!photonView.IsMine)
            return;


        if (Input.GetMouseButtonDown(0))
        {
            photonView.RPC("ChangeCube", RpcTarget.All);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ChangeCube2();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(NUM);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            NUM++;
        }
        
    }



    [PunRPC]
    private void ChangeCube()
    {
        this.transform.localScale = this.transform.localScale + new Vector3(0, 0, 1);
    }

    private void ChangeCube2()
    {
        this.transform.localScale = this.transform.localScale + new Vector3(1, 0, 0);
    }

    public int NUM = 0;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.NUM);
        }
        else
        {
            this.NUM = (int)stream.ReceiveNext();
        }
    }


}
