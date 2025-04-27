using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class teacherSensor : MonoBehaviour
{
    public List<GameObject> detectedTargets = new List<GameObject>();
    public GameCore gameCore;
    public PlayerControl playerControl;

    public bool playerInArea = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameCore = GameObject.Find("GameCore").GetComponent<GameCore>();        
        playerControl = gameCore.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        detectedTargets.Clear();
        playerInArea = false;
    }
    void Update()
    {
       
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInArea = true;
            //Debug.Log("Target in area, player, from teacher" + transform.parent.name);
        }
        else if (collision.gameObject.tag == "eleObject")
        {
            //Debug.Log("Target in area, eleObject, from teacher" + transform.parent.name);
            playerInArea = false;
            if (!detectedTargets.Contains(collision.gameObject))
            {
                detectedTargets.Add(collision.gameObject);
            }
        }
        else
        {
            playerInArea=  false;
            //Debug.Log("Target dec tag missed");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInArea = true;
            //Debug.Log("Target in area, player, from teacher" + transform.parent.name);
        }
        else if (collision.gameObject.tag == "eleObject")
        {
            //Debug.Log("Target in area, eleObject, from teacher" + transform.parent.name);
            playerInArea = false;
            if (!detectedTargets.Contains(collision.gameObject))
            {
                detectedTargets.Add(collision.gameObject);
            }
        }
        else
        {
            playerInArea = false;
            //Debug.Log("Target dec tag missed");
        }
    }


    public GameObject CheckClosestTarget(List<GameObject> targets)
    {
        if (targets == null || targets.Count == 0)
            return null;

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
            //returnTarget = closest;
        }
        return closest;
    }
}
