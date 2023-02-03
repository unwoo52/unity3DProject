using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SSceneLoaderScript : MonoBehaviour
{
    public static SSceneLoaderScript instance = null;
    public Slider Sliderslider;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }
    public void SceneChange(int i)
    {
        StartCoroutine(Loading(i));
    }

    IEnumerator Loading(int i)
    {
        yield return UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("SLoad");

        Sliderslider.gameObject.SetActive(true);
        Sliderslider.value= 0;

        StartCoroutine(LoadingTarget(i));
    }

    IEnumerator LoadingTarget(int i)
    {
        AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(i);
        asyncOperation.allowSceneActivation= false;
        while (!asyncOperation.isDone)
        {
            Sliderslider.value = asyncOperation.progress / 0.9f;
            if(asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1);
        Sliderslider.gameObject.SetActive(false);
       
    }
}
