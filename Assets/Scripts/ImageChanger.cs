using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public Image uiImage;                    // a UI element to keep images 
    public Sprite[] images;                  // Keep images in an array 
    private int currentImageIndex = 0;       //index for looping through the images 
    private int imageSwitchCount = 0;        //the number of images 
    private int sugarCount = 0;

    private bool canSwitchImages = true;     // let the images to switch
    private bool isOrdering = false;    // check the order plays once 
    private bool isOrdering2 = false;   //check the second order plays 
    private bool isFilling = false;
    private bool isFillingLarge = false;
    private bool isServe = false;
    private bool isMessaging = false;
    

    private int counter=0;
    private int serveCounter = 0;
   

    public Button redButton;    //Trash button
    public Button sugarButton;  //SugarCube
    public Button sugarButton2;
    public Button onButton;     //Turn on the coffe maker 
    public Button serveButton;  //Serve the coffee
    public Button coffeeButton; //Coffee button

    public bool servePushed = false;
    public bool serveFinished = false;

    
    public Animator CoffeeFiller;
    public GameObject FillAnimate; // the game object with the  Filling animator attached to it 

    public Animator Serve;
    public GameObject ServeAnimate; // the game object with the Serve animator attached to it
                                    // 
    public Animator Order;
    public GameObject OrderParent;

    public Animator Order2;
    public GameObject OrderParent2;

    public Animator FillLarge;
    public GameObject FillLargeCup;

    public Animator FinalMessage;
    public GameObject Message;

   
   

    // Start is called before the first frame update
    void Start()
    {


        CoffeeFiller = GameObject.Find("Fill").GetComponent<Animator>();
        Serve = GameObject.Find("Serve").GetComponent<Animator>();
        Order = GameObject.Find("Order").GetComponent<Animator>();
        Order2= GameObject.Find("TwoSugar").GetComponent<Animator>();
        FillLarge = GameObject.Find("FillLarge").GetComponent<Animator>();
        FinalMessage = GameObject.Find("Message").GetComponent<Animator>();


        if (CoffeeFiller == null)
              Debug.Log("animator not found "); 

        if (images!=null&& images.Length>0)
        {
            uiImage.sprite = images[currentImageIndex];
        }
        else
        {
            Debug.LogError("No Images assigned!");
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        //if the mouse is clicked and there is any image
        if (Input.GetMouseButtonDown(0)&&images.Length>0 && canSwitchImages) 
        {
            SwitchToNextImage();
        }

        //checks if the animation has finished or not 
        if (isFilling && CoffeeFiller.GetCurrentAnimatorStateInfo(0).IsName("Fill") && CoffeeFiller.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f  )
        {
            
            Debug.Log("cup filling is stopped!");
            onButton.gameObject.SetActive(false);
            serveButton.gameObject.SetActive(true);
            isFilling = false;
            canSwitchImages = false;
          
        }

        if (isServe && Serve.GetCurrentAnimatorStateInfo(0).IsName("Serve") && Serve.GetCurrentAnimatorStateInfo(0).normalizedTime >=1.0f /*&& /*!serveFinished  */)
        {
            //Serve.enabled = false;
            ServeAnimate.SetActive(false);
            serveFinished = true;
            isServe = false;
            // OrderParent.SetActive(true);
            // Order.Play("Order",-1,0f);
            // isOrdering = true;
            counter++;
            Debug.Log(counter);
            if (counter == 2)
            {
                nextOrder();

            }

            else if (counter == 3)
            {
                Message.SetActive(true);
                FinalMessage.Play("Message", -1, 0f);
                isMessaging = true;
            }
              
            else
                order();
            

        }
        if (isOrdering && Order.GetCurrentAnimatorStateInfo(0).IsName("Order") && Order.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            
           // Order.enabled = false;
            OrderParent.SetActive(false);
            isOrdering = false;
            SwitchToNextImage();
            onButton.gameObject.SetActive(false);
            coffeeButton.gameObject.SetActive(true);
            
        }
        if(isOrdering2 && Order2.GetCurrentAnimatorStateInfo(0).IsName("TwoSugar") && Order2.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            OrderParent2.SetActive(false);
            isOrdering2 = false;
            SwitchToNextImage();
            Debug.Log("Order two finished ");
            sugarButton2.gameObject.SetActive(true);
  
        }
        if(isFillingLarge && FillLarge.GetCurrentAnimatorStateInfo(0).IsName("FillLarge") && FillLarge.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {

            Debug.Log("Large cup filling is stopped!");
            onButton.gameObject.SetActive(false);
            serveButton.gameObject.SetActive(true);
            isFillingLarge = false;
            canSwitchImages = false;
          

        }
        if (isMessaging && FinalMessage.GetCurrentAnimatorStateInfo(0).IsName("Message") && FinalMessage.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Debug.Log("finish");
            
            SceneManager.LoadScene(1);
        }


    }

   void SwitchToNextImage()
    {
        if (images.Length == 0) return;

        currentImageIndex = (currentImageIndex + 1)%images.Length ;
        uiImage.sprite = images[currentImageIndex];
        imageSwitchCount++;
        if (imageSwitchCount == 2)
        {
            redButton.gameObject.SetActive(true);
            canSwitchImages = false;
        }
        if(imageSwitchCount==7)
        {
            sugarButton.gameObject.SetActive(true);
            canSwitchImages = false;
        }
        if(imageSwitchCount==8)
        {
            onButton.gameObject.SetActive(true);
            canSwitchImages = false;
        }
       
        
    }
   public void onButtonClick()
    {
        redButton.gameObject.SetActive(false);
        canSwitchImages = true;
        SwitchToNextImage();

    }
    public void onSugarClick()
    {
       
        sugarButton.gameObject.SetActive(false);
        currentImageIndex += 1;
        canSwitchImages = true;
        SwitchToNextImage();
        
    }
    public void onSugar2click()
    {
        if (sugarCount ==0)
        {

            canSwitchImages = true;
            SwitchToNextImage();
            sugarCount++;
        }
        else
        {
            canSwitchImages = false;
            sugarButton2.gameObject.SetActive(false);
            onButton.gameObject.SetActive(true);

        }
        
        
      
    }
    public void TurnOnButtonClick()
    {
        onButton.gameObject.SetActive(true);
        if (sugarCount==1)
        {
            onButton.gameObject.SetActive(false);
            onFillLargeCup();
        }
        else
        {
            onButton.gameObject.SetActive(false);
            canSwitchImages = true;
            SwitchToNextImage();
            canSwitchImages = false;
            onFillCup();
        
        }

        /* FillAnimate.SetActive(true);
         CoffeeFiller = GameObject.FindAnyObjectByType<Animator>();//finds the animator 
         CoffeeFiller.Play("Fill");*/

        /*
                 if(CoffeeFiller.GetCurrentAnimatorStateInfo(0).IsName("Fill") && CoffeeFiller.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                 {
                     Debug.Log("Animation finished");
                     serveButton.gameObject.SetActive(true);
                 }*/

    }
    public void ServeButtonClick()
    {
        serveCounter++;
        Debug.Log(serveCounter);
        servePushed = true;
        serveButton.gameObject.SetActive(false);
       /* if(serveCounter==3)

        {
            canSwitchImages = false;
            serveCup();
           
        }
            
        else*/
        {
            SwitchToNextImage();
            serveCup();
        }
        
        



        /* ServeAnimate.SetActive(true);
         Serve.Play("Serve");
         FillAnimate.SetActive(false);*/
    }
    public void CoffeeButtonClick()
    {
        coffeeButton.gameObject.SetActive(false);
        SwitchToNextImage();
        onButton.gameObject.SetActive(true);
       
        

    }
    public void onFillCup()
    {
        isFilling = true;
        FillAnimate.SetActive(true);
        CoffeeFiller.Play("Fill",-1,0f);
      
        Debug.Log("cup is filling");
        
    }
    public void onFillLargeCup()
    {
        isFillingLarge = true;
        FillLargeCup.SetActive(true);
        FillLarge.Play("FillLarge", -1, 0f);
        Debug.Log("Large cup is filling");
    } 
    public void serveCup()
    {
        isServe = true;
        ServeAnimate.SetActive(true);
        Serve.Play("Serve",-1,0f);
        FillAnimate.SetActive(false);
        FillLargeCup.SetActive(false);
        Debug.Log("cup is being served");
    }
    public void order()
    {
        OrderParent.SetActive(true);
        isOrdering = true;
         Order.Play("Order",-1,0f);
    }
    public void nextOrder()
    {
        coffeeButton.gameObject.SetActive(false);
        sugarButton.gameObject.SetActive(false);

        isOrdering2 = true;
        OrderParent2.SetActive(true);
        Order2.Play("TwoSugar", -1, 0f);
        Debug.Log("Second order is running ");

    }
    
}
