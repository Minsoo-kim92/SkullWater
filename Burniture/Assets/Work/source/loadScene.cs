﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Threading;
public class loadScene : MonoBehaviour {

    public DateTime EndNow = DateTime.Now;
    public int EndStatus = 0;
    private GameObject Toast;
    private float screenx, screeny;

    // Use this for initialization
    void Start () {

        //Toast = GameObject.Find("Toast");

        //Toast = GameObject.FindWithTag("Toast");

        //Toast.SetActive(false);
        Toast = GameObject.Find("Toast");
    }
	
	// Update is called once per frame
	void Update () {
        DateTime now = DateTime.Now;//시간 계속 갱신
        TimeSpan datediff = now - EndNow;
        

        if (EndStatus == 1 && datediff.Seconds == 2)
            EndStatus = 0;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EndStatus == 0)
            {
                EndNow = DateTime.Now;//백키 누른 시간을 저장
                EndStatus = 1;
                Toast.SetActive(true);
                Toast.transform.GetChild(0).GetComponent<Text>().text = "한번 더 누르면 종료됩니다.";
                //삭제해주는코드 
                //Invoke("FalseActive", 3);
            }
            else if (EndStatus == 1)
            {
                Application.Quit();
            }
        }
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void FalseActive()
    {
        Toast.SetActive(false);
    }
}
