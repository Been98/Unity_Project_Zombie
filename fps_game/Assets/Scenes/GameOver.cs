using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOver : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(re(4.0f));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator re(float gg)
    {
        
        yield return new WaitForSeconds(gg);
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
