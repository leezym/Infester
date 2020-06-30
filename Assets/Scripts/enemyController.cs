using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyController : MonoBehaviour
{
    public float lookRadius, collisonRadius, bulletSpeed, lifeEnemy;
    public Transform spawnBullet, destiny;
    [HideInInspector]
    public Transform[] moveToPoint;
    NavMeshAgent agent;
    Rigidbody shooting;
    public Rigidbody[] bullet;
    Rigidbody rigidbody;
    public float clock, damageBullet, damageFlea;
    float countDamageFlea = 5f;
    [HideInInspector]
    public bool shoot, walk, die, throwB, fight, impactFlea, goDestiny;
    int selectWalk;
    public Transform target;
    public Animator anim;
    spawnEnemyController spawnEnemyControllerScript;
    hudMenu hudMenuScript;
    AudioSource sounds;
    public AudioClip steps, findRat, hurt, dieS;
    float timeRandom = 1f;

    private void Awake()
    {
       
    }

    void Start()
    {
        //anim = GetComponent<Animator>();
        sounds = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        spawnEnemyControllerScript = GameObject.FindObjectOfType<spawnEnemyController>().GetComponent<spawnEnemyController>();
        hudMenuScript = GameObject.FindObjectOfType<hudMenu>().GetComponent<hudMenu>();

        shoot = true;
        lifeEnemy = hudMenuScript.sliderLifeEnemy.maxValue;
        impactFlea = false;
        moveToPoint = GameObject.Find("pointsPatrol").GetComponentsInChildren<Transform>();
        goDestiny = true;
        die = false;
        walk = false;
        throwB = false;
        selectWalk = 0;
        fight = false;
    }

    void Update()
    {        
        moveEnemy();

        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }      

        if (shoot)
        {
            clock = 0.8f;
        }

        // Asignar destino
        if (goDestiny)
        {
            selectWalk = Random.Range(1, 3);
            destiny = moveToPoint[Random.Range(1, moveToPoint.Length)].transform; // no se incluye el primero porque es el padre
            goDestiny = false;
        }
        // Cambie de destino
        else if (transform.position.x == destiny.position.x && transform.position.z == destiny.position.z)
        {
            goDestiny = true;
        }

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius && distance > collisonRadius) // Throw
        {
            // Sonido
            timeRandom -= Time.deltaTime;
            if (timeRandom <= 0)
            {
                sounds.PlayOneShot(findRat);
                timeRandom = 2f;
            }

            throwB = true;
            fight = false;
            walk = false;
            agent.SetDestination(target.transform.position);
            if (shoot)
            {
                shootPlayer();
                shoot = false;
            }
            else
            {
                clock -= Time.deltaTime;
                if (clock <= 0)
                {
                    shoot = true;
                }
            }
        }
        else if (distance <= lookRadius && distance <= collisonRadius) // Fight
        {
            fight = true;
            throwB = false;
            walk = false;
            shoot = false;
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else if (distance > lookRadius)
        {
            walk = true;
            fight = false;
            throwB = false;
            agent.SetDestination(destiny.position);
        }

        // Daño progresivo pulga
        if (impactFlea)
        {
            countDamageFlea -= Time.deltaTime;
            lifeEnemy -= 2 * Time.deltaTime;
        }
        if (countDamageFlea <= 0)
        {
            impactFlea = false;
            countDamageFlea = 5f;
        }

        // Muerte
        if (lifeEnemy <= 0)
        {
            gameObject.SetActive(false);
            spawnEnemyControllerScript.spawnNewEnemy(this.gameObject); // respawn
        }
    }    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, collisonRadius);
    }

    void shootPlayer()
    {
        int numBullet = Random.Range(0, 3);
        shooting = Instantiate(bullet[numBullet], spawnBullet.transform.position, Quaternion.identity);
        shooting.AddForce(new Vector3(transform.forward.x, target.transform.position.y + 2f, transform.forward.z ) * bulletSpeed, ForceMode.Impulse);
    }

    void moveEnemy()
    {
        anim.SetInteger("selectWalk", selectWalk);
        anim.SetBool("walk", walk);
        anim.SetBool("throw", throwB);
        anim.SetBool("fight", fight);
        //anim.SetBool("die", die);
    }

    private void LateUpdate()
    {
        /*if (die && anim.GetCurrentAnimatorStateInfo(0).IsName("Fight") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            die = false;
            //gameObject.SetActive(false);
            //spawnEnemyControllerScript.spawnNewEnemy(this.gameObject); // respawn
        }

        if (die && anim.GetCurrentAnimatorStateInfo(0).IsName("Throw") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            die = false;
            //gameObject.SetActive(false);
            //spawnEnemyControllerScript.spawnNewEnemy(this.gameObject); // respawn
        }*/

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "bulletPlayer")
        {
            sounds.PlayOneShot(hurt);
            lifeEnemy -= damageBullet;
            hudMenuScript.sliderDamage.value += 10;
        }
        if (collision.gameObject.tag == "fleaPlayer")
        {
            sounds.PlayOneShot(hurt);
            lifeEnemy -= damageFlea;
            impactFlea = true;
            hudMenuScript.sliderDamage.value += 20;
        }
    }
}
