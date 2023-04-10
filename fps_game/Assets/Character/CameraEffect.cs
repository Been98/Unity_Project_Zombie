using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffect : MonoBehaviour
{
    public Transform PlayerTr;
    // Start is called before the first frame update
    void Start()
    {
        PlayerTr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator Shake(float duration, float ShakeArrange)    //지속시간 범위 
    {
        Vector3 origin_Pos = transform.localPosition;   //부모위치 

        float time = 0.0f;

        while (time < duration)
        {

            float posX = Random.Range(-1f, 1f) * ShakeArrange;
            float posY = Random.Range(-1f, 1f) * ShakeArrange;

            transform.localPosition = origin_Pos + new Vector3(posX, posY, 1f);

            time += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = origin_Pos;
    }
}