using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure
{
    Vector2[] vertices;
    private int nSides;
    private float sideLength;
    float angle;
    GameObject point;
    GameObject container;

    public Figure (int nSides, float sideLength, GameObject prefab)
    {
        this.nSides = nSides;
        this.sideLength = sideLength;
        point = prefab;

        angle = (nSides - 2) * 180.0f / nSides;

        vertices = new Vector2[nSides];

        MakeShape();
    }

    void MakeShape()
    {
        container = new GameObject();
        container.name = "Figure";

        for (int i = 0; i < nSides; i++)
        {
            //'angle / 2 / 180.0f' is the angle fixing the orientation
            float v = (2.0f * i / nSides  - angle / 2 / 180.0f)* Mathf.PI;
            vertices[i] = new Vector2((Mathf.Cos(v)), (Mathf.Sin(v))) * sideLength;

            GameObject tmp = GameObject.Instantiate(point, vertices[i], Quaternion.identity);
            tmp.name = "Vertice: " + (i + 1);
            tmp.transform.localScale = Vector3.one * 2;
            tmp.GetComponent<SpriteRenderer>().color = Color.red;
            tmp.transform.SetParent(container.transform);
        }
    }

    public Vector2[] GetVertices()
    {
        return (Vector2[])vertices.Clone();
    }

    public void Clear()
    {
        //Since the number of vertices is low, just destroy and recreate them
        foreach (Transform i in container.transform)
        {
            GameObject.Destroy(i.gameObject);
        }
        GameObject.Destroy(container);
    }
}
