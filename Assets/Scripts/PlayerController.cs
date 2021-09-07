using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPun,IPunObservable
{
    private Rigidbody rdby;
    private Vector3 rdby_v;
    private Vector3 rdby_av;

    private Animator animator;

    private Vector3 latestPos;
    private Quaternion latestRot;


    public float forceNum = 250.0f;
    public float rotateSpeed = 50.0f;
    private float aniDistance = 0.05f; //�������룬�������ֵ�������ſ���ִ��


    public GameObject bulletPreb; //�ӵ�
    public Transform bulletTran; //�ӵ�����λ��
    public Transform camTran; //�����λ��
    



    [SerializeField]private GameObject uiObj; //UI����
    [SerializeField]private Slider hpSlider; //Ѫ��
    [SerializeField]private Text nameText; //����

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

            DataConnectMgr.Instance.SetData<string>("playerName", PhotonNetwork.NickName);
            DataConnectMgr.Instance.SetData<int>("hp", 100);
            DataConnectMgr.Instance.SetData<int>("killNum", 0);

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
            bullet.GetComponent<BulletController>().Init(this);
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



    public void DownHP(PlayerController attckPlayer)
    {

        Player player = photonView.Owner;

        int attckPlayerHp = DataConnectMgr.Instance.ReadData<int>("hp", player);
        attckPlayerHp -= 10;
        DataConnectMgr.Instance.SetData<int>("hp", attckPlayerHp, player);

    }


    //�Լ���������������Ļ�ɱ����һ
    private void Die(PlayerController attckPlayer)
    {
        UIManager.Instance.SetDieScreen(true);
    }

    public void SetHpSlider(float hp)
    {
        hpSlider.value = hp;
    }
    public void SetNameText(string pname)
    {
        nameText.text = pname;
    }


    #region ͬ��������Ϣ
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(rdby.velocity);
            stream.SendNext(rdby.angularVelocity);
     
        }
        else
        {
            latestPos = (Vector3)stream.ReceiveNext();
            latestRot = (Quaternion)stream.ReceiveNext();
            rdby_v = (Vector3)stream.ReceiveNext();
            rdby_av = (Vector3)stream.ReceiveNext();
  
        }
    }

    private void LateUpdate()
    {

        if (!photonView.IsMine)
        {
            transform.position = Vector3.Lerp(transform.position, latestPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, latestRot, Time.deltaTime * 5);
            rdby.velocity = rdby_v;
            rdby.angularVelocity = rdby_av;


        }
        else
        {
            
        }
    }
    #endregion
}
