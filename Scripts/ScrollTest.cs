using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollTest : MonoBehaviour
{
    public SelectScrollSys scrollSys;
    private void Awake()
    {
        if(scrollSys != null)
        {
            int num = 100;
            string[] names = new string[num];
            Sprite[] sprites = new Sprite[num];
            string[] descriptions = new string[num];
            for (int i = 0; i < num; i++)
            {
                names[i] = (i+1).ToString();
                sprites[i] = null;
                descriptions[i] = "test:"+(i+1).ToString();
            }
            Debug.Log("names count: " + names.Length.ToString());
            Debug.Log("sprites count: " + sprites.Length.ToString());
            Debug.Log("des count: " + descriptions.Length.ToString());

            scrollSys.SetItemInfo(names, sprites, descriptions);

            scrollSys.SelectAction += (index) =>
            {
                print(index);
            };
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
