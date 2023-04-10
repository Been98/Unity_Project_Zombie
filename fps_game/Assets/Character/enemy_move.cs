using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_move : MonoBehaviour
{
    private Transform tf; // 좀비 위치변수
    private Transform playerTransform; //플레이어 위치변수 
    private NavMeshAgent nvAgent;
    public int enemy_hp = 2;
    
    private float timer; //적이 플레이어와 붙어있는 시간
    private float distance; //거리 변수
    public Transform target;
    public GameObject Key_prefab;
    private bool is_key = true;
    private int Key_Random;
    Animator ani_enemy;
    // Start is called before the first frame update
    void Start()
    {
        Key_Random = Random.Range(0, 3);
        tf = this.gameObject.GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        ani_enemy = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
        distance = Vector3.Distance(tf.position, playerTransform.position);//좀비와 플레이어의 위치거리 
        if (distance >= 1.5f && distance <= 15.0f)
        {
            this.transform.LookAt(target); //플레이어 쳐다보기 
            nvAgent.destination = playerTransform.position; // 플레이어 따라오기
            ani_enemy.SetBool("is_run", true);
        }

        else
        {
            ani_enemy.SetBool("is_run", false);

        }
        
    }
   void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" )
        {
            StartCoroutine(AttackDelay(0.1f));
        }
        
        if (other.tag == "bullet")
        {
            enemy_hp--;
            if (enemy_hp <= 0)
            {
                if (is_key && Key_Random == 1)
                {
                    Key_prefab = Instantiate(Key_prefab, tf.transform.position, tf.transform.rotation);
                    is_key = false;
                }
                ani_enemy.SetTrigger("is_die");
                Destroy(this.gameObject,1.0f);
            }
        }
    }
    IEnumerator AttackDelay(float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        ani_enemy.SetTrigger("is_attack");
    }
   
}
