using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Controlling animation of left side orders 
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger("OneCF");
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("TwoCoffee");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetTrigger("OneSugar");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetTrigger("TwoSugar");
        }

        //Controlling animation of Right side orders  

        if (Input.GetKeyDown(KeyCode.Z))
        {
            anim.SetTrigger("OneCoffeeR");
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("TwoCoffeeR");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            anim.SetTrigger("OneSugarR");
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            anim.SetTrigger("TwoSugarR");
        }
    }
}
