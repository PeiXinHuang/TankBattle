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
    private float aniDistance = 0.05f; //动画距离，大于这个值，动画才可以执行


    public GameObject bulletPreb; //子弹
    public Transform bulletTran; //子弹发射位置
    public Transform camTran; //摄像机位置




    [SerializeField]private GameObject uiObj; //UI对象
    [SerializeField]private Slider hpSlider; //血条
    [SerializeField]private Text nameText; //名称

    private void Start()
    {
        rdby = this.GetComponent<Rigidbody>();
        
        animator = this.GetComponentInChildren<Animator>();

        this.Init();
           

    }

    //初始化自己
    private void Init()
    {

       

        if (photonView.IsMine)
        {
            //设置摄像机跟随
            Camera.main.GetComponent<CameraController>().selfTran = this.camTran;
            Camera.main.GetComponent<CameraController>().playerTran = this.transform;

            uiObj.SetActive(false);

            DataConnectMgr.Instance.SetData<int>("hp", 100); //初始化血量

            
        }
        else
        {
           

        }

    }



    public void UpdateTankUIRot()
    {
        uiObj.transform.LookAt(Camera.main.transform.position);

    }


    public void Update()
    {
        UpdateTankUIRot();

        if (!photonView.IsMine) return;

      
        PlayAni();

        Rotate();
        Move();
        Attack();

        RotateByTerrain();
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
        //this.GetComponent<CharacterController>().Move(this.transform.forward * v * Time.deltaTime * 3000);
    }

    private void Rotate()
    {
        float h = Input.GetAxis("Horizontal");
        transform.Rotate(0, h * Time.deltaTime * rotateSpeed, 0);
    }

    private void RotateByTerrain()
    {
    

        RaycastHit hit;

        int Rmask = LayerMask.GetMask("Terrain");

        Vector3 Point_dir = transform.TransformDirection(transform.up*(-1));

        if (Physics.Raycast(transform.position, Point_dir, out hit, 50.0f, Rmask))
        {

            Quaternion NextRot = Quaternion.LookRotation(Vector3.Cross(hit.normal, Vector3.Cross(transform.forward, hit.normal)), hit.normal);


            transform.rotation = Quaternion.Lerp(transform.rotation, NextRot, Time.deltaTime);

        }
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

        //刷新血量
        int playerHp = DataConnectMgr.Instance.ReadData<int>("hp", player);
        playerHp -= 10;
        DataConnectMgr.Instance.SetData<int>("hp", playerHp, player);

        //血量小于0,攻击对象击杀数+1
        if (playerHp <= 0)
        {
            int attackPlayerKillNum = DataConnectMgr.Instance.ReadData<int>("killNum", attckPlayer.photonView.Owner);
            attackPlayerKillNum++;

            DataConnectMgr.Instance.SetData<int>("killNum",attackPlayerKillNum, attckPlayer.photonView.Owner);

        }

    }


    //自己死亡，攻击对象的击杀数加一
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


    #region 同步刚体信息
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
