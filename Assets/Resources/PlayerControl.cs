using JetBrains.Annotations;
using NUnit.Framework;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEditor;

public class PlayerControl : MonoBehaviour
{
    public GameCore gameCore;
    public Rigidbody2D rb2d;
    public Vector2 constVec = new Vector2();
    Vector2 calVec = new Vector2();

    public GameObject JudgeCircle;

    [Header("Move System")]
    public float moveSpeed = 7f;
    public bool isRunning = false;
    public bool moveable = true;

    [Header("Judge Circle System")]
    public float Rad = 1.2f;
    public int judgeFrame = 10;
    public GameObject target;
    public float limitedDistance = 10f;


    [Header("Following Soul system")]
    public GameObject soulPrefab;
    public List<GameObject> followingTargets;
    public float distanceX = 1.2f;
    public float distanceY = 0.1f;
    static public int playerFacingPos = 1; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nullCheck();
        moveable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameCore.IsGameGoing)
        {
            if (moveable)
            {
                PlayerMove();
                StartFight();
                //fallowingSoulMakeALine();

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GetOneSoul();
                }

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    isRunning = true;
                    moveSpeed = 14f;
                }
                else
                {
                    isRunning = false;
                    moveSpeed = 7f;
                }
            }
            else
            {
                calVec = Vector2.zero;
                rb2d.linearVelocity = calVec;
            }
        }
    }

    public void StartFightRequest(Eleable eObject)
    {
        gameCore.StartFightCoroutine(eObject);

    }

    public void GetOneSoul()
    {
        GameObject obj = Instantiate(soulPrefab);
        followingTargets.Add(obj);
        fallowingSoulMakeALine();
    }
    public void GetOneSoul(Vector2 pos)
    {
        GameObject obj = Instantiate(soulPrefab, pos, Quaternion.identity);
        followingTargets.Add(obj);
        fallowingSoulMakeALine();
    }

    public void StartFight()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Make a area and judge if there's a eleable object in it, using a tag?

            // 如果要轉換成世界座標（2D情境）
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            worldPosition.z = 0; // 如果是2D場景，通常把z軸設成0
            //Debug.Log("滑鼠世界座標：" + worldPosition);


            //生成swap用的判定區域
            GameObject judgeArea =  Instantiate(JudgeCircle, worldPosition, Quaternion.identity);
            judgeArea.GetComponent<JudgeCircle>().playerControl = this;
            CircleCollider2D cirCol2d = judgeArea.GetComponent<CircleCollider2D>();
            cirCol2d.radius = Rad;
            cirCol2d.offset = Vector2.zero;

            judgeArea.GetComponent<JudgeCircle>().lastFrame = judgeFrame;
        }
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

    bool isInputtingX = false;
    bool isInputtingY = false;
    public void PlayerMove()
    {

        isInputtingX = false;
        isInputtingY = false;

        if (Input.GetKey(KeyCode.W))
        {
            calVec = new Vector2(calVec.x, 1 * moveSpeed);
            isInputtingY = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            calVec = new Vector2(calVec.x, -1 * moveSpeed);
            isInputtingY = true;
        }
        if (!isInputtingY)
        {
            calVec.y = 0;
        }


        if (Input.GetKey(KeyCode.A))
        {
            calVec = new Vector2(-1*moveSpeed, calVec.y);
            playerFacingPos = 1;
            isInputtingX = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            calVec = new Vector2(1 * moveSpeed, calVec.y);
            playerFacingPos = -1;
            isInputtingX = true;
        }
        if (!isInputtingX)
        {
            calVec.x = 0;
        }

        rb2d.linearVelocity = calVec;
    }

    public void nullCheck()
    {
        if (gameCore == null)
        {
            gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        }
        if (rb2d == null)
        {
            rb2d = gameObject.GetComponent<Rigidbody2D>();
        }
    }

}
