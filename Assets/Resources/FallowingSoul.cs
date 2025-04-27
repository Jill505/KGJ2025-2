using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FallowingSoul : MonoBehaviour
{
    public GameObject FollowingTarget;
    public int myFollwingIndex = 0;

    public float disX = 1f;
    public float disY;
    public float offestSpeed = 0.2f;

    public bool isFollowingTeacher = false;
    public Teacher teacher;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FollowingTarget != null)
        {
            if (teacher == null)//代表是玩家
            {
                Vector2 offest = new Vector2(disX * (myFollwingIndex + 1), 0) * PlayerControl.playerFacingPos;
                transform.position = Vector2.Lerp(transform.position, (FollowingTarget.transform.position) + (Vector3)offest, offestSpeed);
            }
            else//代表是老師
            {
                Debug.Log("TeacherOBject");
                Vector2 offest = new Vector2(disX * (myFollwingIndex + 1), 0) * teacher.FacingPos;
                transform.position = Vector2.Lerp(transform.position, (FollowingTarget.transform.position) + (Vector3)offest, offestSpeed);
            }
        }
    }

    public void kys()
    {
        Destroy(gameObject);
    }


    public SpriteRenderer sr;
    public void makeFade()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(makeFadeCoroutineR());
    }
    IEnumerator makeFadeCoroutineR()
    {
        //gameObject.GetComponent<Collider2D>().enabled = false;
        float alpha = 1f;

        for (int i = 0; i < 20; i++)
        {
            alpha -= 0.05f;
            yield return new WaitForSeconds(0.01f);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
        yield return new WaitForSeconds(0.1f);

        Destroy(gameObject);
    }
}
