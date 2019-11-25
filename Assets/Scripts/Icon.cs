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
    public Button button;
    public PickupData pickupData;



    // Start is called before the first frame update
    private void Awake()
    {
        CraftManager.UpdateIconsEvent += UpdateIcon;
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
            if (!data.storedPickups[position])
            {
                ResetIcon();
                return;
            }

            if (data.storedPickups[position].amount == 0)
            {
                data.storedPickups.RemoveAt(position);                
                ResetIcon();
                return;
            }

            pickupData = data.storedPickups[position];
            textName.text = pickupData.name;
            textNumber.text = pickupData.amount.ToString();
            sprite.sprite = pickupData.icon;
            button.interactable = true;
        }
        else
        {
            ResetIcon();
        }
    }

    public  void ResetIcon()
    {
        pickupData = null;
        textName.text = "Empty";
        textNumber.text = "";
        sprite.sprite = null;
        button.interactable = false;
    }

    public void UpdateCraftElement(PickupData pickupData)
    {
        textName.text = pickupData.name;
        sprite.sprite = pickupData.icon;
    }

    public void OnClick() 
    {
        CraftManager.ItemSelectEvent?.Invoke(this);
    }
}
