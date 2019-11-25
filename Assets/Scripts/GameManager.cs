using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public delegate void PauseDelegate();
    public static PauseDelegate PauseEvent;

    public delegate void InventoryDelegate();
    public static InventoryDelegate InventoryEvent;

    public delegate void CraftDelegate();
    public static CraftDelegate CraftEvent;
    public static CraftDelegate EndCraftEvent;

    public List<SpawnPoint> spawnPoints;
    public Player player;
    public GameObject menuCanvas;
    public CraftManager craftManager; 
    public PlayerInputs inputs;
    public static bool isPaused;

    public static bool isCrafting;

    public AudioSource pickupSFX;
            
    private void Awake()
    {
        inputs = new PlayerInputs();

        inputs.GamePlay.Pause.performed += PauseGame;

        InventoryEvent += PlayPickUPSFX;
        CraftEvent += ActivateCraftManager;
        EndCraftEvent += DeactivateCraftManager;


    }

    // Start is called before the first frame update
    void Start()
    {
        menuCanvas.SetActive(false);

        foreach(SpawnPoint spawnPoint in spawnPoints)
        {
            if(player.playerData.activeLevel == spawnPoint.sceneYouComeFrom)
            {
                player.transform.position = spawnPoint.transform.position;
                player.transform.rotation = spawnPoint.transform.rotation;
                break;
            }
        }

        player.playerData.activeLevel = SceneManager.GetActiveScene().buildIndex;
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }


    public static void GiveItemToPlayer(Player player, PickupData item)
    {
        if (player.playerData.storedPickups.Count == 0)
        {
            item.amount++;
            player.playerData.storedPickups.Add(item);
            
            
        }
        else
        {
            int i = 0;
            bool alreadyHadPickup = false;
            foreach (PickupData storedPickupData in player.playerData.storedPickups)
            {
                if (storedPickupData == item)
                {
                    player.playerData.storedPickups[i].amount++;
                    alreadyHadPickup = true;
                    break;
                }
                i++;
            }

            if (!alreadyHadPickup)
            {
                item.amount++;
                player.playerData.storedPickups.Add(item);
                
            }
        }

        if (item.showsInPlayer)
        {
            if (!player.isHandling)
            {
                GameObject prefab = Instantiate(item.prefab);
                prefab.transform.position = player.objectHolder.transform.position;
                prefab.transform.rotation = player.objectHolder.transform.rotation;
                prefab.transform.parent = player.objectHolder.transform;
                player.isHandling = true;
            }
        }

        InventoryEvent?.Invoke();
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (isCrafting)
        {

        }
        else
        {

            if (isPaused) //the game was paused
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isPaused = false;
                menuCanvas.SetActive(false);
                Time.timeScale = 1f;
            }
            else //the game was running
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isPaused = true;
                menuCanvas.SetActive(true);
                Time.timeScale = 0f;
            }
        }

    }

    public void PlayPickUPSFX()
    {
        pickupSFX.Play();
    }

    public void ActivateCraftManager()
    {
        craftManager.gameObject.SetActive(true);
    }

    public void DeactivateCraftManager()
    {
        craftManager.gameObject.SetActive(false);
    }



}
