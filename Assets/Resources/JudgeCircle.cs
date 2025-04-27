using System.Collections.Generic;
using UnityEngine;

public class JudgeCircle : MonoBehaviour
{
    public CircleCollider2D col2d;
    public GameCore gameCore;
    public PlayerControl playerControl;

    public GameObject returnTarget;
    List<GameObject> detectedTargets = new List<GameObject>();

    int frameCount = 0;
    public int lastFrame = 2;

    public bool dected = false;

    // Start is called once before the first exe
    void Start()
    {
        col2d = gameObject.GetComponent<CircleCollider2D>();
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();
    }

    // Update is called once per frame
    void Update()
    {
        frameCount += 1;      
        
        if (frameCount >= lastFrame)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "teacher")
        {

        }
        else if (collision.gameObject.tag == "eleObject")
        {
            if (!detectedTargets.Contains(collision.gameObject))
            {
                detectedTargets.Add(collision.gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "teacher")
        {

        }
        else if (collision.gameObject.tag == "eleObject")
        {
            if (!detectedTargets.Contains(collision.gameObject))
            {
                detectedTargets.Add(collision.gameObject);
            }
        }
    }

    private void OnDestroy()
    {
        if (detectedTargets.Count > 0)
        {
            CheckClosestTarget(detectedTargets);
            dected = true;

            float distance = Vector2.Distance(returnTarget.transform.position, playerControl.gameObject.transform.position);
            if (distance > playerControl.limitedDistance)
            {
                //超出距離限制
                Debug.Log("超出距離限制");
            }
            else
            {
                Debug.Log("範圍內有目標, 回傳最近目標，" + returnTarget.name);
                playerControl.target = returnTarget;

                //Start a fight
                playerControl.StartFightRequest(returnTarget.GetComponent<Eleable>());
            }
        }
        else
        {
            Debug.Log("範圍內沒有目標");
        }
    }


    private void LateUpdate()
    {
          
    }


    private void CheckClosestTarget(List<GameObject> targets)
    {
        if (targets == null || targets.Count == 0)
            return;

        GameObject closest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject target in targets)
        {
            float distance = Vector3.Distance(currentPosition, target.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = target;
            }
        }

        if (closest != null)
        {
            returnTarget = closest;
        }
    }
}
