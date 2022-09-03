using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFun : MonoBehaviour
{
  PosAnim posAnim;
  private void Start()
  {
    posAnim = GameObject.Find("Canvas").GetComponent<PosAnim>();
  }
  public void AnimOver_1()
  {
    if (posAnim.operationAnim[posAnim.animationClip.name].speed == 1)
      StartCoroutine("OverTime");
  }
  public void AnimOver_0()
  {
    if (posAnim.operationAnim[posAnim.animationClip.name].speed == -1)
      StartCoroutine("OverTime");
  }
  IEnumerator OverTime()
  {
    yield return new WaitForSeconds(0.3f);
    posAnim.animationClip = null;
    posAnim.AnimOver();
  }
}
