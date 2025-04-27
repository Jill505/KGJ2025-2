using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    static public bool CameraTracking = false;

    public bool cameraTrackinForTest = false;
    public GameObject player;
    public Transform target;
    public float fulence = 0.16f;
    bool keepTimeCal = false;
    int randomDir
    {
        get
        {
            int random = Random.Range(-1, 1);
            if (random == 0)
            {
                random = 1;
            }
            return random;
        }
    }
    void Start()
    {
  
        target = player.GetComponent<Transform>();
    }
    void FixedUpdate()
    {
        if (CameraTrack.CameraTracking == true || cameraTrackinForTest)
        {
            if (target != null)
            {
                if (transform.position != target.position)
                {
                    Vector3 targetpos = target.position;
                    //transform.position = Vector3.Lerp(transform.position, targetpos, fulence);
                    transform.position = Vector3.LerpUnclamped(transform.position, targetpos, fulence);    
                }
            }
        }
    }

    public void shakeScreen(float force, float time)//次數為0.025秒一次 也就是40幀
    {
        StartCoroutine(shake(force));
        StartCoroutine(keepTimeCalculate(time));
    }
    public void pauseTimer(float scale, float time)
    {
        StartCoroutine(pauseTime(scale, time));
    }
    IEnumerator shake(float force)//這個數字建議小一點比較好
    {
        int xdir = randomDir;
        int ydir = randomDir;
        while (!keepTimeCal)
        {
            gameObject.transform.position = new Vector3(xdir * force, ydir * force, 0f);
            yield return new WaitForSeconds(0.01f);
            gameObject.transform.position = new Vector3(xdir * force * -1f, ydir * force * -1f, 0f);
            yield return new WaitForSeconds(0.01f);
            xdir = randomDir;
            ydir = randomDir;
        }
        keepTimeCal = false;
        yield return 0;
    }
    IEnumerator keepTimeCalculate(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        keepTimeCal = true;
        yield return 0;
    }
    IEnumerator pauseTime(float scale, float time)
    {
        if (scale > 0f)
        {
            scale = 1f;
        }
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(time);
        Time.timeScale = 1f;
        yield return 0;
    }
}
