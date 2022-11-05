using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public MeshRenderer[] obstacleRenderers13;
    public MeshRenderer[] obstacleRenderers14;
    public MeshRenderer[] obstacleRenderers15;
    public GameObject sondol;
    void Update()

    {
        float Distance = Vector3.Distance(transform.position, sondol.transform.position);
        Vector3 Direction = (sondol.transform.position - transform.position).normalized;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Direction, out hit, Distance))
        {
            if (hit.transform.gameObject.layer.ToString().Equals("11"))
                for (int i = 0; i < obstacleRenderers13.Length; i++)
                {
                    Material Mat = obstacleRenderers13[i].material;
                    Color matColor = Mat.color;
                    matColor.a = 0.5f;
                    Mat.color = matColor;
                }
            else
            {
                for (int i = 0; i < obstacleRenderers13.Length; i++)
                {
                    Material Mat = obstacleRenderers13[i].material;
                    Color matColor = Mat.color;
                    matColor.a = 1f;
                    Mat.color = matColor;
                }
            }
            if (hit.transform.gameObject.layer.ToString().Equals("12"))
                for (int i = 0; i < obstacleRenderers14.Length; i++)
                {
                    Material Mat = obstacleRenderers14[i].material;
                    Color matColor = Mat.color;
                    matColor.a = 0.5f;
                    Mat.color = matColor;
                }
            else
            {
                for (int i = 0; i < obstacleRenderers14.Length; i++)
                {
                    Material Mat = obstacleRenderers14[i].material;
                    Color matColor = Mat.color;
                    matColor.a = 1f;
                    Mat.color = matColor;
                }
            }
            if (hit.transform.gameObject.layer.ToString().Equals("13"))
                for (int i = 0; i < obstacleRenderers15.Length; i++)
                {
                    Material Mat = obstacleRenderers15[i].material;
                    Color matColor = Mat.color;
                    matColor.a = 0.5f;
                    Mat.color = matColor;
                }
            else
            {
                for (int i = 0; i < obstacleRenderers15.Length; i++)
                {
                    Material Mat = obstacleRenderers15[i].material;
                    Color matColor = Mat.color;
                    matColor.a = 1f;
                    Mat.color = matColor;
                }
            }
        }
    }
}