using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //public int index;
    //public int indexWeapon;
    //public int indedShield;

    //public Sprite[] bodySprite;

    //public SpriteRenderer srBody;

    Animator anim;
    Joystick joystick;
    Rigidbody2D rb;
    [SerializeField] int speed = 5;
    
    [SerializeField] private float reboot;
    public float damage = 1;
    public bool ground;
    bool attack = true;
    bool block = true;
    public Transform attackPos;
    public float attackRange;
    public LayerMask isEnemy;
    public float health = 10;
    float startHealth;
    public float attackRate = 2f;
    float nextAttackTime = 0f;

    private void Start()
    {
        
        startHealth = health;
        anim = GetComponent<Animator>();
        joystick = GameObject.Find("Joystick").GetComponent<Joystick>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (joystick.Horizontal == 0)
            anim.SetBool("Walk", false);
        else
            anim.SetBool("Walk", true);
        transform.Translate(transform.right * joystick.Horizontal * speed * Time.fixedDeltaTime);
        if (joystick.Vertical > 0.5f)
            Jump();
        Flip();
    }
    void Flip()
    {
        if (joystick.Horizontal > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        if (joystick.Horizontal < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void Damage(float damage)
    {
        health -= damage;
        GameObject.Find("HealthBar").GetComponent<Image>().fillAmount = health / startHealth;
        if (health <= 0)
        {
            anim.SetBool("Die", true);
            Camera.main.GetComponent<UISetting>().Lose();

        }
    }
    public void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            if (attack == true)
            {
                nextAttackTime = Time.time + 1f / attackRate;
                attack = false;
                anim.SetTrigger("Attack");
                Collider2D[] enemiscToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, isEnemy);
                for (int i = 0; i < enemiscToDamage.Length; i++)
                {
                    enemiscToDamage[i].GetComponent<Enemy>().Damage(damage);
                }
                Invoke("AttackReset", reboot);
            }
        }
    }

    void AttackReset()
    {
        attack = true;
    }
    public void Shield()
    {
        if (block == true)
        {
            block = false;

            Invoke("OffShield", 1.5f);
        }
    }
    void OffShield()
    {
        block = true;
    }
   
    void Jump()
    {
        if (ground == true)
        {
            rb.AddForce(transform.up * 10, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
}
