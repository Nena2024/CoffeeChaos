using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        if (audio != null)
        {
            audio.time = 7f;
            audio.Play();

        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
