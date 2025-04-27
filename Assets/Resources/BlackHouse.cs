using System.Collections;
using UnityEngine;

public class BlackHouse : MonoBehaviour
{
    public bool isInArea;
    public float rad = 3f;

    public GameCore gameCore;
    public PlayerControl playerControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
        playerControl = gameCore.playerControl;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(playerControl.gameCore.transform.position, transform.position) < rad)
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
