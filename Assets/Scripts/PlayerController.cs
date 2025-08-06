using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public enum ToxicType
{
    Not, Threats, Insult, Disruption, Illwishes
}

[Serializable]
public class Messages
{
    public string messageText;
    public ToxicType isToxic;
    public string usernameText;
}


public class PlayerController : MonoBehaviour
{
    public TMP_Text countText;
    public TMP_Text LivesText;
    public TMP_Text winText;
    public TMP_Text messageText;
    public TMP_Text goodMessage;
    public TMP_Text badMessage;
    public TMP_Text wrongCategoryMessage;
    public TMP_Text rightCategoryMessage;
  
    public List<Messages> messages;
    public float messageInterval = 5f;
    public float feedbackMessageInterval = 5f;
    public float feedbackMessageTimer;
    public float categoryMessageTimer;
    public float categoryMessageInterval = 5f;
    public float BaseSpeed = 0f;
    public GameObject enemy;
    public Transform RespawnPoint;
    public Transform TestSpawn;

    public AudioSource AudioSource;
    public AudioClip CoinPickupAudio;
    public AudioClip FrogAudio;


    public Animator myAnim;


    public TMP_Text usernameText;
    
    public float usernameInterval = 5f;


    private float speed = 0;

    private float messageTimer;
    private float usernameMessageTimer;
    private Rigidbody rb;
    private int count;
    private int LifeCount;
    private int currentMessage = 0;
    private bool isFeedbackMessageShown= false;
    public bool isCategoryMessageShown= false;
    private float movementX;
    private float movementY;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speed = BaseSpeed;
        ShowNextMessage();
        ShowNextUsernameMessage();

 

        count = 0;
        LifeCount = 3;
        SetCountText();
        SetLivesText();
        rb = GetComponent<Rigidbody>();
        winText.gameObject.SetActive(false);
        goodMessage.gameObject.SetActive(false);
        badMessage.gameObject.SetActive(false);
        wrongCategoryMessage.gameObject.SetActive(false);
        rightCategoryMessage.gameObject.SetActive(false);
        myAnim = GetComponent<Animator>();
       
    }

    void ShowNextMessage()
    {
        messageInterval = 5f;
        currentMessage = UnityEngine.Random.Range(0, messages.Count);
        messageText.GetComponent<TextMeshProUGUI>().text = messages[currentMessage].messageText; 
    }

    void ShowNextUsernameMessage()
    {
        usernameInterval = 5f;
        int currentUser = UnityEngine.Random.Range(0, messages.Count);
        usernameText.GetComponent<TextMeshProUGUI>().text = messages[currentUser].usernameText;
        
    }


    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }


    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        rb.AddForce(movement * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            bool IsFollowing = enemy.GetComponent<EnemyMovement>().isFollowingPlayer;
            if (IsFollowing == true) 
            {
                LifeCount = LifeCount - 1;
                SetLivesText();
                this.transform.position = RespawnPoint.position;

                if (LifeCount<=0)
                {
                    Destroy(gameObject);
                    // Update the winText to display "You Lose!"
                    winText.gameObject.SetActive(true);
                    winText.GetComponent<TextMeshProUGUI>().text = "You Lose!";
                    goodMessage.gameObject.SetActive(false);
                    badMessage.gameObject.SetActive(false);
                }

                
            }

           else if (IsFollowing == false)
            {

                winText.gameObject.SetActive(true);

                enemy.GetComponent<EnemyMovement>().Die();
                AudioSource.resource = FrogAudio;
                AudioSource.Play();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup")) 
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();

            AudioSource.resource = CoinPickupAudio;
            AudioSource.Play();
        }
       
    }




    void SetCountText()
    {
        countText.text = "Count:" + count.ToString() + "/39";
        if(count >= 39)
        {
            winText.gameObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
           
        }
    }

    void SetLivesText()
    {
        LivesText.text = "Lives:" + LifeCount.ToString();
    }


    void OnReport()
    {
        isFeedbackMessageShown = true;
        feedbackMessageTimer = 0f;
        if (messages[currentMessage].isToxic == 0)
        {
            speed = BaseSpeed * 0.7f;
            goodMessage.gameObject.SetActive(false);
            badMessage.gameObject.SetActive(true);
            feedbackMessageInterval = 5f;

        }
        else {
            badMessage.gameObject.SetActive(false);
            goodMessage.gameObject.SetActive(true);
            feedbackMessageInterval = 10f;
            speed = BaseSpeed * 1.2f;
            enemy.GetComponent<EnemyMovement>().StopChasingPlayer();
            this.transform.position = TestSpawn.position;
            messageInterval = 15f;
            usernameInterval = 15f;
        }
        ;

    }


    private void Update()
    {
        messageTimer = messageTimer + Time.deltaTime;
        if (messageTimer > messageInterval)
        {
            ShowNextMessage();
            messageTimer = 0f;
        }

        usernameMessageTimer = usernameMessageTimer + Time.deltaTime;
        if (usernameMessageTimer > usernameInterval)
        {
            ShowNextUsernameMessage();
            usernameMessageTimer = 0f;
        }

        if (isFeedbackMessageShown == true)
        {
            feedbackMessageTimer = feedbackMessageTimer + Time.deltaTime;
            if (feedbackMessageTimer > feedbackMessageInterval)
            {
                goodMessage.gameObject.SetActive(false);
                badMessage.gameObject.SetActive(false);
                feedbackMessageTimer = 0f;
                isFeedbackMessageShown = false;
            }
            
        }

        if (isCategoryMessageShown == true)
        {
            categoryMessageTimer = categoryMessageTimer + Time.deltaTime;
            if (categoryMessageTimer > categoryMessageInterval)
            {
                rightCategoryMessage.gameObject.SetActive(false);
                wrongCategoryMessage.gameObject.SetActive(false);
                categoryMessageTimer = 0f;
                isCategoryMessageShown = false;
            }

        }
    }

    public Messages GetCurrentMessage()
    {
        return messages[currentMessage];

    }

    public void IncreaseLife()
    {
        LifeCount++;
        SetLivesText();
    }

    public void ResetPosition()
    {
        this.transform.position = RespawnPoint.position;
    }
}
