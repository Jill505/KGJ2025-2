using NUnit.Framework;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class GameCore : MonoBehaviour
{
    static public bool IsGameGoing = true;
    static public bool AllowEle = true;

    [Header("Game System")]
    public int playerStudentIndex;
    public int p2Score;
    public int p3Score;
    public int p4Score;

    public PlayerControl playerControl;

    [Header("Fight System")]
    public GameObject FightTarget;
    public GameObject FightCanvas;

    public Image fightFill;

    public GameObject linerPrefab;
    public GameObject liner;

    public float fightScaleConstX = 1f;
    public float fightScaleConstY = 0.2f;

    float clickCal = 0;

    float studentStr = 1f;

    [Header("student spawn system")]
    public GameObject studentAreaA;
    public GameObject studentAreaB;
    public GameObject[] studentPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(ranSpawnCycle());

        for (int i = 0; i < 10; i++)
        {
            studentSpawner();
        }
    }

    public Text theText;
    public void UIupdate()
    {
        string str = "玩家分數：" + playerStudentIndex + "/30\n" + "綠老師分數：" + p2Score + "/30\n" + "藍老師分數：" + p3Score + "/30\n" + "紅老師分數：" + p4Score + "/30\n";

        theText.text = str;
    }

    // Update is called once per frame
    void Update()
    {
        UIupdate();
    }

    IEnumerator ranSpawnCycle()
    {
        while (true)
        {
            studentSpawner();
            yield return new WaitForSeconds(Random.Range(2f, 8f));
        }
    }

    public void studentSpawner()
    {
        float ranX = Random.Range(studentAreaA.transform.position.x, studentAreaB.transform.position.x);
        float ranY = Random.Range(studentAreaA.transform.position.y, studentAreaB.transform.position.y);
        Vector2 ranVec = new Vector2(ranX, ranY);

        int index = Random.Range(0, studentPrefab.Length);

        Instantiate(studentPrefab[index], ranVec, Quaternion.identity);
    }

    public void teacherFightPlayer(Teacher teacher, Eleable eObject)
    {
        StartCoroutine(teacherFightPlayerCoroutine(teacher, eObject));
    }
    IEnumerator teacherFightPlayerCoroutine(Teacher teacher, Eleable eObject)
    {
        yield return null;
        if (eObject.students != null)
        {
            eObject.students.moveable = false;
        }

        //製造射線

        Vector2 pos = (teacher.gameObject.transform.position + eObject.transform.position) / 2;

        Vector2 dirVector = eObject.transform.position - teacher.gameObject.transform.position;
        float dir = Mathf.Atan2(dirVector.y, dirVector.x) * Mathf.Rad2Deg;

        float dist = Vector2.Distance(teacher.gameObject.transform.position, eObject.gameObject.transform.position);

        liner = Instantiate(linerPrefab, pos, Quaternion.Euler(0, 0, dir));
        liner.transform.localScale = new Vector3(fightScaleConstX * dist, fightScaleConstY, 0);

        //計時隨機
        yield return Random.Range(1.2f, 6f);

        //刪除射線、目標、學生，並且rol +1
        eObject.makeFadeAndDel();

    }




    public void StartFightCoroutine(Eleable eObject)
    {
        StartCoroutine(fightCoroutine(eObject));
    }

    IEnumerator fightCoroutine(Eleable eObject)
    {
        AllowEle = false;
        bool succ = false;
        playerControl.moveable = false;

        if (eObject.students != null)
        {
            eObject.students.moveable = false;
        }

        clickCal = 0;
        int targetClickNum = Random.Range(6, 12);

        Vector2 pos = (playerControl.transform.position + eObject.transform.position) / 2;

        Vector2 dirVector = eObject.transform.position - playerControl.transform.position;
        float dir = Mathf.Atan2(dirVector.y, dirVector.x) * Mathf.Rad2Deg;

        float dist = Vector2.Distance(playerControl.transform.position, eObject.gameObject.transform.position);

        liner = Instantiate(linerPrefab, pos, Quaternion.Euler(0,0,dir));
        liner.transform.localScale = new Vector3(fightScaleConstX *dist, fightScaleConstY, 0);


        //open canvas?
        FightCanvas.SetActive(true);

        bool t = true;
        float timeCal = 0;


        if (eObject.students != null)
        {
            eObject.students.Strength = studentStr;
        }


        while (t)
        {
            clickCal -= studentStr * Time.deltaTime;


            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                clickCal++;
            }

            fightFill.fillAmount = (float)clickCal / (float)targetClickNum;

            if (clickCal >= targetClickNum)
            {
                t = false;
                succ = true;
            }

            yield return null;
        }

        yield return null;
        if (succ)
        {
            Debug.Log("成功，快去聽<<溫室雜草>>！");
            //playerControl.GetOneSoul();
            //Del Target;
            eObject.makeFadeAndDel();
        }
        else
        {

        }

        //open
        playerControl.moveable = true;
        FightCanvas.SetActive(false);
        Destroy(liner);
        AllowEle = true;
    }
}
