using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIFunctions : MonoBehaviour
{
    ReadInput readInput;
    PosAnim posAnim;
    float time;
    public Text speedText;
    public float animSpeed;
    [HideInInspector]
    public bool isPuase = false;
    public bool isStart = false;
    public GameObject popup;
    // Start is called before the first frame update
    void Start()
    {
        speedText.text = "SPEED: " + Time.timeScale;

        if (readInput == null)
            readInput = this.GetComponent<ReadInput>();
        if (posAnim == null)
            posAnim = this.GetComponent<PosAnim>();
    }

    //start
    public void StartOpentron()
    {
        // posAnim._isOver = false;
        // readInput.Run();
        posAnim.index_New = 0;
        posAnim._father = -1;
        posAnim._isMove = true;
        posAnim.isMove = true;
        if (isStart)
        {
            OverOpentron();
        }
        else
        {
            isStart = true;
            posAnim.Operat(posAnim.index_New);
        }
    }

    //pause
    public void PauseOpentron()
    {
        if (posAnim.animationClip != null)
        {
            animSpeed = posAnim.operationAnim[posAnim.animationClip.name].speed;
            posAnim.operationAnim[posAnim.animationClip.name].speed = 0;
        }
        List<Tween> tweens = DOTween.PlayingTweens();
        if (tweens != null)
        {
            Debug.Log("num of animations: " + tweens.Count);
            for (int i = 0; i < tweens.Count; i++)
            {
                tweens[i].Pause();
            }
        }
        isPuase = false;
    }

    //continue
    public void ContinueOpentron()
    {
        if (posAnim.animationClip != null)
        {
            posAnim.operationAnim[posAnim.animationClip.name].speed = animSpeed;
        }
        List<Tween> tweens = DOTween.PausedTweens();
        if (tweens != null)
        {
            Debug.Log("num of animation: " + tweens.Count);
            for (int i = 0; i < tweens.Count; i++)
            {
                tweens[i].Play();
            }
        }
        if (isPuase)
        {
            posAnim.Operat(posAnim.index_New);
        }
    }
    //finish
    public void OverOpentron()
    {
        ContinueOpentron();
        // posAnim._isOver = true;
    }

    //speed setting
    public void TimeSpeed()
    {
        if (Time.timeScale == 1)
            Time.timeScale = 2;
        else if (Time.timeScale == 2)
            Time.timeScale = 3;
        else if (Time.timeScale == 3)
            Time.timeScale = 1;
        time = Time.timeScale;
        speedText.text = "SPEED：" + time;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        //pre processing
#if UNITY_EDITOR    //under editing mode
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }


}
