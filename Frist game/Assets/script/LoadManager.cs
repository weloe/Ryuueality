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
        loadScreen.SetActive(true);//����������



        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

        operation.allowSceneActivation = false;//ϣ�����������겻�Զ���ת

        while(!operation.isDone)//û�����
        {
            slider.value = operation.progress;//������0-1��progress0-1

            text.text = operation.progress * 100 + "%";//�Ŵ�һ�ٱ����ϰٷֺ�

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
