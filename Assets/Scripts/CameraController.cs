using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform target;//�����ҪΧ������ת������ 
    public float distance = 7.0f;//�������Ŀ������֮��ľ��� 
    //private float eulerAngles_x;
    //private float eulerAngles_y;
    //public bool canRotate = true;

    //ˮƽ������� 
    public int distanceMax = 10;//�������Ŀ������֮��������� 
    public int distanceMin = 3;//�������Ŀ������֮�����С���� 
    public float xSpeed = 70.0f;//�����ˮƽ������ת�ٶ� 

    //��ֱ������� 
    public int yMaxLimit = 60;//���y����λ�ǽǶȣ� 
    public int yMinLimit = -10;//��Сy����λ�ǽǶȣ� 
    public float ySpeed = 70.0f;//�����������ת�ٶ� 

    public float oriDistance = 5.0f;

    //������� 
    public float MouseScrollWheelSensitivity = 1.0f;//�����������ȣ���ע�������ֹ����󽫵��������Ŀ������֮��ļ���� 
    public LayerMask CollisionLayerMask;

    private List<GameObject> collideredObjects = new List<GameObject>();//��������hit����GameObject
    private List<GameObject> bufferOfCollideredObjects = new List<GameObject>();//�ϴ�����hit����GameObject


    void Start()
    {
        //Vector3 eulerAngles = this.transform.eulerAngles;//��ǰ�����ŷ���� 
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
                bufferOfCollideredObjects.Add(collideredObjects[temp]);//�õ��ϴε�
            }
            collideredObjects.Clear();

            //��������
            Vector3 dir = -(target.position - transform.position).normalized;
            RaycastHit[] hits;
            hits = Physics.RaycastAll(target.position, dir, Vector3.Distance(target.position, transform.position));
            // Debug.DrawLine(player_Transform.position, transform.position, Color.red);//������ʾ�Ա�۲�

            for (int i = 0; i < hits.Length; i++)
            {

                if (hits[i].collider.gameObject.tag == "wall")
                {
                    collideredObjects.Add(hits[i].collider.gameObject);//�õ����ڵ�
                }
            }

            //���ϴεĻ�ԭ����ε�͸��
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

            //    //�������������ת�ǶȺ�λ��
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

    //�ѽǶ����Ƶ�������Χ�� 
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

    //�Ƿ�͸��
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