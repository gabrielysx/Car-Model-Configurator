using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectScrollSys : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler

{
	[SerializeField] private GameObject itemPrefab;
	[SerializeField] public RectTransform itemParent;
	[SerializeField] private Text descriptionText;
	[SerializeField] private ItemInfo[] itemInfos;
	[SerializeField] private int displayNumber;
	[SerializeField] private float itemSpace;
	[SerializeField] private float moveSmooth;
	[SerializeField] private float dragSpeed;
	[SerializeField] private float scaleMultiplying;
	[SerializeField] private float alphaMultiplying;

	public event Action<int> SelectAction;

	private ScrollItems[] items;
	private float displayWidth;
	private int offsetTimes;
	private bool isDrag;
	private int currentItemIndex;
	private float[] distances;
	private float selectItemX;

	private bool isSelectMove;
	private bool isSelected;

	[Serializable]
	private struct ItemInfo
	{
		public string name;
		public Sprite sprite;
		public string description;
		public ItemInfo(string name, Sprite sprite, string description)
		{
			this.name = name;
			this.sprite = sprite; ;
			this.description = description;
		}
	 };
	public void OnDrag(PointerEventData eventData)
	{
		isSelectMove = false;
		itemParent.localPosition = new Vector2(itemParent.localPosition.x +
			eventData.delta.x * dragSpeed, itemParent.localPosition.y);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		isDrag = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isDrag = false;
	}

    private void Start()
    {
        int num = 10;
        string[] names = new string[num];
        Sprite[] sprites = new Sprite[num];
        string[] descriptions = new string[num];
        for (int i = 0; i < num; i++)
        {
            names[i] = (i + 1).ToString();
            sprites[i] = null;
            descriptions[i] = "test:" + (i + 1).ToString();
        }
        SetItemInfo(names, sprites, descriptions);
        Init();
        MoveItems(0);
    }

    private void OnEnable()
    {
        MoveItems(offsetTimes);
    }

	// Update is called once per frame
	void Update()
	{
		if (!isDrag)
		{
			Absorption();
		}
		int currentOffsetTimes = Mathf.FloorToInt(itemParent.localPosition.x / itemSpace);
		if(currentOffsetTimes != offsetTimes)
		{
			offsetTimes = currentOffsetTimes;
			MoveItems(offsetTimes);
		}
		ItemsControl();
	}

	private void Init()
	{
		displayWidth = (displayNumber - 1) * itemSpace;
		items = new ScrollItems[displayNumber];
		for (int i = 0; i < displayNumber; i++)
		{
			ScrollItems item = Instantiate(itemPrefab,itemParent).GetComponent<ScrollItems>();
			item.itemIndex= i;
			items[i] = item;
		}
		gameObject.SetActive(false);
	}

	public void SetItemInfo(string[] names, Sprite[] sprites, string[] descriptions)
	{
		if(names.Length != sprites.Length || sprites.Length != descriptions.Length || 
			descriptions.Length != names.Length)
		{
			Debug.Log("insufficient data");
			return;
		}
		itemInfos = new ItemInfo[names.Length];
		for(int i = 0;i<itemInfos.Length;i++)
		{
			itemInfos[i] = new ItemInfo(names[i], sprites[i], descriptions[i]); 
		}
		SelectAction = null;
		isSelected = false;
	}

	/*
	public void Select(int itemIndex, int infoIndex, RectTransform itemRT)
	{
		if(!isSelected && itemIndex == currentItemIndex)
		{
			SelectAction?.Invoke(infoIndex);
			isSelected= true;
			Debug.Log("select "+(itemIndex+1).ToString());
		}
		else
		{
			isSelectMove = true;
			selectItemX = itemRT.localPosition.x;
		}
	}
	*/

	private void MoveItems(int offsetTimes)
	{
		for(int i = 0; i < displayNumber; i++)
		{
			float x = itemSpace * (i - offsetTimes) - displayWidth / 2;
			if (items[i].RT != null)
			{
				items[i].RT.localPosition = new Vector2(x, items[i].RT.localPosition.y);
			}
		}
		int middle;
		if (offsetTimes > 0)
		{
			middle = itemInfos.Length - offsetTimes % itemInfos.Length;
		}
		else
		{
			middle = -offsetTimes % itemInfos.Length;
		}

		int infoIndex = middle;

		for (int i = Mathf.FloorToInt(displayNumber / 2f); i < displayNumber; i++)
		{
			if(infoIndex >= itemInfos.Length)
			{
				infoIndex = 0;
			}
			items[i].SetInfo(itemInfos[infoIndex].sprite, itemInfos[infoIndex].name, 
				itemInfos[infoIndex].description, infoIndex, this);
			infoIndex++;
		}

		infoIndex = middle - 1;
		for (int i = Mathf.FloorToInt(displayNumber/2f) - 1; i >= 0; i--)
		{
			if (infoIndex <= -1)
			{
				infoIndex = itemInfos.Length - 1;
			}
			items[i].SetInfo(itemInfos[infoIndex].sprite, itemInfos[infoIndex].name, 
				itemInfos[infoIndex].description, infoIndex, this);
			infoIndex--;
		}
	}

	private void Absorption()
	{
		float targetX;
		if (!isSelectMove)
		{
			float dis = itemParent.localPosition.x % itemSpace;
			int times = Mathf.FloorToInt(itemParent.localPosition.x / itemSpace);
			if (dis > 0)
			{
				if(dis < itemSpace / 2)
				{
					targetX = times * itemSpace;
				}
				else
				{
					targetX = (times +1) * itemSpace;
				}
			}
			else
			{
                if (dis < -itemSpace / 2)
                {
                    targetX = times * itemSpace;
                }
                else
                {
                    targetX = (times + 1) * itemSpace;
                }
            }
		}
		else
		{
			targetX = -selectItemX;
		}
		itemParent.localPosition = new Vector2(Mathf.Lerp(itemParent.localPosition.x, 
			targetX, moveSmooth / 10), itemParent.localPosition.y);
	}

	private void ItemsControl()
	{
		distances = new float[displayNumber];
		for (int i = 0; i<displayNumber; i++)
		{
			float dis = MathF.Abs(items[i].RT.position.x - transform.position.x);
			distances[i] = dis;
			float scale = 1 - dis * scaleMultiplying;
			items[i].RT.localScale = new Vector3(scale, scale, 1);
			items[i].setAlpha(1 - dis * alphaMultiplying);
		}
		float minDis = itemSpace * displayNumber;
		int minIndex = 0;
		for (int i = 0; i<displayNumber;i++)
		{
			if (distances[i] <minDis)
			{
				minDis = distances[i];
				minIndex = i;
			}
		}
		descriptionText.text = items[minIndex].description;
		currentItemIndex = items[minIndex].itemIndex;

	}
	public void clearItems()
	{

		int childCount = itemParent.transform.childCount;
		Debug.Log(childCount);
		for (int i = 0; i < childCount; i++)
		{
			Destroy(itemParent.transform.GetChild(0).gameObject);
		}
    }
}
