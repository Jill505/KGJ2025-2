using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;

public class Teacher : MonoBehaviour
{
    public GameCore gameCore;
    public GameObject soulPrefab;
    public bool moveable = false;

    [Header("teacher move sys")]
    public float FacingPos = 1f;
    public GameObject moveAreaA;
    public GameObject moveAreaB;
    public float speed = 6f;

    public Vector2 targetPos = Vector2.zero;


    [Header("teacher")]
    public List<GameObject> followingTargets;

    public teacherSensor the_teacherSensor;

    [Header("teacher fight")]
    public GameObject teacherFightCanvas;


    public Image showProcessImage;
    public Image durBar;
    public Image shell;

    public float durLim;
    public float durMax;
    public float durCCCCCC = 0.2f;

    public float strength = 7f;

    public float nowProcess;

    public float max = 20f;


    public float requireTime = 3f;
    public float inDurTime = 0f;


    public void moveToVec()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        //transform.position = Vector2.Lerp(transform.position, targetPos, 0.05f);
    }

    IEnumerator roll()
    {
        while (true)
        {
            while (moveable)
            {
                float ranX = Random.Range(moveAreaA.transform.position.x, moveAreaB.transform.position.x);
                float ranY = Random.Range(moveAreaB.transform.position.y, moveAreaA.transform.position.y);

                targetPos = new Vector2(ranX,ranY);

                if (ranX > transform.position.x)
                {
                    FacingPos = 1;
                }
                else
                {
                    FacingPos= -1;
                }

                Debug.Log("yyy" + targetPos);
                yield return new WaitForSeconds(7f);
            }
        }
    }
    public void RandomMove()
    {
        moveToVec();
    }

    public void GetOneSoul(Vector2 pos)
    {
        GameObject obj = Instantiate(soulPrefab, pos, Quaternion.identity);
        followingTargets.Add(obj);
        fallowingSoulMakeALine();
    }

    public void fallowingSoulMakeALine()
    {
        //Call all soul and pass them the argument
        for (int i = 0; i < followingTargets.Count; i++)
        {
            if (i == 0)
            {
                followingTargets[i].GetComponent<FallowingSoul>().FollowingTarget = gameObject;
            }
            else
            {
                followingTargets[i].GetComponent<FallowingSoul>().FollowingTarget = followingTargets[i - 1];
            }
        }
    }

    IEnumerator myFixedUpdate()
    {
        WaitForSeconds sec = new WaitForSeconds(0.25f);
        while (true)
        {
            GameObject target = the_teacherSensor.CheckClosestTarget(the_teacherSensor.detectedTargets);
            teacherJudgeIsFight(target);

            yield return sec;
        }
    }

    private void FixedUpdate()
    {
    }
    public void teacherJudgeIsFight(GameObject target)
    {
        //Debug.Log("Rolling");
        if (the_teacherSensor.playerInArea)
        {
            //Debug.Log("Rolling player");

            int roll = Random.Range(0, 150);
            if (roll <= 1 + (gameCore.playerControl.followingTargets.Count * 4))
            {
                //對玩家發起戰鬥
                Debug.Log(gameObject.name + "對玩家發起了戰鬥");
            }
        }
        else if (the_teacherSensor.detectedTargets.Count > 0)
        {
            //Debug.Log("Rolling gameo");

            int roll = Random.Range(0, 30);
            if (roll <= 2)
            {
                //對最近的目標發起戰鬥
                target.GetComponent<Eleable>().makeEle(gameObject);

            }
        }

    }
    public void teacherFight()
    {
        StartCoroutine(teacherFightCoroutine());
    }
    IEnumerator teacherFightCoroutine()
    {
        //開始
        teacherFightCanvas.SetActive(true);
        bool T = true;
        float percentage = 0;
        inDurTime = 0;
        strength = 6f;

        durLim = Random.Range(0.1f, 0.75f);
        durMax = durLim + durCCCCCC;

        requireTime = 3f + (followingTargets.Count * 2f);

        //中間的過程
        while (T)
        {
            //輸入
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                nowProcess += 1f;

            }
            percentage = nowProcess / max;
            showProcessImage.fillAmount = percentage;

            //掙扎
            nowProcess -= strength * Time.deltaTime + (Random.Range(-1, 3)) ;
            //顯示
            durBar.fillAmount = durMax;
            shell.fillAmount = durLim;


            //區間時間判斷
            if (nowProcess >= durLim && nowProcess <= durMax)
            {
                inDurTime += Time.deltaTime;
                Debug.Log(inDurTime + "|||| 正在進行teacher fight!");
            }

            //檢查跳出
            if (inDurTime > requireTime)
            {
                //戰鬥結束
                T = false;
            }

            strength += Random.Range(0f, 0.3f) * Time.deltaTime;
            if (strength >= 20f)
            {
                strength = 20f;
            }
            yield return null;
        }
        

        //結束
        teacherFightCanvas.SetActive(false);
        yield return null;
        //發放獎勵
        for (int i = 0; i < followingTargets.Count; i++)
        {
            gameCore.playerControl.GetOneSoul();
        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        StartCoroutine(myFixedUpdate());

        moveable = true;

        StartCoroutine(roll());
    }

    // Update is called once per frame
    void Update()
    {
        RandomMove();
    }
}
