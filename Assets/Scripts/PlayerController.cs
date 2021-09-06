using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun,IPunObservable
{
    private Rigidbody rdby;
    private Animator animator;

    public int hpNum = 100;

    public float forceNum = 250.0f;
    public float rotateSpeed = 50.0f;


    private float aniDistance = 0.5f; //�������룬�������ֵ�������ſ���ִ��

    public GameObject bulletPreb; //�ӵ�
    public Transform bulletTran; //�ӵ�����λ��
    public Transform camTran; //�����λ��
    



    public GameObject uiObj; //UI����
    public Slider hpSlider; //Ѫ��
    public Text nameText; //����

    private void Start()
    {
        rdby = this.GetComponent<Rigidbody>();
        animator = this.GetComponentInChildren<Animator>();

        this.Init();
           

    }

    //��ʼ���Լ�
    private void Init()
    {

       

        if (photonView.IsMine)
        {
            //�������������
            Camera.main.GetComponent<CameraController>().selfTran = this.camTran;
            Camera.main.GetComponent<CameraController>().playerTran = this.transform;

            uiObj.SetActive(false);

            //��ȡ������ҵ�����,������
            foreach (GameObject item in GameObject.FindGameObjectsWithTag("Player"))
            {
                Debug.Log(item.GetComponent<PhotonView>().Owner.NickName);
                item.GetComponent<PlayerController>().nameText.text = item.GetComponent<PhotonView>().Owner.NickName;
            }

            //֪ͨ��������Լ������ƣ���������Ϊ�Լ���������
            photonView.RPC("SetName", RpcTarget.All,PhotonNetwork.NickName);

            UIManager.Instance.SetNameText(PhotonNetwork.NickName);

        }
        else
        {
           

        }

    }



    public void Update()
    {
        if (!photonView.IsMine) return;

        PlayAni();

        Rotate();
        Move();
        Attack();
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {      
            GameObject bullet = PhotonNetwork.Instantiate(bulletPreb.name, bulletTran.position, this.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * Time.deltaTime * 30000.0f;
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

    private void PlayAni()
    {

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        if(v*v+h*h> aniDistance)
            this.animator.SetBool("isRun", true);
        else
            this.animator.SetBool("isRun", false);

    }




    public void DownHP()
    {

       


        photonView.RPC("SetHP", RpcTarget.All);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(hpNum);

        }
        else
        {
            hpNum = (int)stream.ReceiveNext();
            hpSlider.value = hpNum / 100.0f;
        }
    }

    //����Ѫ����ͬ��
    [PunRPC]
    private void SetHP()
    {
        hpNum -= 10;
        Debug.Log(hpNum);
        hpSlider.value = hpNum / 100.0f;


        if (photonView.IsMine)
            UIManager.Instance.SetHpSlider(hpNum / 100.0f);
    }


    //�������Ʋ�ͬ��
    [PunRPC]
    private void SetName(string name)
    {

        nameText.text = name;
    }
}
