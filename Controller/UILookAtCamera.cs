using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 将场景的UI面向相机，绑定在场景中的UI上
/// </summary>
public class UILookAtCamera : MonoBehaviour {

    [SerializeField]
    Transform camera_transform;

    [SerializeField]
    private GameObject viewCard;

    private Vector3 original_Euler;

    [SerializeField]
    bool LookAt;

	// Use this for initialization
	void Awake () {

        original_Euler = transform.localEulerAngles;
	}
	
	// Update is called once per frame
	void Update () {

        if (LookAt && viewCard.activeInHierarchy)
        {
            if (camera_transform.gameObject.activeInHierarchy)
                transform.LookAt(camera_transform);
        }
        else
        {
            transform.localEulerAngles = original_Euler;
        }
	}
}
