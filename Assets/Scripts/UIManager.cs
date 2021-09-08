using Photon.Pun;
using Photon.Realtime;
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

    [SerializeField] private Slider hpSlider;
    [SerializeField] private Text nameText;
    [SerializeField] private GameObject beginPlayScreen;
    [SerializeField] private Text killText;
    [SerializeField] private GameObject dieScreen;

    public void SetHpSlider(float value)
    {
        hpSlider.value = value;
    }
    public void SetNameText(string str)
    {
        nameText.text = str;
    }
    public void SetBeginPlayScreen(bool isShow)
    {
        beginPlayScreen.SetActive(isShow);
    }
    public void SetKillNumText(int killNum)
    {
        killText.text = "KillNum:" + killNum.ToString();
    }
    public void SetDieScreen(bool isShow)
    {
        dieScreen.SetActive(isShow);
    }

    public void ShowDieScreen()
    {
        dieScreen.SetActive(true);

    }


    /// <summary>
    /// 刷新UI
    /// </summary>
    public void UpdateUI()
    {
       //刷新所有tank的UI
        foreach (PlayerController playerController in GameObject.FindObjectsOfType<PlayerController>())
        {
            Player player = playerController.photonView.Owner;

            PlayerData playerData = GameData.Instance.ReadData(player);

            playerController.SetHpSlider(playerData.Hp / 100.0f);
            playerController.SetNameText(playerData.PlayerName);

        }

        //刷新自己的UI
        PlayerData m_playdata = GameData.Instance.ReadData();
        SetHpSlider(m_playdata.Hp / 100.0f);
        SetNameText(m_playdata.PlayerName);
        SetKillNumText(m_playdata.KillNum);

    }


}
