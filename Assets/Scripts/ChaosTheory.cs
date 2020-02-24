using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChaosTheory : MonoBehaviour
{
    //Changing this values make possible have different configurations
    //Changed from UI
    int n;
    float side;
    Figure figure;
    Vector2[] vertices;

    int counter;

    public GameObject prefabPoint;
    GameObject container;
    List<GameObject> points;
    Vector2 currentPositionPoint;

    float time, timer, proportion;

    public GameObject VertexUI,LengthUI,SpeedUI,ProportionUI,CounterUI;
    // Start is called before the first frame update
    void Start()
    {
        //Create a list of Object. It will use as a pool to avoid delete and create new objs every time configuration changes
        points = new List<GameObject>();
        container = new GameObject();
        container.name = "Points";
        //Called every time configuration changes
        Restart();
    }
    public void Restart()
    {
        if (container)
        {
            //Place all the point away from camera sight
            for (int i = 0; i < points.Count; i++)
            {
                points[i].transform.position = new Vector2(-999,-999);
            }
        }
        //Delete all the vertices points
        if (!(figure is null))
            figure.Clear();

        //Obtain data for the new configuration
        ChangeVertices(true);
        ChangeLength(true);
        ChangeSpeed();
        ChangeProportion(true);

        counter = 0;
        CounterUI.GetComponent<Text>().text = "Iteration: " + counter;
        figure = new Figure(n, side, prefabPoint);
        vertices = figure.GetVertices();

        //Start position is random choosen
        currentPositionPoint = new Vector2(Random.Range(-side, side), Random.Range(-side, side)) * 1.5f;
        //Instantiate/set position of the point at position:currentPositionPoint
        DrawPoint(currentPositionPoint);
    }
    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > timer)
        {
            time = 0;
            //Calculate new position as vectorial sum of the current position and a vector whose direction is toward the new point (scaled by proportion [0,1])
            Vector2 pos = currentPositionPoint + (getVertice()- currentPositionPoint) * proportion;
            //Update current position
            currentPositionPoint = pos;
            //Instantiate/set position of the new point
            DrawPoint(pos);
        }
    }

    Vector2 getVertice()
    {
        //Return a random vertices
        int index = Random.Range(0,n);
        return vertices[index];
    }
    void DrawPoint(Vector2 posVertex)
    {
        GameObject tmp;
        if (counter<points.Count)
        {
            //The pool is still full so just set the position the the point at index counter
            points[counter].transform.position = posVertex;
        }
        else
        {
            //The pool is empty. Create a new object and add it to the pool
            tmp = Instantiate(prefabPoint, posVertex, Quaternion.identity);
            tmp.name = "Point " + counter;
            tmp.transform.localScale = Vector3.one / 2;
            tmp.transform.SetParent(container.transform);
            points.Add(tmp);
        }
        //Update counter
        counter++;
        //Update UI
        CounterUI.GetComponent<Text>().text = "Iteration: " + counter;
    }

    //In the editor value will be false, it means that the changes are applied only after the button Restart is clicked
    public void ChangeVertices(bool value = false)
    {
        Text text = VertexUI.GetComponent<Text>();
        Slider slider = VertexUI.transform.GetChild(0).GetComponent<Slider>();
        if (value)
            n = (int)slider.value;
        text.text = "# Vertices: " + slider.value;
    }
    public void ChangeLength(bool value = false)
    {
        Text text = LengthUI.GetComponent<Text>();
        Slider slider = LengthUI.transform.GetChild(0).GetComponent<Slider>();
        if(value)
            side = (int)slider.value;
        text.text = "Length Side : " + slider.value;
    }
    //Speed does not have a bool value since it can be change runtime without recreate everything
    public void ChangeSpeed()
    {
        Text text = SpeedUI.GetComponent<Text>();
        Slider slider = SpeedUI.transform.GetChild(0).GetComponent<Slider>();
        timer = 2 - slider.value;
        text.text = "Speed: " + slider.value;
    }
    public void ChangeProportion(bool value = false)
    {
        Text text = ProportionUI.GetComponent<Text>();
        Slider slider = ProportionUI.transform.GetChild(0).GetComponent<Slider>();
        if(value)
            proportion = slider.value;
        text.text = "Proportion: " + ((int)(slider.value * 100))/100f;
    }
}
