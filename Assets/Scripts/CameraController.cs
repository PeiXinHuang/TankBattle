using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform target;//主相机要围绕其旋转的物体 
    public float distance = 7.0f;//主相机与目标物体之间的距离 
    //private float eulerAngles_x;
    //private float eulerAngles_y;
    //public bool canRotate = true;

    //水平滚动相关 
    public int distanceMax = 10;//主相机与目标物体之间的最大距离 
    public int distanceMin = 3;//主相机与目标物体之间的最小距离 
    public float xSpeed = 70.0f;//主相机水平方向旋转速度 

    //垂直滚动相关 
    public int yMaxLimit = 60;//最大y（单位是角度） 
    public int yMinLimit = -10;//最小y（单位是角度） 
    public float ySpeed = 70.0f;//主相机纵向旋转速度 

    public float oriDistance = 5.0f;

    //滚轮相关 
    public float MouseScrollWheelSensitivity = 1.0f;//鼠标滚轮灵敏度（备注：鼠标滚轮滚动后将调整相机与目标物体之间的间隔） 
    public LayerMask CollisionLayerMask;

    private List<GameObject> collideredObjects = new List<GameObject>();//本次射线hit到的GameObject
    private List<GameObject> bufferOfCollideredObjects = new List<GameObject>();//上次射线hit到的GameObject


    void Start()
    {
        //Vector3 eulerAngles = this.transform.eulerAngles;//当前物体的欧拉角 
        //this.eulerAngles_x = eulerAngles.y;
        //this.eulerAngles_y = eulerAngles.x;
    }

    void LateUpdate()
    {
        if (this.target != null)
        {

            this.oriDistance = Mathf.Clamp(this.oriDistance - (Input.GetAxis("Mouse ScrollWheel") * MouseScrollWheelSensitivity), (float)this.distanceMin, (float)this.distanceMax);

            //RaycastHit hitInfo = new RaycastHit();
            bufferOfCollideredObjects.Clear();
            for (int temp = 0; temp < collideredObjects.Count; temp++)
            {
                bufferOfCollideredObjects.Add(collideredObjects[temp]);//得到上次的
            }
            collideredObjects.Clear();

            //发射射线
            Vector3 dir = -(target.position - transform.position).normalized;
            RaycastHit[] hits;
            hits = Physics.RaycastAll(target.position, dir, Vector3.Distance(target.position, transform.position));
            // Debug.DrawLine(player_Transform.position, transform.position, Color.red);//让其显示以便观测

            for (int i = 0; i < hits.Length; i++)
            {

                if (hits[i].collider.gameObject.tag == "wall")
                {
                    collideredObjects.Add(hits[i].collider.gameObject);//得到现在的
                }
            }

            //把上次的还原，这次的透明
            for (int i = 0; i < bufferOfCollideredObjects.Count; i++)
            {
                SetMaterialsColor(bufferOfCollideredObjects[i].GetComponent<Renderer>(), false);
            }
            for (int i = 0; i < collideredObjects.Count; i++)
            {
                SetMaterialsColor(collideredObjects[i].GetComponent<Renderer>(), true);
            }

            //if (canRotate)
            //{
            //    this.eulerAngles_x += ((Input.GetAxis("Mouse X") * this.xSpeed) * this.distance) * 0.02f;
            //    this.eulerAngles_y -= (Input.GetAxis("Mouse Y") * this.ySpeed) * 0.02f;
            //    this.eulerAngles_y = ClampAngle(this.eulerAngles_y, (float)this.yMinLimit, (float)this.yMaxLimit);

            //    Quaternion quaternion = Quaternion.Euler(this.eulerAngles_y, this.eulerAngles_x, (float)0);

            //    Vector3 vector = ((Vector3)(quaternion * new Vector3((float)0, (float)0, -this.oriDistance))) + this.target.position;

            //    //更改主相机的旋转角度和位置
            //    this.transform.rotation = quaternion;
            //    this.transform.position = vector;
            //}
            //else
            //{
            //    Quaternion quaternion = Quaternion.Euler(this.eulerAngles_y, this.eulerAngles_x, (float)0);

            //    Vector3 vector = ((Vector3)(quaternion * new Vector3((float)0, (float)0, -this.oriDistance))) + this.target.position;
            //    this.transform.position = vector;
            //}

            //Quaternion quaternion = Quaternion.Euler(this.eulerAngles_y, this.eulerAngles_x, (float)0);

            this.transform.position = this.target.position; 
            this.transform.rotation = target.transform.rotation;



        }






    }

    //把角度限制到给定范围内 
    public float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360)
        {
            angle += 360;
        }
        while (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }

    //是否透明
    void SetMaterialsColor(Renderer r, bool isClear)
    {
        if (isClear)
        {
            int materialsNumber = r.sharedMaterials.Length;
            for (int i = 0; i < materialsNumber; i++)
            {
                r.materials[i].shader = Shader.Find("Transparent/Diffuse");
                Color tempColor = r.materials[i].color;
                tempColor.a = 0.1f;
                r.materials[i].color = tempColor;

            }
        }
        else
        {
            int materialsNumber = r.sharedMaterials.Length;
            for (int i = 0; i < materialsNumber; i++)
            {
                r.materials[i].shader = Shader.Find("Transparent/Diffuse");
                Color tempColor = r.materials[i].color;
                tempColor.a = 1f;
                r.materials[i].color = tempColor;

            }
        }
    }

    public void SetSpeed(Slider slider)
    {
        xSpeed = 5 + 50 * slider.value;
        ySpeed = 5 + 50 * slider.value;
    }
}