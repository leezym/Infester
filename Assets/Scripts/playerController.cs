using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public int masa;
    Animator anim;
    Rigidbody rigidbody, shooting;
    public float movementSpeed, runSpeed, bulletSpeed, jumpSpeed, fallSpeed;
    float verticalInput, timeRandom;
    [HideInInspector]
    public bool run, climb, nearWall, fall, duck, jump, die, pickUp, throwA, blowA;
    bool throwBullet, throwFlea;
    GameObject camera;
    [HideInInspector]
    public Transform spawnBullet, spawnFlea;
    public Rigidbody bullet, flea;
    hudMenu hudMenuScript;
    GameObject mainCanvas, hudCanvas, pauseCanvas;
    AudioSource sounds;
    public AudioClip steps, hurt;
    public AudioClip[] talk;
    
    //spawnObjectsController spawnObjectsControllerScript;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sounds = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        spawnBullet = GameObject.Find("spawnBullet").GetComponent<Transform>();
        spawnFlea = GameObject.Find("spawnFlea").GetComponent<Transform>();        
        hudMenuScript = GameObject.FindObjectOfType<hudMenu>().GetComponent<hudMenu>();
        //mainCanvas = GameObject.Find("mainMenu");
        hudCanvas = GameObject.Find("HUD");
        pauseCanvas = GameObject.Find("/GameController/Menus/pauseMenu");
        gameObject.SetActive(false);
    }
   
    void Start()
    {
        nearWall = false;
        climb = false;
        fall = false;
        duck = false;
        run = false;
        jump = false;
        die = false;
        //pickUp = false;
        throwA = false;
        blowA = false;
        throwBullet = true;
        throwFlea = true;
        rigidbody.mass = masa;
        timeRandom = Random.Range(10, 30);
    }

    // Update is called once per frame
    void Update()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        timeRandom -= Time.deltaTime;
        if (timeRandom <= 0)
        {
            int talkRandom = Random.Range(0, 4); 
            sounds.PlayOneShot(talk[talkRandom]);
            timeRandom = Random.Range(10, 30);
        }

        movePlayer();
        actionsPlayer();
        detectEnemy();
    }

    IEnumerator waitThrow()
    {
        throwA = true;
        throwBullet = false;
        yield return new WaitForSeconds(0.2f);
        shootEnemy(spawnBullet, bullet, 0.6f);
    }

    IEnumerator waitBlow()
    {
        blowA = true;
        throwFlea = false;
        yield return new WaitForSeconds(0.2f);
        shootEnemy(spawnFlea, flea, 0.5f);
    }

    void actionsPlayer()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
            pauseCanvas.SetActive(true);
            hudCanvas.SetActive(false);
        }

        if (Input.GetMouseButtonDown(0) && !die && throwBullet)
        {
            StartCoroutine(waitThrow());
        }
        else if (Input.GetMouseButtonDown(1) && !die && throwFlea)
        {
            StartCoroutine(waitBlow());
        }
    }

    void movePlayer()
    {
        // Movimiento camara
        verticalInput = Input.GetAxis("Vertical");
        //limitCamera();
        transform.localEulerAngles = new Vector3(0, camera.transform.localEulerAngles.y, 0);

        anim.SetFloat("inputV", verticalInput);
        anim.SetBool("runV", run);
        anim.SetBool("climb", climb);
        anim.SetBool("fall", fall);
        anim.SetBool("jump", jump);
        anim.SetBool("die", die);
        anim.SetBool("throw", throwA);
        anim.SetBool("blow", blowA);        

        // Saltar
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }       

        // Escalar
        if (nearWall && Input.GetKey(KeyCode.E))
        {
            rigidbody.mass = masa * 3;
            climb = true;
        }
        else
        {
            rigidbody.mass = masa;
            climb = false;
        }

        // Morir
        if (hudMenuScript.sliderLifePlayer.value <= 0)
        {
            die = true;
        }
    }

    void LateUpdate()
    {
        if (jump && anim.GetCurrentAnimatorStateInfo(0).IsName("Jumping") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            jump = false;
        }

        if (die && anim.GetCurrentAnimatorStateInfo(0).IsName("Die") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            checkDie();
            die = false;
        }

        if (throwA && anim.GetCurrentAnimatorStateInfo(0).IsName("Throw") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            throwA = false;
            throwBullet = true;
        }

        if (blowA && anim.GetCurrentAnimatorStateInfo(0).IsName("Blow") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            blowA = false;
            throwFlea = true;
        }

        if (fall && anim.GetCurrentAnimatorStateInfo(0).IsName("Falling") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            fall = false;
        }
    }

    void FixedUpdate()
    {
        bool floor = touchGround();
        bool wall = touchWall();
        bool sJump = smallJump();

        if (!die)
        {
            // Adelate o subir
            if (Input.GetKey(KeyCode.W) && !climb)
            {
                rigidbody.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * movementSpeed);
            }
            else if (Input.GetKey(KeyCode.W) && climb)
            {
                rigidbody.AddForce(Vector3.up * movementSpeed, ForceMode.Impulse);
                sounds.PlayOneShot(steps);
            }
            
            // Bajar
            if (Input.GetKey(KeyCode.S) && climb)
            {
                rigidbody.AddForce(-Vector3.up * movementSpeed, ForceMode.Impulse);
                sounds.PlayOneShot(steps);
            }

            // Correr
            if (Input.GetKey(KeyCode.LeftShift) && (throwA == false || blowA == false))
            {
                run = true;
                rigidbody.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * runSpeed);
            }
            else
            {
                run = false;
            }
        }

        // Caer
        if (fall)
        {
            rigidbody.MovePosition(transform.position - transform.up * Time.fixedDeltaTime * fallSpeed);
        }

        // Salto
        if (jump)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jumping") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f)
            {
                rigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
                if (Input.GetKey(KeyCode.W))
                {
                    rigidbody.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * movementSpeed);
                }
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("Jumping") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
            {
                rigidbody.AddForce(-Vector3.up * movementSpeed, ForceMode.Impulse);
                if (Input.GetKey(KeyCode.W))
                {
                    rigidbody.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * movementSpeed);
                }
            }
        }

        // Escalar y Caer
        if (wall)
        {
            nearWall = true;
        }
        else
        {
            nearWall = false;
        }

        if (floor)
        {
            fall = false;
        }
        else
        {
            if (sJump)
            {
                fall = false;
            }
            else
            {
                fall = true;
            }
        }
    }

    bool touchWall()
    {
        int mask = 1 << 9;
        if (Physics.Raycast(transform.position, transform.forward, 2f, mask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool touchGround()
    {
        if (Physics.Raycast(transform.position, -transform.up, 1f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool smallJump()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 8f) && !fall) // Condicion de muerte
        {
            Debug.DrawRay(transform.position, -transform.up * hit.distance, Color.blue);
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, -transform.up, Color.red);
            return false;
        }
    }

    void checkDie()
    {
        if (hudMenuScript.countLife > 0)
        {
            transform.position = GameObject.Find("initPlayer").GetComponent<Transform>().position;
            hudMenuScript.countLife -= 1;
            hudMenuScript.lifes[hudMenuScript.countLife].enabled = false;
            hudMenuScript.sliderLifePlayer.value = hudMenuScript.sliderLifePlayer.maxValue;
        }
    }

    void shootEnemy(Transform spawn, Rigidbody bullet, float posY)
    {
        shooting = Instantiate(bullet, spawn.transform.position, Quaternion.identity);
        shooting.AddForce((transform.forward + new Vector3(0,posY,0))* bulletSpeed, ForceMode.Impulse);
        Debug.DrawLine(transform.position, shooting.transform.position, Color.magenta);
    }

    void detectEnemy()
    {
        int mask = 1 << 10;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, GameObject.FindObjectOfType<enemyController>().lookRadius, mask))
        {
            hudMenuScript.sliderLifeEnemy.gameObject.SetActive(true);
            hudMenuScript.sliderLifeEnemy.value = hit.transform.gameObject.GetComponent<enemyController>().lifeEnemy;
        }
        else
        {
            hudMenuScript.sliderLifeEnemy.gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "enemy")
        {
            hudMenuScript.sliderLifePlayer.value -= 15;
            sounds.PlayOneShot(hurt, 0.6f);
        }             
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "bulletEnemy")
        {
            hudMenuScript.sliderLifePlayer.value -= 10;
            sounds.PlayOneShot(hurt, 0.6f);
        }

        if (collision.gameObject.tag == "ratTrap")
        {
            hudMenuScript.sliderLifePlayer.value = 0;
            collision.gameObject.SetActive(false);
            GameObject.FindObjectOfType<spawnObjectsController>().spawnNewItem(collision.gameObject);
            sounds.PlayOneShot(hurt, 0.6f);
        }
        else if (collision.gameObject.tag == "cheeseLife")
        {
            float value = Random.Range(0, 2);
            if (value == 0 && hudMenuScript.sliderLifePlayer.value < hudMenuScript.sliderLifePlayer.maxValue)
            {
                hudMenuScript.sliderLifePlayer.value += 15;
                collision.gameObject.SetActive(false);
                GameObject.FindObjectOfType<spawnObjectsController>().spawnNewItem(collision.gameObject);
            }
            else if (value == 1)
            {
                hudMenuScript.sliderLifePlayer.value -= 15;
                collision.gameObject.SetActive(false);
                GameObject.FindObjectOfType<spawnObjectsController>().spawnNewItem(collision.gameObject);
                sounds.PlayOneShot(hurt,0.6f);
            }
            
        }
    }
}
