using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    static DataManager _instance;
    public SelectScrollSys scroll_sys;
    public Sprite droplistsprite;
    public GameObject selection_drop_list,NameUI_text,PriceUI_text,car_anchor,mod_button_parent,mod_button_prefab;
    public float mod_button_spacing = 65f;
    public CarConfig[] configs;
    public CarConfig current_car_config;
    public string current_name;
    public int current_price;
    public int mod_number;
    public string[] modifiable_items_names;
    public int[] current_choice;
    public GameObject current_car;
    private bool menu_state = false;
    private int cur_menu_index;

    private void Awake()
    {
        _instance = this;
    }

    public static DataManager Instance
    {
        get
        {
            return _instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        LoadConfig(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Initialize()
    {
        if (car_anchor == null)
        {
            car_anchor = GameObject.Find("Car_anchor");
        }
        //set up selection dropdown list
        selection_drop_list.GetComponent<CustomDropdown>().items = new List<CustomDropdown.Item>(); 
        for (int i = 0; i< configs.Length; i++)
        {
            selection_drop_list.GetComponent<CustomDropdown>().CreateNewItem(configs[i].car_name, droplistsprite, true);
            //CustomDropdown.Item t = new CustomDropdown.Item { itemName = configs[i].name, itemIndex = i };
            //selection_drop_list.GetComponent<CustomDropdown>().items.Add(t);
        }
        selection_drop_list.GetComponent<CustomDropdown>().SetupDropdown();
        selection_drop_list.GetComponent<CustomDropdown>().UpdateItemLayout();
    }

    public void LoadConfig(int index)
    {
        scroll_sys.gameObject.SetActive(false);
        current_car_config = configs[index];
        if (current_car_config != null)
        {
            //clear exsisting car
            Transform TR = car_anchor.transform;
            int childCount = TR.childCount;
            for (int i = 0; i< childCount; i++)
            {
                Destroy(TR.GetChild(i).gameObject);
            }

            //Set up new car based on the config

            //Create Prefab
            current_car = Instantiate(current_car_config.car_prefab, TR);
            //Set car base price and name
            current_name = current_car_config.car_name;
            current_price = current_car_config.car_base_price;
            //Create car modifiable items list
            mod_number = current_car_config.configs.Length;
            modifiable_items_names = new string[mod_number];
            current_choice = new int[mod_number];
            //Clear button list
            TR = mod_button_parent.transform;
            childCount = TR.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(TR.GetChild(i).gameObject);
            }

            for (int t =0; t< mod_number; t++)
            {
                //Set modification names
                modifiable_items_names[t] = current_car_config.configs[t].type_name;
                //Set up button
                float space = (t + 1) * mod_button_spacing;
                GameObject tempButton = Instantiate(mod_button_prefab, mod_button_parent.transform);
                tempButton.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0f,-space,0f);
                tempButton.GetComponent<ButtonManager>().SetText(current_car_config.configs[t].type_name);

                //Set up button event
                tempButton.GetComponent<PrefabButtonEvent>().button_index = t;
                tempButton.GetComponent<PrefabButtonEvent>().data_manager = this;



                //Set materials to default (0)
                Set_material(current_car_config.configs[t], 0);
                //calculate price
                current_price += current_car_config.configs[t].material_prices[0];
            }
        }
        else
        {
            Debug.Log("No Available Config was found");
        }
        updateUIText();
    }

    public void Set_material(Config_base modded_part_config,int material_index)
    {
        int parts_num = modded_part_config.part_number;
        for(int i =0; i< parts_num;i++)
        {
            //Find the target game object that need to be changed material
            Transform temp = RecursiveSearchinChildren(current_car.transform,
                modded_part_config.gameobject_names[i]);
            if(temp != null)
            {
                //change the material
                int target_index = modded_part_config.GO_material_index[i];
                Material[] renderer_mat_list = temp.GetComponent<MeshRenderer>().materials;

                //rebuild the material list for the target
                Material[] t_mat_list = new Material[renderer_mat_list.Length];
                for (int j = 0;j < renderer_mat_list.Length; j++)
                {
                    //only change the needed one with the new material index
                    if (j == target_index)
                    {
                        t_mat_list[j] = modded_part_config.available_materials[material_index];
                    }
                    else
                    {
                        //other materials remain the same as before.
                        t_mat_list[j] = renderer_mat_list[j];
                    }
                }
                //override the old list
                temp.GetComponent<MeshRenderer>().materials = t_mat_list;

                Debug.Log("Set Material sucessful "+ modded_part_config.type_name+" "+temp);
                Debug.Log(renderer_mat_list[target_index].ToString());
            }
            else
            {
                Debug.Log("Cant find the target, skipped");
            }
            temp = null;
        }
    }

    public static Transform RecursiveSearchinChildren(Transform parent, string name)
    {
        Transform t = null;
        t =parent.Find(name);
        if(t== null)
        {
            foreach (Transform tran in parent)
            {
                t = RecursiveSearchinChildren(tran, name);
                if(t != null)
                {
                    return t;
                }
            }
        }
        return t;
    }

    public void callout_mod_menu(int index)
    {
        bool need_init = false;
        //repeat press button would close the menu
        if(menu_state == true)
        {
            scroll_sys.gameObject.SetActive(false);
            if (cur_menu_index == index)
            {
                menu_state = false;
                cur_menu_index = -1;
            }
            else
            {
                need_init = true;
            }

        }
        else
        {
            need_init= true;
        }
        //if press the new menu button 
        if (need_init)
        {
            //set up the menu of the target modification
            Config_base cc = current_car_config.configs[index];
            int num = cc.available_materials.Length;
            string[] names = new string[num];
            Sprite[] sprites = new Sprite[num];
            string[] descriptions = new string[num];
            for (int i = 0; i < num; i++)
            {
                names[i] = cc.material_names[i];
                sprites[i] = null;
                descriptions[i] = "Price: " + cc.material_prices[i].ToString() + "$";
            }

            scroll_sys.SetItemInfo(names, sprites, descriptions);

            scroll_sys.gameObject.SetActive(true);
            menu_state = true;
            cur_menu_index = index;
        }


    }

    public void change_material(int index)
    {
        int old_index = current_choice[cur_menu_index];
        //calculate new price
        //minus the old price
        current_price -= current_car_config.configs[cur_menu_index].material_prices[old_index];
        //set new choice
        current_choice[cur_menu_index] = index;
        //plus the new price
        current_price += current_car_config.configs[cur_menu_index].material_prices[index];

        //set new material
        Set_material(current_car_config.configs[cur_menu_index], index);

        //update price
        updateUIText();
    }

    public void updateUIText()
    {
        PriceUI_text.GetComponent<TMP_Text>().SetText("Current Price: " + current_price + "$");
        NameUI_text.GetComponent<TMP_Text>().SetText(current_name);
    }

    public void ChangeConfig()
    {
        int indx = selection_drop_list.GetComponent<CustomDropdown>().selectedItemIndex;
        LoadConfig(indx);
    }

}
