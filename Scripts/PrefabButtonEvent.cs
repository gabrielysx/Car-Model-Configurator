using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabButtonEvent : MonoBehaviour
{
    public int button_index;
    public DataManager data_manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void button_click()
    {
        data_manager.callout_mod_menu(button_index);
    }

}
