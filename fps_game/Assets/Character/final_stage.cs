using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class final_stage : MonoBehaviour
{
    int Key_Random;
    bool is_key = true;
    public GameObject Key_prefab;
    private NavMeshAgent nvAgent;
    Animator ani_enemy;
    public int enemy_hp;
    private float distance; //거리 변수
    private Transform playerTransform;
    private Transform tf;
    GameObject target = null; // 플레이어 쳐다보기위해 

    // Start is called before the first frame update
    void Start()
    {
        Key_Random = Random.Range(0, 11);
        ani_enemy = GetComponent<Animator>();
        tf = this.gameObject.GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        target = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(tf.position, playerTransform.position);//좀비와 플레이어의 위치거리 
        if (distance >= 1.5f && distance <= 30.0f)
        {
            this.transform.LookAt(target.transform.position); //플레이어 쳐다보기 
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
        if (other.tag == "Player") //player가 닿을경우
        {
            StartCoroutine(AttackDelay(0.1f));
        }

        if (other.tag == "bullet")//bullet가 닿을경우
        {
            enemy_hp--;
            if (enemy_hp <= 0)
            {
                if ( is_key &&Key_Random == 1)//
                {
                    Key_prefab = Instantiate(Key_prefab, tf.transform.position, tf.transform.rotation);
                    is_key = false;
                }
                ani_enemy.SetTrigger("is_die");
                Destroy(this.gameObject, 0.5f);
            }
        }
    }
    IEnumerator AttackDelay(float attackTime)
    {
        yield return new WaitForSeconds(attackTime);
        ani_enemy.SetTrigger("is_attack");
    }
}
