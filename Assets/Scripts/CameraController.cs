using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviourPun
{
    public Transform selfTran;//�����ҪΧ������ת������ 
    public Transform playerTran; //��Ϸ����


    private List<GameObject> collideredObjects = new List<GameObject>();//��������hit����GameObject
    private List<GameObject> bufferOfCollideredObjects = new List<GameObject>();//�ϴ�����hit����GameObject
    

    void Start()
    {

    }

    void LateUpdate()
    {

       
       

        if (this.selfTran != null && playerTran != null)
        {
       
            bufferOfCollideredObjects.Clear();
            for (int temp = 0; temp < collideredObjects.Count; temp++)
            {
                bufferOfCollideredObjects.Add(collideredObjects[temp]);//�õ��ϴε�
            }
            collideredObjects.Clear();

           
 
            Vector3 dir = -(selfTran.position - playerTran.position).normalized;
            RaycastHit[] hits;

            hits = Physics.RaycastAll(selfTran.position, dir, Vector3.Distance(selfTran.position, playerTran.position));
          
            for (int i = 0; i < hits.Length; i++)
            {

                if (hits[i].collider.gameObject.tag == "Wall")
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

         

            this.transform.position = this.selfTran.position; 
            this.transform.rotation = selfTran.transform.rotation;



        }






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


}