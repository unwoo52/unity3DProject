using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STitle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //Application.LoadLevel(5); << 여러 문제를 내포하고 있기 때문에 보통 사용되지 않음
            //UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(5);

            SSceneLoaderScript.instance.SceneChange(5);
        }
    }
}
