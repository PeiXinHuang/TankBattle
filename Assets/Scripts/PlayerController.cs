using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun,IPunObservable
{
    public float forceNum = 250.0f;
    public float rotateSpeed = 50.0f;

    public GameObject bulletPreb;
    public Transform bulletTran;

    public Rigidbody rdby;

    private int hp = 100; //血量100
    public Slider hpSlider; //血条

    private void Start()
    {
        rdby = this.GetComponent<Rigidbody>();
    }

    



    public void Update()
    {
        if (!photonView.IsMine) return;

        Debug.Log(hp);

        Rotate();
        Move();
        Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            GameObject bullet = PhotonNetwork.Instantiate(bulletPreb.name, bulletTran.position, this.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * Time.deltaTime * 10000.0f;
        }
    }


    private void Move()
    {
        float v = Input.GetAxis("Vertical");
        rdby.AddForce(this.transform.forward * v * Time.deltaTime * forceNum);
    }

    private void Rotate()
    {
        float h = Input.GetAxis("Horizontal");
        transform.Rotate(0, h * Time.deltaTime * rotateSpeed, 0);
    }

    private void OnDestroy()
    {
        
    }

    /// <summary>
    /// 同步数据
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
        if (stream.IsWriting == true)
        {         
            stream.SendNext(this.hp);
        }
        else
        {
            this.hp =(int)stream.ReceiveNext();
        }
    }


    public void DropHP()
    {
        
        hp -= 10;
        SetHPSilder();

    }

    /// <summary>
    /// 设置血条
    /// </summary>
    private void SetHPSilder()
    {
        hpSlider.value = hp / 100.0f;
    }

}
