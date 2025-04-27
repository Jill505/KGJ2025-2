using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHouse : MonoBehaviour
{
    public bool isInArea;
    public float rad = 3f;

    public GameObject p2;
    public GameObject p3;
    public GameObject p4;


    public GameCore gameCore;
    public PlayerControl playerControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        playerControl = gameCore.playerControl;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {

        if (collision.transform.gameObject.tag == "teacher")
        {
            Debug.Log("aaa");
            //save in;
            teacherSave(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "teacher")
        {
            Debug.Log("aaa");
            //save in;
            teacherSave(collision.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.tag == "teacher")
        {
            Debug.Log("aaa");
            //save in;
            teacherSave(collision.gameObject);
        }
    }
    public void teacherSave(GameObject teacher)
    {
        //Get teacher obj
        Teacher tec = teacher.GetComponent<Teacher>();
        while (tec.followingTargets.Count > 0)
        {
            GameObject swap = tec.followingTargets[0];
            tec.followingTargets.RemoveAt(0);
            Destroy(swap);
            //add 1 point;
            if (tec.myPlayerIndex == 2)
            {
                gameCore.p2Score += 1;
            }
            if (tec.myPlayerIndex == 3)
            {
                gameCore.p3Score += 1;
            }
            if (tec.myPlayerIndex == 4)
            {
                gameCore.p4Score += 1;
            }
        }

    }


    // Update is called once per frame
    void Update()
    {

        if (Vector2.Distance(p2.transform.position, transform.position) < rad)
        {
            teacherSave(p2);
        }
        if (Vector2.Distance(p3.transform.position, transform.position) < rad)
        {
            teacherSave(p3);
        }
        if (Vector2.Distance(p4.transform.position, transform.position) < rad)
        {
            teacherSave(p4);
        }


        //Debug.Log("Vector¡G" + Vector2.Distance(playerControl.transform.position, transform.position));
        if (Vector2.Distance(playerControl.transform.position, transform.position) < rad)
        {
            //Debug.Log("player is in area");
            isInArea = true;
            //allow to input the students
            if (playerControl.followingTargets.Count > 0)
            {
                //allow input
                StartCoroutine(studentSaveIn());
            }
        }
        else
        {
            isInArea = false;
            //Debug.Log("player is not in area");
        }

    }
    IEnumerator studentSaveIn()
    {
        bool clamp = true;
        float countT = 0;
        yield return null;
        while (isInArea && playerControl.followingTargets.Count > 0)
        {
            Debug.Log("is during saving student coroutine process");
            //startCountTime
            if (clamp)
            {
                countT += Time.deltaTime;
                //°õ¦æ¥\¯à

                if (countT > 3f)
                {
                    GameObject obj = playerControl.followingTargets[0];
                    playerControl.followingTargets.RemoveAt(0);

                    obj.GetComponent<FallowingSoul>().FollowingTarget = gameObject;

                    Destroy(obj, 0.8f);
                    gameCore.playerStudentIndex += 1;
                    obj.GetComponent<FallowingSoul>().makeFade();

                    countT = 0;
                }
            }

            yield return null;
        }
    }
}
