using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialButtonPrefabEvent : MonoBehaviour
{
    public ScrollItems SI;
    private DataManager DM;
    // Start is called before the first frame update
    void Start()
    {
        DM = DataManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void button_double_click()
    {
        DM.change_material(SI.infoIndex);
    }
}
