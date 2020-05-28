using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
    public Sprite Sprite;

    public int GridSize;

    public float[,] Grid;

    private int vertical, horizontal, columns, rows;

    private Vector3 leftSource = new Vector3(-100, -100);
    private Vector3 rightSource = new Vector3(-100, -100);
    private float waveLenght = 1;


    // Use this for initialization
    void Start()
    {
        leftSource = new Vector3(-waveLenght, 0);
        rightSource = new Vector3(waveLenght, 0);
        CreateGrid();
    }

    private void CreateGrid()
    {
        vertical = (int)(Camera.main.orthographicSize * Sprite.pixelsPerUnit);
        Debug.Log("vertical = (int)Camera.main.orthographicSize\t\t has value: " + vertical.ToString());
        horizontal = vertical * (Screen.width / Screen.height);
        Debug.Log("horizontal = vertical * (Screen.width / Screen.height)\t\t has value: " + horizontal);

        rows = vertical * 2;
        columns = horizontal * 2;
        Debug.Log("Rows: " + rows + " Columns: " + columns);

        SpawnSourceTile((int)leftSource.x, (int)leftSource.y, 1f);
        SpawnSourceTile((int)rightSource.x, (int)rightSource.y, 1f);
        Grid = new float[columns, rows];
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                var position = new Vector3((i - horizontal) / Sprite.pixelsPerUnit, (j + 1 - vertical) / Sprite.pixelsPerUnit);
                Grid[i, j] = CalculateInterference(position);
                SpawnTile(i, j, position, Grid[i, j]);
            }
        }

    }
    private void SpawnTile(int x, int y, Vector3 position, float value)
    {
        var g = new GameObject();
        g.transform.position = position;//new Vector3((x - horizontal)/Sprite.pixelsPerUnit,(y + 1 - vertical) / Sprite.pixelsPerUnit);
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = Sprite;
        s.color = new Color(value, value, value);
    }
    private void SpawnSourceTile(int x, int y, float value = 1)
    {
        var g = new GameObject();
        g.transform.position = new Vector3((x) / Sprite.pixelsPerUnit, (y + 1) / Sprite.pixelsPerUnit);
        var s = g.AddComponent<SpriteRenderer>();
        s.sprite = Sprite;
        s.color = new Color(value, value, value);
    }

    private float CalculateInterference(Vector3 pointCoordinates)
    {
        var distLeft = Vector3.Distance(leftSource, pointCoordinates);
        var distRight = Vector3.Distance(rightSource, pointCoordinates);
        var sum = distLeft + distRight;
        var percentage = Math.Abs(((sum - ((int)( sum / waveLenght)) * waveLenght)- 0.5f)) * 2;
        return percentage;
    }

}