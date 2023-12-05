using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Cinemachine;

[Serializable]
class PlayerData {
    public int hitPoint;
    public int hitPointMax;
    public float blockPoint;
    public float blockPointMax;
    public int baseDamage;
    public int baseDefense;
    public int gold;
    public int[] inventoryIDs;
    public int[] inventoryQuantities;
    public int[] equipmentIDs;
    public int equippedSwordID;
    public int equippedBowID;
    public int equippedShieldID;
    public float positionX;
    public float positionY;
    public string sceneToLoad;
    public List<string> interactableNames = new List<string>();
    public List<bool> interactableStates = new List<bool>();
}

public class GameManager : MonoBehaviour
{
    // Instance
    public static GameManager instance;

    // References
    private Player player;
    private FloatingTextManager floatingTextManager;
    private UIManager uiManager;
    private InventoryManager inventoryManager;
    private DialogueManager dialogueManager;
    private PersistenceManager persistenceManager;
    private CinemachineVirtualCamera virtualCamera;

    // Data
    //private int hitPoint;
    //private int hitPointMax;
    //private float blockPoint;
    //private float blockPointMax;
    //private int baseDamage;
    //private int baseDefense;
    //private int gold;
    private int[] inventoryIDs;
    private int[] inventoryQuantities;
    private int[] equipmentIDs;
    //private int equippedSwordID;
    //private int equippedBowID;
    //private int equippedShieldID;
    //private float positionX;
    //private float positionY;
    //private string sceneToLoad;
    //private List<string> interactableNames = new List<string>();
    //private List<bool> interactableStates = new List<bool>();

    private string filePath;

    private void Awake() {
        if (GameManager.instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        PlayerPrefs.DeleteAll();

        SceneManager.sceneLoaded += GetReferences;
        SceneManager.sceneLoaded += FindInteractables;
        SceneManager.sceneLoaded += LoadGameState;
        DontDestroyOnLoad(gameObject);

        filePath = Application.persistentDataPath + "/playerInfo.dat";
    }

    private void GetReferences(Scene scene, LoadSceneMode mode) {
        player = GameObject.Find("Player").GetComponent<Player>();
        floatingTextManager = GameObject.Find("FloatingTextManager").GetComponent<FloatingTextManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        inventoryManager = GameObject.Find("MenuCanvas").GetComponent<InventoryManager>();
        dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
        persistenceManager = GameObject.Find("PersistenceManager").GetComponent<PersistenceManager>();
        virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
        virtualCamera.Follow = player.transform;
    }

    public void SetNewScene(string sceneName, float posX, float posY) {
        sceneToLoad = sceneName;
        positionX = posX;
        positionY = posY;
    }

    public GameObject GetPlayer() {
        return player.gameObject;
    }

    public void SaveGameData() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        PlayerData data = new PlayerData();

        data.hitPoint = player.hitPoint;
        data.hitPointMax = player.hitPointMax;
        data.blockPoint = player.blockPoint;
        data.blockPointMax = player.blockPointMax;
        data.baseDamage = player.baseDamage;
        data.baseDefense = player.baseDefense;
        data.gold = player.gold;
        data.positionX = player.transform.position.x;
        data.positionY = player.transform.position.y;
        data.sceneToLoad = sceneToLoad;
        data.interactableNames = persistenceManager.interactableNames;
        data.interactableStates = persistenceManager.interactableStates;

        data.equippedSwordID = player.GetEquipment(EquipType.sword).equipID;
        data.equippedBowID = player.GetEquipment(EquipType.bow).equipID;
        data.equippedShieldID = player.GetEquipment(EquipType.shield).equipID;

        inventoryIDs = new int[inventoryManager.itemSlots.Length];
        inventoryQuantities = new int[inventoryManager.itemSlots.Length];
        equipmentIDs = new int[inventoryManager.equipSlots.Length];

        for (int i = 0; i < inventoryManager.itemSlots.Length; i++) {
            if (inventoryManager.itemSlots[i].isFull) {
                inventoryIDs[i] = inventoryManager.itemSlots[i].itemID;
                inventoryQuantities[i] = inventoryManager.itemSlots[i].quantity;
            }
        }

        for (int i = 0; i < inventoryManager.equipSlots.Length; i++) {
            if (inventoryManager.equipSlots[i].isFull) {
                equipmentIDs[i] = inventoryManager.equipSlots[i].itemID;
            }
        }

        data.inventoryIDs = inventoryIDs;
        data.inventoryQuantities = inventoryQuantities;
        data.equipmentIDs = equipmentIDs;

        bf.Serialize(file, data);
        file.Close();
        
        /*
        string gameData = "";

        gameData += sceneToLoad + "|";
        gameData += positionX.ToString() + "|";
        gameData += positionY.ToString() + "|";
        gameData += player.hitPoint.ToString() + "|";
        gameData += player.hitPointMax.ToString() + "|";
        gameData += player.gold.ToString();

        PlayerPrefs.SetString("GameData", gameData);
        
        */
        Debug.Log("Game data saved.");
        
    }

