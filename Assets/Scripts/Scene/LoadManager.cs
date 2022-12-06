using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour
{
    public static LoadManager Inst = null;
    public Slider mySlider = null;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this);
        }
    }

    public void ChangeScene(int i)
    {
        StartCoroutine(Loading(i));
    }

    IEnumerator Loading(int i)
    {
        yield return SceneManager.LoadSceneAsync("Loading");
        mySlider.gameObject.SetActive(true);
        mySlider.value = 0.0f;
        StartCoroutine(LoadingScene(i));
    }

    IEnumerator LoadingScene(int i)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(i);

        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            mySlider.value = ao.progress / 0.9f;

            if (ao.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1.0f);
                mySlider.gameObject.SetActive(false);
                ao.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
