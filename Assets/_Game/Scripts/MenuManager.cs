using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject settingPanel;
    [SerializeField] Button MusicBtn, SoundBtn;
    [SerializeField] Sprite MusicOnImg, SoundOnImg, MusicOffImg, SoundOffImg;
    [SerializeField] AudioClip ClickSound;

    private void Start()
    {
        MusicSet();
        SoundSet();
    }
    public void OnClick_PlayBtn()
    {
        Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        SceneManager.LoadScene(1);
    }
    public void OnClick_ExitBtn()
    {
        Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        Application.Quit();
    }
    public void OnClick_SettingBtn()
    {
        Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        settingPanel.SetActive(true);
    }
    public void OnClick_SettingBtnClose()
    {
        Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        settingPanel.SetActive(false);
    }

    public void MusicManager()
    {
        Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        if (Common.Instance.isMusicPlaying == false)
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().mute = false;
            MusicBtn.GetComponent<Image>().sprite = MusicOnImg;
            Common.Instance.isMusicPlaying = true;
        }
        else
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().mute = true;
            MusicBtn.GetComponent<Image>().sprite = MusicOffImg;
            Common.Instance.isMusicPlaying = false;
        }
    }
    public void MusicSet()
    {
        if (Common.Instance?.isMusicPlaying == false)
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().mute = true;
            MusicBtn.GetComponent<Image>().sprite = MusicOffImg;
        }
        else
        {
            Common.Instance.gameObject.transform.GetChild(1).GetComponent<AudioSource>().mute = false;
            MusicBtn.GetComponent<Image>().sprite = MusicOnImg;

        }
    }
    public void SoundManager()
    {
        Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().PlayOneShot(ClickSound);
        if (Common.Instance.isSoundPlaying == false)
        {
            Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().mute = false;
            SoundBtn.GetComponent<Image>().sprite = SoundOnImg;
            Common.Instance.isSoundPlaying = true;
        }
        else
        {
            Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().mute = true;
            SoundBtn.GetComponent<Image>().sprite = SoundOffImg;
            Common.Instance.isSoundPlaying = false;
        }
    }
    public void SoundSet()
    {
        if (Common.Instance.isSoundPlaying == false)
        {
            Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().mute = true;
            SoundBtn.GetComponent<Image>().sprite = SoundOffImg;
        }
        else
        {
            Common.Instance.gameObject.transform.GetChild(0).GetComponent<AudioSource>().mute = false;
            SoundBtn.GetComponent<Image>().sprite = SoundOnImg;

        }
    }
}
