using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turret : MonoBehaviour
{
    [SerializeField] Transform m_tfGunBody = null;
    [SerializeField] float m_range = 0f; //사정거리 
    [SerializeField] LayerMask m_layerMask = 0; //특정 레이어만 공격
    [SerializeField] float m_spinSpeed = 0f; //적발견시 포신회전
    [SerializeField] float m_fireRate = 0f;
    bool is_key = false;
    bool is_key2 = false;
    bool turret_On = false;
    float m_currentFireRate;
    public Transform bullet;
    

    Transform m_tfTarget = null;// 공격대상 transform
    void SearchEnemy() //적 찾기 
    {
        Collider[] t_cols = Physics.OverlapSphere(transform.position, m_range, m_layerMask);//적들의 위치 저장
        Transform t_shortestTarget = null; //가장 짧은 적의 위치저장
        if (t_cols.Length > 0)
        {
            float t_shortestDistance = Mathf.Infinity;//가장 짧은것을 찾기위해 가장 긴것 넣기
            foreach(Collider t_colTarget in t_cols)
            {
                float t_distance = Vector3.SqrMagnitude(transform.position - t_colTarget.transform.position);//제곱반환(실제거리*실제거리)
                if(t_shortestDistance > t_distance)
                {
                    t_shortestDistance = t_distance;
                    t_shortestTarget = t_colTarget.transform;
                }
            }
        }
        m_tfTarget = t_shortestTarget;//최종타켓 저장
    }
    // Start is called before the first frame update
    void Start()
    {
        m_currentFireRate = m_fireRate;
    }

    // Update is called once per frame
    void Update()
    {
         
           
        if (is_key )
        {
            SearchEnemy(); // 열쇠를 가지고 있으면 함수 부르기 
        }
        if (m_tfTarget == null)//적 찾기전 회/
            m_tfGunBody.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
        else
        {
            //Quaternion t_lookRotation = Quaternion.LookRotation(m_tfTarget.position);
            Quaternion t_lookRotation = Quaternion.LookRotation(m_tfTarget.position - this.gameObject.transform.position);//LookRotation - 특정좌표를 바라보게 만드는 회전-
            Vector3 t_euler = Quaternion.RotateTowards(m_tfGunBody.rotation, t_lookRotation, m_spinSpeed * Time.deltaTime).eulerAngles;// a~b까지 c의 속도로 회/
            m_tfGunBody.rotation = Quaternion.Euler(0, t_euler.y, 0);//포신이 y축으로만 회전하기위/

            Quaternion t_fireRotation = Quaternion.Euler(0, t_lookRotation.eulerAngles.y, 0);//터렛이 조준해야 될 최종 방햐
            if (Quaternion.Angle(m_tfGunBody.rotation, t_fireRotation) < 5f)//포신이 가지고 있는 위치 차이가 5이하면 사격
            {
                m_currentFireRate -= Time.deltaTime;
                if(m_currentFireRate <= 0)
                {
                    m_currentFireRate = m_fireRate;
                    GameObject spawn_point = GameObject.Find("Cannon_1");
                    Transform prefab_bullet = Instantiate(bullet, spawn_point.transform.position, spawn_point.transform.rotation);
                    prefab_bullet.GetComponent<Rigidbody>().AddForce(spawn_point.transform.forward * 4000.0f);
                }
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && is_key2 )
        {
            is_key = true;
        }
    }
    public void Key(bool Key)
    {
        is_key2 = Key;
    }

}
