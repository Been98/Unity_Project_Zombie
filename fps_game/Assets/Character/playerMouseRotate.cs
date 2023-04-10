using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//y축 x축 시선 고정식

[System.Serializable]
public class playerMouseRotate
{
    public float xSensitivity = 2f;//x축 민감도
    public float ySensitivity = 2f;//y축 민감도
    public bool clampVR = true;//vertical 회전 제한 여부
    public float minRotateX = -70f;//x축 최소 앵글
    public float maxRotateY = 70f;//y축 최소 앵글
    public bool smooth = true;//부드러움 여부
    public float smoothTime = 5f;//브드러움 강도설정
    private Quaternion playerTargetRotate;//플래이어 회전 계산저장 변수
    private Quaternion cameraTargetRotate;//카메라 회전 계산 저장 변수
    

    public void Init(Transform player, Transform camera)//각 회전 값 삽입
    {
        playerTargetRotate = player.localRotation;
        cameraTargetRotate = camera.localRotation;
    }

    public void LookRotation(Transform player, Transform camera)
    {
        float yRotate = Input.GetAxis("Mouse X") * xSensitivity;
        float xRotate = Input.GetAxis("Mouse Y") * ySensitivity;//입력된 크기의 2배해줌

        playerTargetRotate *= Quaternion.Euler(0f, yRotate, 0f);//플레이어의 회전값 계산
        cameraTargetRotate *= Quaternion.Euler(-xRotate, 0f, 0f);//카메라의 회전값 계산

        if (clampVR)
            cameraTargetRotate = ClampRotationX(cameraTargetRotate);

        if (smooth)
        { //시작 회전 각 부터 목표의 회전을 일정 시간을 두어 실행합니다. 
            player.localRotation = Quaternion.Slerp(player.localRotation, playerTargetRotate, smoothTime * Time.deltaTime);
            camera.localRotation = Quaternion.Slerp(camera.localRotation, cameraTargetRotate, smoothTime * Time.deltaTime);
        }
        else
        {
            player.localRotation = playerTargetRotate;
            camera.localRotation = cameraTargetRotate;
        }
    }
    private Quaternion ClampRotationX(Quaternion quat)
    {
        quat.x /= quat.w;
        quat.y /= quat.w;
        quat.z /= quat.w;
        quat.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(quat.x);//아크탄젠트 좌표를 이용해 각도 구하기 Rad2Deg = 라디안->도수법으로
        angleX = Mathf.Clamp(angleX, minRotateX, maxRotateY);//최대값, 최소값을 정해서 넘지않도록  
        quat.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);//탄젠트를 구합니다. 

        return quat;
    }

}

