using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviourPun
{
    public Transform selfTran;//主相机要围绕其旋转的物体 
    public Transform playerTran; //游戏对象


    private List<GameObject> collideredObjects = new List<GameObject>();//本次射线hit到的GameObject
    private List<GameObject> bufferOfCollideredObjects = new List<GameObject>();//上次射线hit到的GameObject
    

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
                bufferOfCollideredObjects.Add(collideredObjects[temp]);//得到上次的
            }
            collideredObjects.Clear();

           
 
            Vector3 dir = -(selfTran.position - playerTran.position).normalized;
            RaycastHit[] hits;

            hits = Physics.RaycastAll(selfTran.position, dir, Vector3.Distance(selfTran.position, playerTran.position));
          
            for (int i = 0; i < hits.Length; i++)
            {

                if (hits[i].collider.gameObject.tag == "Wall")
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

         

            this.transform.position = this.selfTran.position; 
            this.transform.rotation = selfTran.transform.rotation;



        }






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


}