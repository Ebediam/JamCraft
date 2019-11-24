using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Icon : MonoBehaviour
{
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textNumber;
    public Image sprite;
    public PlayerData data;
    public int position;



    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    void Start()
    { 
        UpdateIcon();

    }

    public void OnEnable()
    {
        UpdateIcon();
    }

    public void OnDisable()
    {
        //GameManager.InventoryEvent -= UpdateIcon;
    }

    // Update is called once per frame
    void UpdateIcon()    
    {

        if (data.storedPickups.Count > position)
        {
            textName.text = data.storedPickups[position].name;
            textNumber.text = data.amount[position].ToString();
            sprite.sprite = data.storedPickups[position].icon;
        }
        else
        {
            textName.text = "Empty";
            textNumber.text = "";
            sprite.sprite = null;
        }
    }
}
