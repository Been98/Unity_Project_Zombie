using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerCtrl : MonoBehaviour
{
    private int gun_bullet = 30; //탄 수
    
    private float timer = 0.0f;
    public AudioClip button_sound;
    public AudioClip reroad_sound;
    public AudioClip Player_hit_sound;
    public Transform bullet;// 총알 
    public GameObject playerlook; // 플레이어 보는곳(총 나가는곳)
    public Text bullet_ui; //탄창수 화면에 표시 
    public Text not_key; //열쇠가 없을때 포탈을 타면
    public Texture2D icon = null; //총알 아이콘 
    //변수 선언
    private float h = 0.0f;//가로  x축
    private float v = 0.0f;//세로 y축
    private Transform tr;//이동속도 변수
    Animator ani_player; //플레이어 애니메이터 
    CameraEffect CameraEffect; //피격시 카메라 흔들림을 위해 스크립트참조
    FadeOut fadeout;
    turret turret;
    [Header("플레이어 설정")]
    public float moveSpeed;//속도
    public Image Hp_bar; // hp바 이미지 
    private float hp =1.0f; // 내 hp 
    private bool is_Key = false;  // key 획득 여부
    private bool is_key2 = false;
    [SerializeField] private playerMouseRotate mouseRotate;//마우스 회전에 관한것들 저장하는 변수(데이터 타입 playerMouseRotate)
    [SerializeField] private Camera _camera;//카메라 에 관한 것들 저장하는 변수(데이터 타입 Camera)


    // Start is called before the first frame update
    void Start()
    {
        CameraEffect = GameObject.Find("Main Camera").GetComponent<CameraEffect>();
        tr = GetComponent<Transform>();//Transform 클래스의 기능을 추출해 tr에 대입(저장)
        _camera = GetComponentInChildren<Camera>();//메인카메라의 정보 대입
        mouseRotate.Init(tr, _camera.transform);//playerMouseRotate클래스의 메소드 lnit에 값 전달
        ani_player = GetComponent<Animator>();
        bullet_ui.text =  gun_bullet+"/30";
       
    }
    // Update is called once per frame
    void Update()
    {
        RotateView();//회전
        PlayerMove();//움직임
        PlayerShoot();
        PlayerDie();
        timer += Time.deltaTime;
        
    }
    private void RotateView()
    {
        mouseRotate.LookRotation(tr, _camera.transform);

    }

    void PlayerMove()//움직임함수
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        Vector3 MoveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(MoveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
        //
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            ani_player.SetBool("ani_run", true);
        else
            ani_player.SetBool("ani_run", false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "is_Key")
        {
            is_Key = true;
        }
        if(other.tag == "is_gold")
        {
            turret = GameObject.Find("turret").GetComponent<turret>();
            is_key2 = true;
            turret.Key(is_key2);
        }
        if (other.tag == "enemy")
        {
            if (hp > 0)
            {
                AudioSource.PlayClipAtPoint(Player_hit_sound, this.transform.position);
                StartCoroutine(CameraEffect.Shake(0.1f, 0.2f));
                hp -= 0.1f;
                Hp_bar.fillAmount -= 0.1f;
            }
        }
        if (other.tag == "stage6") //prefab들 
        {
            if (hp > 0)
            {
                AudioSource.PlayClipAtPoint(Player_hit_sound, this.transform.position);
                StartCoroutine(CameraEffect.Shake(0.1f, 0.2f));
                hp -= 0.1f;
                Hp_bar.fillAmount -= 0.1f;
            }
        }
        if (other.tag == "potal1" && is_Key == true)
        {
            transform.position = new Vector3(-3.5f, 0.0f, -150.0f);
            hp = 1.0f;
            Hp_bar.fillAmount = 1.0f;
            is_Key = false;
        }
        else if(other.tag=="potal1"&& is_Key ==false)
        {
            StartCoroutine(KeyDelay(2.0f));
        }
        if (other.tag == "potal2" && is_Key == true)
        {
            transform.position = new Vector3(-153.0f, 0.0f, 115.0f);
            hp = 1.0f;
            Hp_bar.fillAmount = 1.0f;
            is_Key = false;
        }
        else if (other.tag == "potal2" && is_Key == false)
        {
            StartCoroutine(KeyDelay(2.0f));
        }
        if (other.tag == "potal3" && is_Key == true)
        {
            transform.position = new Vector3(89.0f, 0.0f, 13.0f);
            hp = 1.0f;
            Hp_bar.fillAmount = 1.0f;
            is_Key = false;
        }
        else if (other.tag == "potal3" && is_Key == false)
        {
            StartCoroutine(KeyDelay(2.0f));
        }
        if (other.tag == "potal4" && is_Key == true)
        {
            transform.position = new Vector3(96.0f, 0.0f, 233.0f);
            hp = 1.0f;
            Hp_bar.fillAmount = 1.0f;
            is_Key = false;
        }
        else if (other.tag == "potal4" && is_Key == false)
        {
            StartCoroutine(KeyDelay(2.0f));
        }
        if (other.tag == "potal5" && is_Key == true)
        {
            hp = 1.0f;
            Hp_bar.fillAmount = 1.0f;
            is_Key = false;
            SceneManager.LoadScene(6, LoadSceneMode.Single);
        }
        else if (other.tag == "potal5" && is_Key == false)
        {
            StartCoroutine(KeyDelay(2.0f));
        }
    }
    void PlayerShoot()
    {
        if (Input.GetButtonDown("Fire1") && gun_bullet != 0 && timer >= 2.0f) //장전타임 주기위해 
        {
            GameObject spawn_point = GameObject.Find("sp_bullet");
            Transform prefab_bullet = Instantiate(bullet, spawn_point.transform.position, spawn_point.transform.rotation);
            prefab_bullet.GetComponent<Rigidbody>().AddForce(playerlook.transform.forward * 4000.0f);
            AudioSource.PlayClipAtPoint(button_sound, this.transform.position);
             gun_bullet--;
            bullet_ui.text =  gun_bullet + "/30";
        }
        if (gun_bullet !=30 &&Input.GetKeyDown(KeyCode.R) && timer >= 2.0f) //r키 장전 
        {
            timer = 0f;
            gun_bullet = 30;
            bullet_ui.text =   gun_bullet + "/30";
            AudioSource.PlayClipAtPoint(reroad_sound, this.transform.position);
        }
        if (gun_bullet == 0 && timer >= 2.0f) // 총알 다 떨어지면 장전 
        {
            timer = 0f;
            gun_bullet = 30;
            bullet_ui.text =  gun_bullet + "/30";
            AudioSource.PlayClipAtPoint(reroad_sound, this.transform.position);
        }
    }
    void PlayerDie()
    {
        if(hp <= 0)
        {
            fadeout = GameObject.Find("Canvas").GetComponent<FadeOut>();
            fadeout.Fade();
            StartCoroutine(DieScene(2.0f));
        }
    }
    IEnumerator KeyDelay(float Keytime)
    {
        not_key.text = "열쇠가 필요합니다!";
        yield return new WaitForSeconds(Keytime);
        not_key.text = "";
    }
    IEnumerator DieScene(float die)
    {
        yield return new WaitForSeconds(die);
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
    void OnGUI()
    {
        GUI.DrawTexture(new Rect(1240, Screen.height-135, 64, 64), icon);
        
    }
}