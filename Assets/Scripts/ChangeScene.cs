using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Wait()// inam
    {
        yield return new WaitForSeconds(4f);
    }
    public void SceneChange()
    {
        StartCoroutine(Wait());// in taze ezafe shod 
        SceneManager.LoadScene(1);
    }
}