    public void LoadGameData() {
        if (File.Exists(filePath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);

            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            player.hitPoint = data.hitPoint;
            player.hitPointMax = data.hitPointMax;
            player.blockPoint = data.blockPoint;
            player.blockPointMax = data.blockPointMax;
            player.baseDamage = data.baseDamage;
            player.baseDefense = data.baseDefense;
            player.gold = data.gold;
            player.transform.position = new Vector3(data.positionX, data.positionY, 0);
            sceneToLoad = data.sceneToLoad;
            persistenceManager.interactableNames = data.interactableNames;
            persistenceManager.interactableStates = data.interactableStates;

            for (int i = 0; i < inventoryManager.itemSlots.Length; i++) {
                inventoryManager.itemSlots[i].EmptySlot();
            }

            for (int i = 0; i < inventoryManager.equipSlots.Length; i++) {
                inventoryManager.equipSlots[i].EmptySlot();
            }

            // Player Inventory
            for (int i = 0; i < data.inventoryIDs.Length; i++) {
                for (int j = 0; j < inventoryManager.itemDatabase.Count; j++) {
                    if (data.inventoryIDs[i] == inventoryManager.itemDatabase[j].itemID) {
                        AddItem(inventoryManager.itemDatabase[j].itemID, inventoryManager.itemDatabase[j].itemName, data.inventoryQuantities[i],
                            inventoryManager.itemDatabase[j].itemSprite, inventoryManager.itemDatabase[j].itemDescription, inventoryManager.itemDatabase[j].itemType);
                        break;
                    }
                }
            }

            // Player Equipment
            for (int i = 0; i < data.equipmentIDs.Length; i++) {
                for (int j = 0; j < inventoryManager.equipDatabase.Count; j++) {
                    if (data.equipmentIDs[i] == inventoryManager.equipDatabase[j].itemID) {
                        AddItem(inventoryManager.equipDatabase[j].itemID, inventoryManager.equipDatabase[j].itemName, inventoryManager.equipDatabase[j].itemQuantity,
                            inventoryManager.equipDatabase[j].itemSprite, inventoryManager.equipDatabase[j].itemDescription, inventoryManager.equipDatabase[j].itemType);
                        break;
                    }
                }
            }

            // Player Equipped Weapons
            for (int i = 0; i < inventoryManager.scriptableEquips.Count; i++) {
                if (inventoryManager.scriptableEquips[i].equipID == data.equippedSwordID) {
                    player.SetEquipment(inventoryManager.scriptableEquips[i]);
                    Debug.Log(player.equippedSword.name);
                    for (int j = 0; j < inventoryManager.equipSlots.Length; j++) {
                        if (inventoryManager.equipSlots[j].itemID == data.equippedSwordID) {
                            inventoryManager.equipSlots[j].quantityText.text = "E";
                            Debug.Log(inventoryManager.equipSlots[j].gameObject.name + ": E");
                            inventoryManager.equipSlots[j].quantityText.enabled = true;
                            Debug.Log(inventoryManager.equipSlots[j].gameObject.name + " text enabled.");
                            break;
                        }
                    }
                } else if (inventoryManager.scriptableEquips[i].equipID == data.equippedBowID) {
                    player.SetEquipment(inventoryManager.scriptableEquips[i]);
                    Debug.Log(player.equippedBow.name);
                    for (int j = 0; j < inventoryManager.equipSlots.Length; j++) {
                        if (inventoryManager.equipSlots[j].itemID == data.equippedBowID) {
                            inventoryManager.equipSlots[j].quantityText.text = "E";
                            Debug.Log(inventoryManager.equipSlots[j].gameObject.name + ": E");
                            inventoryManager.equipSlots[j].quantityText.enabled = true;
                            Debug.Log(inventoryManager.equipSlots[j].gameObject.name + " text enabled.");
                            break;
                        }
                    }
                } else if (inventoryManager.scriptableEquips[i].equipID == data.equippedShieldID) {
                    player.SetEquipment(inventoryManager.scriptableEquips[i]);
                    Debug.Log(player.equippedShield.name);
                    for (int j = 0; j < inventoryManager.equipSlots.Length; j++) {
                        if (inventoryManager.equipSlots[j].itemID == data.equippedShieldID) {
                            inventoryManager.equipSlots[j].quantityText.text = "E";
                            Debug.Log(inventoryManager.equipSlots[j].gameObject.name + ": E");
                            inventoryManager.equipSlots[j].quantityText.enabled = true;
                            Debug.Log(inventoryManager.equipSlots[j].gameObject.name + " text enabled.");
                            break;
                        }
                    }
                }
                //break;
            }
        }

        /*
        if (!PlayerPrefs.HasKey("GameData"))
            return;

        string[] gameData = PlayerPrefs.GetString("GameData").Split('|');
        string gameState = "";

        gameState += gameData[0] + "|";
        gameState += gameData[1] + "|";
        gameState += gameData[2] + "|";
        gameState += gameData[3] + "|";
        gameState += gameData[4] + "|";
        gameState += gameData[5];

        PlayerPrefs.SetString("GameState", gameState);
        
        */
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        Debug.Log("Game data loaded.");
    }

