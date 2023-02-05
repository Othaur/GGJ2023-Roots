using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayTile : MonoBehaviour
{
    [SerializeField] GameObject shown;
    [SerializeField] GameObject hidden;

    bool visible = false; 
    // Start is called before the first frame update
    void Start()
    {
        
        //shown.SetActive(false);
        //hidden.SetActive(true);
    }

    public bool IsVisible()
    {
        return visible;
    }

    public void Show(bool state)
    {
        Debug.Log("Setting to " + state);
        shown.SetActive(state);
        hidden.SetActive(!state);
        visible = state;
    }
}
