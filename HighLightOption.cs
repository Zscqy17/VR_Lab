using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightOption : MonoBehaviour
{
    [System.Serializable]
    public struct ActiveInfo
    {
        public int stateIndex;
        public GameObject upOption;
    }

    public GameObject chooser;

    public ActiveInfo[] infos;

    private GameObject upOption_current;

    private bool isActive()
    {
        foreach (var current in infos)
        {
            if(StateController.Instance.stateIndex == current.stateIndex)
            {
                upOption_current = current.upOption;
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == chooser && isActive())
        {
            transform.GetChild(0).gameObject.SetActive(true);
            upOption_current.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == chooser)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            upOption_current.SetActive(false);
        }
    }

    private void Update()
    {
        if (!chooser.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            if (upOption_current)
            {
                upOption_current.SetActive(false);
                upOption_current = null;
            }
           
        }
    }
}
