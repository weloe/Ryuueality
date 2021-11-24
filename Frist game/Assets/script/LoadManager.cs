using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public GameObject loadScreen;
    public Slider slider;
    public Text text;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        loadScreen.SetActive(true);//启动进度条



        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        operation.allowSceneActivation = false;//希望进度条读完不自动跳转

        while(!operation.isDone)//没有完成
        {
            slider.value = operation.progress;//滑动条0-1，progress0-1

            text.text = operation.progress * 100 + "%";//放大一百倍加上百分号

            if(operation.progress>=0.9f)
            {
                slider.value = 1;
                text.text = "Press AnyKey to continue";
                if(Input.anyKeyDown)
                {
                    operation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }

}
