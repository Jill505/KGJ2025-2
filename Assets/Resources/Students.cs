using UnityEngine;
using UnityEngine.UI;

public class Students : MonoBehaviour
{
    public Eleable ele;
    [Header("move system")]
    public Vector2 oriPos;
    public Rigidbody2D rb2d;

    public bool moveable;
    public float moveSpeed = 1f;

    public float Strength =1;

    public float moveXArea = 3f;

    public bool rightMode  = true;
    public bool requireMoveAgain= false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ele = GetComponent<Eleable>();
        oriPos = transform.position;
        Strength = Random.Range(0.3f, 2);
        rb2d = GetComponent<Rigidbody2D>();
        moveSpeed = Random.Range(3.5f,6f);
        moveXArea = Random.Range(2f,4f);

        int Ran = Random.Range(0, 2);
        if (Ran == 0)
        {
            Ran = 1;
            rightMode = true;
        }
        else
        {
            Ran = -1;
            rightMode = false;
        }

        rb2d.linearVelocityX = moveSpeed * Ran;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveable)
        {
            if (requireMoveAgain)
            {
                requireMoveAgain = false;

                int Ran = Random.Range(0, 2);
                if (Ran == 0)
                {
                    Ran = 1;
                    rightMode = true;
                }
                else
                {
                    Ran = -1;
                    rightMode = false;
                }

                rb2d.linearVelocityX = moveSpeed * Ran;
            }

            if (rightMode)
            {

                if (transform.position.x > oriPos.x + moveXArea)
                {
                    //rev
                    rb2d.linearVelocityX = -moveSpeed;
                    rightMode = false;
                }
            }
            else
            {
                if (transform.position.x < oriPos.x - moveXArea)
                {
                    //rev
                    rb2d.linearVelocityX = moveSpeed;
                    rightMode = true;
                }

            }

        }
        else
        {
            rb2d.linearVelocity = Vector2.zero;
        }

    }
}
