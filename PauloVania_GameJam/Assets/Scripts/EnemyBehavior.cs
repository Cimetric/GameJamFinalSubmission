using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Movement Range")]
    [SerializeField] GameObject edge1;
    [SerializeField] GameObject edge2;

    [SerializeField] GameObject detectionEdge1;
    [SerializeField] GameObject detectionEdge2;
    [Header("Movement")]
    [SerializeField] GameObject Enemy;
    [SerializeField] [Range(0.5f, 5f)] float speed;
    
    GameObject player;
    PlayerController p;
    Rigidbody2D rb;
    [SerializeField] bool right = false;
    bool player_inside;
    [Header("Health")]
    [SerializeField] [Range(1,5)] int lifes =1 ;
    int actual_lifes;
    bool hurt = false;
    [SerializeField] [Range(0,5)] float hurt_delay = 1;
    Enemy_Hurt EH;
    float targetTime;
    SpriteRenderer SR;
    bool hurt_start = false;
    bool dead = false;
    [SerializeField] [Range(5, 25)] float hurt_thrust;
    float short_term_thrust;
    bool rightCheck = false;
    bool leftCheck = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player");
        SR = Enemy.GetComponent<SpriteRenderer>();
        EH = Enemy.GetComponent<Enemy_Hurt>();
        p = player.GetComponent<PlayerController>();
        rb = Enemy.GetComponent<Rigidbody2D>();
    }
    void Hurt(){
        if(EH.hurt){
            if(!hurt_start){
                hurt_start = true;
                actual_lifes --;
                if(actual_lifes ==0){
                    dead = true;
                    Enemy.GetComponent<Animator>().SetBool("Dead", true);
                }
                if(p.facingRight){
                    short_term_thrust = hurt_thrust;
                }
                else{
                    short_term_thrust = -hurt_thrust;
                }
                
            }
            SR.color = Color.red;
            targetTime += Time.deltaTime;
            if(targetTime>hurt_delay){
                EH.hurt = false;
            }
            
        }
        else{
            targetTime = 0;
            SR.color = Color.white;
            hurt_start = false;
        }
        if(dead){
            short_term_thrust = 0;
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Hurt();
        if(player.transform.position.x > detectionEdge1.transform.position.x && player.transform.position.x < detectionEdge2.transform.position.x){
            player_inside = true;
            if(player.transform.position.x > Enemy.transform.position.x){
                rb.linearVelocity = new Vector2(1*speed, rb.linearVelocity.y);
                right = true;
            }
            else{
                rb.linearVelocity = new Vector2(-1*speed, rb.linearVelocity.y);
                right = false;
            }
        } else {
            if(Enemy.transform.position.x < edge1.transform.position.x){
                right=true;
            }
            else if(Enemy.transform.position.x > edge2.transform.position.x){
                right = false;
            }

            if(right){
                rb.linearVelocity = new Vector2(1*speed + short_term_thrust *4, rb.linearVelocity.y);
            }
            else{
                rb.linearVelocity = new Vector2(-1*speed + short_term_thrust *4, rb.linearVelocity.y);
            }
        }
        if(!right && !rightCheck){
            Enemy.transform.Rotate(0,180,0);
            rightCheck = true;
            leftCheck = false;
        }
        else if(right && rightCheck){
            Enemy.transform.Rotate(0,180,0);
            rightCheck = false;
            leftCheck = true;
        }
    }
}
