using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Interactable
{
    public bool onCooldown;
    public float cooldownTimer = 10f;
    // Start is called before the first frame update

    public SpawnerData spawnerData;

    private void Start()
    {
        disabled = true;
        textPrompt.text = spawnerData.verb + " " + spawnerData.name;
    }


    public override void OnTriggerEnter(Collider other)
    {
        if (onCooldown)
        {
            return;
        }

        if (other.GetComponent<Player>())
        {
            player = other.GetComponent<Player>();
            if (player.holdedObjectData == spawnerData.toolRequired)
            {
                disabled = false;
                
            }
        }
        base.OnTriggerEnter(other);

    }

    public override void InteractionStarts()
    {
        base.InteractionStarts();
        foreach(PickupData itemData in spawnerData.spawnableItems)
        {
            GameObject itemInstance = Instantiate(itemData.pickupPrefab, transform.position + Vector3.up, Quaternion.identity);
            itemInstance.GetComponentInChildren<Rigidbody>().AddForce(Vector3.up, ForceMode.Impulse);
        }

        onCooldown = true;
        Invoke("CooldownEnds", cooldownTimer);
        Player.UseToolEvent?.Invoke();

    }

    public void CooldownEnds()
    {
        onCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
