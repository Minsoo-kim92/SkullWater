﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Moving : MonoBehaviour
{

    float FirstDistance;
    public float Speed = 15f;
    private int Cube_num = 0;
    private GameObject MoveCube;
    public GameObject Furn;
    private GameObject Arrow;
    private int TouchState = 0;
    private int ChangeStatus = 1;
    private int ColorChangeState = 0;
    private Color F_Color,T_Color;
    private GameObject PreCube; // 이전 큐브 저장
    private GameObject PreArrow;
    private int start_layer;
    private Transform MV;
    private int lotate_state;

    void Start()
    {
        lotate_state = 0;
        //start_layer = MoveCube.layer; // 여기도
    }
    void Update()
    {
        if (Input.touchCount == 0)              // 터치가 없으면
        {
            Cube_num = 0;                     // 선택된 것도 없음
            TouchState = 0;                   // 손가락이 떼지면 변화
        }

        if (Input.touchCount == 1)              // 화면에 터치한 손가락의 갯수가 한개일때
        {
            if ((Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved) && MoveCube != null)
            {
                if (ColorChangeState == 0)
                {
                    Furn = MoveCube.gameObject.transform.GetChild(1).gameObject;
                }
                T_Color = F_Color;
                T_Color.a = 0.5f;
                ColorChangeState = 1;
                Arrow= MoveCube.gameObject.transform.GetChild(0).gameObject;
                Arrow.layer = 2; // 레이캐스트 무시
            }
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if (ColorChangeState == 1)
                {
                    Furn = MoveCube.gameObject.transform.GetChild(1).gameObject;
                    //Furn.GetComponent<Renderer>().material.color = F_Color;
                    ColorChangeState = 0;
                }
                if (Arrow != null && Arrow.transform.parent.gameObject.name == MoveCube.name)
                {
                    Arrow.SetActive(true);
                    Arrow.layer = 0;
                }

            }
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position); // 손가락에서 화면안으로 레이저를쏨
            RaycastHit hit = new RaycastHit(); // 레이저가 맞을때를 hit라고 선언

            if (Physics.Raycast(ray, out hit) && Cube_num == 0) // 레이저가 오브젝트에 맞고, 아직 선택된 것이 없을때
            {
                if (hit.collider.gameObject.tag == "Cube")//터치된것이 큐브인지 확인
                {
                    Cube_num = 1;
                    PreCube = MoveCube;
                    MoveCube = hit.collider.gameObject;
                    ChangeStatus = 0;
                    Arrow = MoveCube.transform.Find("RotateArrow").gameObject; // 자식오브젝트 찾기
                    if (PreCube != null)
                    {
                        PreArrow = PreCube.transform.Find("RotateArrow").gameObject;
                        PreArrow.SetActive(false);
                    }
                }
                else if (Arrow != null && hit.collider.gameObject.tag != "Arrow")
                {
                    Arrow.SetActive(false);
                }
                if (hit.collider.gameObject.name == MoveCube.transform.Find("RotateArrow").gameObject.name)
                {
                    Arrow = MoveCube.transform.Find("RotateArrow").gameObject;
                    if (TouchState == 0)
                    {
                        TouchState = 1; // 누르고있는 동안 한번만 변화하기 위한 상태변수
                        Vector3 A = Arrow.transform.position;
                        MoveCube.transform.Rotate(0, 90, 0);

                        MV = MoveCube.transform;
                        Arrow.transform.position = new Vector3(A.x, A.y, A.z);
                        Arrow.transform.Rotate(0, -270, 0);

                        if (MV.eulerAngles.y >= 270 && MV.eulerAngles.y <= 271)
                        {
                            lotate_state = 3;
                            Debug.Log("270!!");
                        }
                        else if (MV.eulerAngles.y >= 0 && MV.eulerAngles.y <= 1)
                        {
                            lotate_state = 0;
                            Debug.Log("0!!");
                        }
                        else if (MV.eulerAngles.y >= 90 && MV.eulerAngles.y <= 91)
                        {
                            lotate_state = 1;
                            Debug.Log("90!!");
                        }
                        else if (MV.eulerAngles.y >= 180 && MV.eulerAngles.y <= 181)
                        {
                            lotate_state = 2;
                            Debug.Log("180!!");
                        }
                    }
                }
                else
                {
                    if (Arrow != null)
                        Arrow.SetActive(false);
                }
            }
            else
            {
                var touchDeltaPosition = (Vector3)Input.GetTouch(0).deltaPosition;
                //MoveCube.transform.Translate(touchDeltaPosition.x * Time.deltaTime * 20f, 0, touchDeltaPosition.y * Time.deltaTime * 20f); //드래그
                // modify
                if (lotate_state == 3)
                {
                    MoveCube.transform.Translate(touchDeltaPosition.y * Time.deltaTime * 20f, 0, -touchDeltaPosition.x * Time.deltaTime * 20f);
                    Debug.Log("Debug Lotate : 270!!");
                }
                else if (lotate_state == 1)
                {
                    MoveCube.transform.Translate(-touchDeltaPosition.y * Time.deltaTime * 20f, 0, touchDeltaPosition.x * Time.deltaTime * 20f);
                    Debug.Log("Debug Lotate : 90!!");
                }
                else if (lotate_state == 2)
                {
                    MoveCube.transform.Translate(-touchDeltaPosition.x * Time.deltaTime * 20f, 0, -touchDeltaPosition.y * Time.deltaTime * 20f);
                    Debug.Log("Debug Lotate : 180!!");
                }
                else
                {
                    MoveCube.transform.Translate(touchDeltaPosition.x * Time.deltaTime * 20f, 0, touchDeltaPosition.y * Time.deltaTime * 20f);
                    Debug.Log("Debug Lotate : 0!!");
                }
            }
        }

        else if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                if (Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position) > FirstDistance) // 위아래
                {
                    FirstDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    MoveCube.transform.Translate(0, MoveCube.transform.position.y * Time.deltaTime, 0);
                }
                else
                {
                    FirstDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
                    MoveCube.transform.Translate(0, -(MoveCube.transform.position.y * Time.deltaTime), 0);
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(1).phase == TouchPhase.Began)
            {
                FirstDistance = Vector2.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended && Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                FirstDistance = 0;
            }
        }
    }
}