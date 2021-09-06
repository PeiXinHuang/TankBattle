using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType(typeof(UIManager)) as UIManager;
            }
            return instance;
        }
    }

    public Slider hpSlider;
    public Text nameText;
    public void SetHpSlider(float value)
    {
        hpSlider.value = value;
    }
    public void SetNameText(string str)
    {
        nameText.text = str;
    }


}
