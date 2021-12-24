using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGizmos : MonoBehaviour
{
    public GameObject[] renderCameras;



    public void OnDrawGizmos()
    {
        if (renderCameras[0] == null || renderCameras[1] == null || renderCameras[2] == null || renderCameras[3] == null)
        {
            return;
        }
        Vector3 position = (renderCameras[0].transform.position + renderCameras[1].transform.position) / 2;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(renderCameras[0].transform.position, 0.15f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(renderCameras[0].transform.position, 0.15f * 1.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(renderCameras[1].transform.position, 0.15f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(renderCameras[1].transform.position, 0.15f * 1.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(renderCameras[2].transform.position, 0.15f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(renderCameras[2].transform.position, 0.15f * 1.5f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(renderCameras[3].transform.position, 0.15f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(renderCameras[3].transform.position, 0.15f * 1.5f);
    }




}
