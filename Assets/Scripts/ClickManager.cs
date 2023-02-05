using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickManager : MonoBehaviour
{
    Camera cam;
    

    public UnityEvent<Vector3> OnMouseClick;
    

    public HexGrid hexGrid;
    public AudioClip rootGrowth;
    AudioSource audio;

    
    void Awake()
    {
        cam = Camera.main;
        hexGrid = FindObjectOfType<HexGrid>();
        audio = GetComponent<AudioSource>();
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

                if (hexGrid.CanGrow(mousePosition))
                {
                    PlayRootSound();           
                    OnMouseClick?.Invoke(gridCenter);
                }
            }
            else
            {
                PlayRootSound();
                OnMouseClick?.Invoke(mousePosition);
            }
        }
    }

    void PlayRootSound()
    {
        if (audio != null && rootGrowth != null)
        {
            audio.clip = rootGrowth;
            audio.Play();
        }
    }

}
