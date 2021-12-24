using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChoose : MonoBehaviour
{
    public string optionTag;
    public string currentOption;
    public GameObject activeOption;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(optionTag))
        {
            currentOption = other.gameObject.name;
            activeOption.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(optionTag))
        {
            currentOption = null;
            activeOption.SetActive(false);

        }
    }

    private void OnDisable()
    {
        currentOption = null;
        activeOption.SetActive(false);
    }
}
