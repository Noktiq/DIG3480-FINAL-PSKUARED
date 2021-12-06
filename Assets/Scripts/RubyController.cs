using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;
    
    AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioClip musicClipThree;
    public AudioClip musicClipFour;

    public GameObject nextStage;
    public GameObject loseTextObject;
    public GameObject winTextObject;
    private bool gameOver = false;
    private bool gameWin = false;
    //added
    private static int scoreValue = 0;
    
    public Text scoreText;


    public int maxHealth = 5;
    
    public GameObject projectilePrefab;
    public GameObject HitEffect;
    
    public AudioClip throwSound;
    public AudioClip hitSound;
    
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        
                if (Time.timeScale == 0.0f)
                {            
            Time.timeScale = 1.0f;        
                }


        
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;
        musicSource = GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>();
        

        //added
        scoreText.text = "Robots Fixed: " + scoreValue.ToString();
        loseTextObject.SetActive(false);
        winTextObject.SetActive(false);
        nextStage.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey ("escape")) 
        {
            Application.Quit();
            Debug.Log("escaped program");
        }

        if (scoreValue ==4)
        {
            
           nextStage.SetActive(true);
           

        }
        

        

        if (scoreValue ==10)
        {
            
           winTextObject.SetActive(true);
           

        }

        if (SceneManager.GetActiveScene () == SceneManager.GetSceneByName ("Main2")) 
         {
             nextStage.SetActive(false);
         }

        

        
         
         if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
            {

            
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (gameWin == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C))
        {
            Launch();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                   

                    
                    character.DisplayDialog();
                    nextStage.SetActive(false);
                     PlaySound(musicClipThree);
                    if (scoreValue ==4)
                    
                    {SceneManager.LoadScene("Main2");
                    
                    }
                }
            }
        }

        //added
        scoreText.text = "Robots Fixed: " + scoreValue.ToString();
    }

    
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }



 
     void OnCollisionEnter(Collision collision)
{
       if (collision.gameObject.tag=="NPC2")
        {
           PlaySound(musicClipFour);
        }
}
    
 
public void loadlevel(string level)
{
SceneManager.LoadScene(level);
 
}


    //added
    public void ChangeScore(int scoreAmount)
    {
        //scoreValue += scoreAmount;

        scoreValue += scoreAmount;
        //her scoreText is RobotFixed.Text
        scoreText.text = "Robots Fixed: " + scoreValue.ToString();
        Debug.Log("Score: " + scoreValue);

        if (scoreValue ==10)
        {
            speed = 0;
           winTextObject.SetActive(true);
           gameWin = true;
           musicSource.clip = musicClipOne;
            musicSource.Play();

        }

    }


    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;

            Instantiate(HitEffect,transform.position, Quaternion.identity);
            
            PlaySound(hitSound);
        }

        if (currentHealth == 0)
        {
                    if (Time.timeScale == 1.0f) 
                    {           
            Time.timeScale = 0.0f;        
                    }
            speed = 0;
           loseTextObject.SetActive(true);
            gameOver = true;
            
              musicSource.clip = musicClipTwo;
            musicSource.Play();
  
            
            


           
        
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
    
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        
        PlaySound(throwSound);
    } 
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
