using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class enemy_sp : MonoBehaviour
{
    public Transform[] points;
    //몬스터 프리팹을 할당할 변수\
    public GameObject monsterPrefab;
    FadeOut fadeout;
    //몬스터를 발생시킬 주기
    public float createTime;
    //몬스터의 최대 발생 개수
    public int maxMonster = 15;
    //게임 종료 여부 변수
    private bool isGameOver = false;
    private float timer = 180.0f;
    public Text time_ui;
    private int min = 0;
    bool sound = false;
    public AudioClip helicopter_sound;
    public GameObject heli;
    void Start()
    {
        
        points = GameObject.Find("SpawnPoint").GetComponentsInChildren<Transform>();

        if (points.Length > 0)
        {
            StartCoroutine(this.CreateMonster());
        }
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer >= 60)
        { 
            min++;
            timer -= 60;
        }
        if(timer <= 0)
        {
            min--;
            timer += 60;
        }
        if (min == 0 && timer <= 10.0f && !sound)
        {
            heli = Instantiate(heli,new Vector3(-70,30,0), this.transform.rotation);
            AudioSource.PlayClipAtPoint(helicopter_sound, this.transform.position);
            sound = true;
        }
        time_ui.text = string.Format("{0:D2} : {1:D2}", min, (int)timer);
        if (min == 0 && timer <= 1.0f)
        {
            
            fadeout = GameObject.Find("Canvas").GetComponent<FadeOut>();
            fadeout.Fade();
            StartCoroutine(clearScene(2.0f));
        }

    }
    IEnumerator CreateMonster()
    {
        while (!isGameOver)
        {

            int monsterCount = (int)GameObject.FindGameObjectsWithTag("stage6").Length;

            if (monsterCount < maxMonster)
            {

                yield return new WaitForSeconds(createTime);


                int idx = Random.Range(1, points.Length);
                Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }
    void OnTriggerEnter(Collider other) //스테이지 6으로 가서 생성하기위해 
    {

        if (other.tag == "Player")
        {
            isGameOver = false;
        }
    }
    IEnumerator clearScene(float die)
    {
        yield return new WaitForSeconds(die);
        SceneManager.LoadScene(5, LoadSceneMode.Single);
    }
}