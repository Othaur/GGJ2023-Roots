using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickManager : MonoBehaviour
{
    Camera cam;
    

    public UnityEvent<Vector3> OnMouseClick;
    

    public HexGrid hexGrid;

    
    void Awake()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 gridCenter;
            Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
            if (hexGrid != null)
            {
                gridCenter = hexGrid.GetCellCenter(mousePosition);
                OnMouseClick?.Invoke(gridCenter);
            }
            else
            {
                OnMouseClick?.Invoke(mousePosition);
            }
        }
    }
}
