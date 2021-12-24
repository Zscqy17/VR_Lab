using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DevelopEngine;

public class WaitForManager : MonoSingleton<WaitForManager> {
    public void WaitForSecondsAndDo(float time, Action action=null)
    {
        StopAllCoroutines();
        StartCoroutine(WaitCoroutine(time,action));
    }
    IEnumerator WaitCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
        Debug.Log("wait coroutine end");
    }
    
}
