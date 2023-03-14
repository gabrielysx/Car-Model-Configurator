using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollItems : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private Image image;
    [SerializeField] public ButtonManager BM;
    [SerializeField] private CanvasGroup CG;

    public int itemIndex;
    public int infoIndex;
    public string description;

    public RectTransform RT;

    private SelectScrollSys scrollSYS;
    private bool isDrag;

    private void OnEnable()
    {
        RT = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInfo(Sprite sp, string name, string description,int infoIndex, SelectScrollSys SSS)
    {  
        image.sprite = sp;
        BM.SetText(name);
        this.description = description;
        this.infoIndex = infoIndex;
        this.scrollSYS= SSS;

    }

    public void setAlpha(float alpha)
    {
        CG.alpha = alpha;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        scrollSYS.OnDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag= false;
        scrollSYS.OnPointerDown(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDrag)
        {
            //scrollSYS.Select(itemIndex, infoIndex, RT);
        }
        scrollSYS.OnPointerUp(eventData);
    }
}