    // Game State
    public void SaveGameState() {
        string gameState = "";

        gameState += sceneToLoad + "|";
        gameState += positionX.ToString() + "|";
        gameState += positionY.ToString() + "|";
        /*
        gameState += player.hitPoint.ToString() + "|";
        gameState += player.hitPointMax.ToString() + "|";
        gameState += player.gold.ToString();
        */

        PlayerPrefs.SetString("GameState", gameState);
        Debug.Log("Game state saved.");
    }

    public void LoadGameState(Scene scene, LoadSceneMode mode) {
        if (!PlayerPrefs.HasKey("GameState"))
            return;

        string[] gameStateData = PlayerPrefs.GetString("GameState").Split('|');
        
        if (SceneManager.GetActiveScene().name == gameStateData[0])
            player.transform.position = new Vector3(float.Parse(gameStateData[1]), float.Parse(gameStateData[2]), 0f);

        /*
        player.hitPoint = int.Parse(gameStateData[3]);
        player.hitPointMax = int.Parse(gameStateData[4]);
        player.gold = int.Parse(gameStateData[5]);
        */

        UpdateHealthBar(player.hitPoint, player.hitPointMax);
        UpdateBlockBar(player.blockPoint, player.blockPointMax);
        Debug.Log("Game state loaded.");
    }

    // Floating Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        floatingTextManager.ShowText(msg, fontSize, color, position, motion, duration);
    }

    // Debug UI
    public void UpdateDebugUI(string txt) {
        uiManager.UpdateDebugUI(txt);
    }

    // Health Bar
    public void UpdateHealthBar(float hitPoint, float hitPointMax) {
        uiManager.UpdateHealthBar(hitPoint, hitPointMax);
    }

    // Block Bar
    public void UpdateBlockBar(float blockPoint, float blockPointMax) {
        uiManager.UpdateBlockBar(blockPoint, blockPointMax);
    }

    // Inventory
    public bool AddItem(int itemID, string itemName, int itemQuantity, Sprite itemSprite, string itemDescription, ItemType itemType) {
        bool canAdd = inventoryManager.AddItem(itemID, itemName, itemQuantity, itemSprite, itemDescription, itemType);
        return canAdd;
    }

    public bool UseItem(int itemID) {
        bool usable = inventoryManager.UseItem(itemID);
        UpdateHealthBar(player.hitPoint, player.hitPointMax);
        return usable;
    }

    public bool EquipItem(int itemID) {
        bool equippable = inventoryManager.EquipItem(itemID);
        return equippable;
    }

    public bool CheckInventory(ItemType itemType, int itemID) {
        bool hasItem = inventoryManager.CheckInventory(itemType, itemID);
        return hasItem;
    }

    public void RemoveItem(int itemID) {
        inventoryManager.RemoveItem(itemID);
    }

    public void UpdateStatsUI(float hitPoint, float hitPointMax, float blockPoint, float blockPointMax, int swordAtk, int bowAtk, int shieldDef) {
        uiManager.UpdateStatsUI(hitPoint, hitPointMax, blockPoint, blockPointMax, swordAtk, bowAtk, shieldDef);
    }

    public void UpdateSingleStat(string statName, float statValue, float statMax) {
        uiManager.UpdateSingleStat(statName, statValue, statMax);
    }

    public void InitiateDialogue(DialogueSO conversation, NPC npc) {
        dialogueManager.InitiateDialogue(conversation, npc);
    }

    public void FindInteractables(Scene scene, LoadSceneMode mode) {
        persistenceManager.FindInteractables(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void SetInteractableState(string name, bool state) {
        persistenceManager.SetInteractableState(name, state);
    }

    public void Shop(string shopType) {
        inventoryManager.Shop(shopType);
    }
}

