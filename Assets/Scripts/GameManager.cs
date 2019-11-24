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

    public List<SpawnPoint> spawnPoints;
    public Player player;
    public GameObject canvas;
    public PlayerInputs inputs;
    public static bool isPaused;

    private void Awake()
    {
        inputs = new PlayerInputs();

        inputs.GamePlay.Pause.performed += PauseGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);

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
            player.playerData.storedPickups.Add(item);
            player.playerData.amount.Add(1);
        }
        else
        {
            int i = 0;
            bool alreadyHadPickup = false;
            foreach (PickupData storedPickupData in player.playerData.storedPickups)
            {
                if (storedPickupData == item)
                {
                    player.playerData.amount[i]++;
                    alreadyHadPickup = true;
                    break;
                }
                i++;
            }

            if (!alreadyHadPickup)
            {
                player.playerData.storedPickups.Add(item);
                player.playerData.amount.Add(1);
            }
        }

        InventoryEvent?.Invoke();
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (isPaused) //the game was paused
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isPaused = false;
            canvas.SetActive(false);
            Time.timeScale = 1f;
        }
        else //the game was running
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isPaused = true;
            canvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }




}
