using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public struct Config_base
{
    public string type_name;
    public int part_number;
    public string[] gameobject_names;
    public int[] GO_material_index;
    public Material[] available_materials;
    public string[] material_names;
    public int[] material_prices;
}

[CreateAssetMenu(menuName = "MyConfig/Create CarConfig")]
public class CarConfig : ScriptableObject
{
    

    public GameObject car_prefab;
    public string car_name;
    public int car_base_price;
    public Config_base[] configs;


}
