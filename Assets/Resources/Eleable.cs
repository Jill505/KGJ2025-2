using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UIElements.Experimental;

public class Eleable : MonoBehaviour
{
    public Students students;

    public bool ableToBeEle = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        students = GetComponent<Students>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject targetPos;


    //Line renederer, make a line and try make some animation.
    public void makeEle(GameObject tracker)
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(makeEle2(tracker.gameObject.GetComponent<Teacher>()));
    }
    IEnumerator makeEle2(Teacher tracker)
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        float alpha = 1f;

        for (int i = 0; i < 20; i++)
        {
            alpha -= 0.05f;
            yield return new WaitForSeconds(0.01f);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
        yield return new WaitForSeconds(0.1f);

        tracker.GetOneSoul(gameObject.transform.position);
        Destroy(gameObject);
    }

    public SpriteRenderer sr;
    public void makeFadeAndDel()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(makeFadeCoroutine());
    }
    IEnumerator makeFadeCoroutine()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        float alpha = 1f;

        for (int i = 0; i < 20; i++)
        {
            alpha -= 0.05f;
            yield return new WaitForSeconds(0.01f); 
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
        yield return new WaitForSeconds(0.1f);

        GameObject.Find("GameCore").GetComponent<GameCore>().playerControl.GetOneSoul(gameObject.transform.position);
        Destroy(gameObject);
    }


}
