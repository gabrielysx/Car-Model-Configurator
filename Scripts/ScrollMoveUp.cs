using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMoveUp : MonoBehaviour
{   public RectTransform RT;
    public float bottomY = -930f;
    public float topY = -400f;
    public bool startMove = false;
    public bool isMoving = false;
    public float moving_time, cur_time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startMove)
        {
            cur_time = 0;
            moving_time= 0;
            RT.anchoredPosition3D = new Vector3(0, bottomY, 0);
            startMove = false;
            isMoving = true;
        }
        else
        {

        }
    }
}
