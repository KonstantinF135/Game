using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] int speed = 10;
    Animator anim;
    GameObject player;
    bool run = false;
    int range = 2;
    Vector2 enemyPos;
    public float health = 10;
    float startHealth;
    public GameObject HealthBarEnemy;
    public float damage = 1;
    bool attack = true;
    public Transform attackPos;
    public float attackRange;
    public LayerMask isPlayer;
    private void Start()
    {
        startHealth = health;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        InvokeRepeating("RandomEvent", 5, 3);
    }

    private void FixedUpdate()
    {
        if (run == false && Vector2.Distance(transform.position, player.transform.position) > range)
        {
            anim.SetBool("Walk", true);
            enemyPos = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
            if (transform.position.x > player.transform.position.x)
                transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (transform.position.x < player.transform.position.x)
                transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (run == true && Vector2.Distance(transform.position, player.transform.position) > range)
        {
            anim.SetBool("Walk", false);
            enemyPos = Vector2.MoveTowards(transform.position, player.transform.position, -speed * Time.fixedDeltaTime);
            if (transform.position.x > player.transform.position.x)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (transform.position.x < player.transform.position.x)
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        //else if (Vector2.Distance(transform.position, player.transform.position) <= attackRange)
        //    Attack();
        transform.position = new Vector2(enemyPos.x, transform.position.y);
    }
    public void Damage(float damage)
    {
        health -= damage;
        HealthBarEnemy.transform.localScale = new Vector2(health / startHealth, 0.3f);
        if (health <= 0)
        {
            anim.SetBool("Die", true);
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;
            Camera.main.GetComponent<UISetting>().Win();
        }
    }
    public void Attack()
    {
        if (attack == true)
        {
            attack = false;
            anim.SetTrigger("Attack");
            Collider2D[] playerToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, isPlayer);
            for (int i = 0; i < playerToDamage.Length; i++)
            {
                playerToDamage[i].GetComponent<PlayerController>().Damage(damage);
            }
            Invoke("AttackReset", 1);
        }
    }

    void AttackReset()
    {
        attack = true;
    }
    void RandomEvent()
    {
       
            switch (Random.Range(0, 3))
            {
                case 1:
                    run = true;
                    Invoke("Run", 1);
                    break;
                case 2:
                    Jump();
                    break;
            }
        

    }
    void Run()
    {
        run = false;
    }
    public void Jump()
    {
        anim.SetTrigger("Jump");
        rb.AddForce(transform.up * 10, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ground")
            Jump();
        if (collision.gameObject.tag == "Player")
            Attack();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
