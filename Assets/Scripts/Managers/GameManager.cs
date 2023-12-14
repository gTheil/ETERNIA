using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Cinemachine;
using TMPro;

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
    public List<string> npcNames = new List<string>();
    public List<int> npcSequences = new List<int>();
}

public class GameManager : MonoBehaviour
{
    // Instance
    public static GameManager instance;

    // References
    private Player player;
    private FloatingTextManager floatingTextManager;
    private UIManager uiManager;
    private HUDManager hudManager;
    private InventoryManager inventoryManager;
    private DialogueManager dialogueManager;
    private PersistenceManager persistenceManager;
    private CinemachineVirtualCamera virtualCamera;

    // Data
    private int hitPoint;
    private int hitPointMax;
    private float blockPoint;
    private float blockPointMax;
    private int baseDamage;
    private int baseDefense;
    private int gold;
    private int[] inventoryIDs;
    private int[] inventoryQuantities;
    private int[] equipmentIDs;
    private int equippedSwordID;
    private int equippedBowID;
    private int equippedShieldID;
    private float positionX;
    private float positionY;
    private string sceneToLoad;
    private List<string> interactableNames = new List<string>();
    private List<bool> interactableStates = new List<bool>();
    private List<string> npcNames = new List<string>();
    private List<int> npcSequences = new List<int>();

    public string filePath;
    private bool newGame = true;

    private void Awake() {
        if (GameManager.instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        PlayerPrefs.DeleteAll();

        SceneManager.sceneLoaded += GetReferences;
        SceneManager.sceneLoaded += FindInteractables;
        SceneManager.sceneLoaded += FindNPCs;
        SceneManager.sceneLoaded += LoadGameState;
        DontDestroyOnLoad(gameObject);

        filePath = Application.persistentDataPath + "/playerInfo.dat";
    }

    private void GetReferences(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name != "MainMenuScene") {
            player = GameObject.Find("Player").GetComponent<Player>();
            floatingTextManager = GameObject.Find("FloatingTextManager").GetComponent<FloatingTextManager>();
            uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
            hudManager = GameObject.Find("HUDCanvas").GetComponent<HUDManager>();
            inventoryManager = GameObject.Find("MenuCanvas").GetComponent<InventoryManager>();
            dialogueManager = GameObject.Find("DialogueManager").GetComponent<DialogueManager>();
            persistenceManager = GameObject.Find("PersistenceManager").GetComponent<PersistenceManager>();
            virtualCamera = GameObject.Find("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
            if (virtualCamera != null)
                virtualCamera.Follow = player.transform;
            UpdateHealthBar(player.hitPoint, player.hitPointMax);
            UpdateBlockBar(player.blockPoint, player.blockPointMax);
        } else {
            if (player != null)
                Destroy(player.gameObject);
            if (hudManager != null)
                Destroy(hudManager.gameObject);
        }
    }

    public void SetNewScene(string sceneName, float posX, float posY) {
        sceneToLoad = sceneName;
        positionX = posX;
        positionY = posY;
    }

    public GameObject GetPlayer() {
        if (player != null)
            return player.gameObject;
        else
            return null;
    }

    public void SaveGameData() {
        SetNewScene(SceneManager.GetActiveScene().name, player.transform.position.x, player.transform.position.y);

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
        data.positionX = positionX;
        data.positionY = positionY;
        data.sceneToLoad = sceneToLoad;
        data.interactableNames = persistenceManager.interactableNames;
        data.interactableStates = persistenceManager.interactableStates;
        data.npcNames = persistenceManager.npcNames;
        data.npcSequences = persistenceManager.npcSequences;

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

            hitPoint = data.hitPoint;
            hitPointMax = data.hitPointMax;
            blockPoint = data.blockPoint;
            blockPointMax = data.blockPointMax;
            baseDamage = data.baseDamage;
            baseDefense = data.baseDefense;
            gold = data.gold;
            inventoryIDs = data.inventoryIDs;
            inventoryQuantities = data.inventoryQuantities;
            equipmentIDs = data.equipmentIDs;
            equippedSwordID = data.equippedSwordID;
            equippedBowID = data.equippedBowID;
            equippedShieldID = data.equippedShieldID;
            positionX = data.positionX;
            positionY = data.positionY;
            sceneToLoad = data.sceneToLoad;
            interactableNames = data.interactableNames;
            interactableStates = data.interactableStates;
            npcNames = data.npcNames;
            npcSequences = data.npcSequences;

            Debug.Log(sceneToLoad);
            SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
            Debug.Log("Game data loaded.");
        } else {
            Debug.Log("no file to load");
        }
        
    }

    // Game State
    public void SaveGameState() {
        string gameState = "";

        gameState += sceneToLoad + "|";
        gameState += positionX.ToString() + "|";
        gameState += positionY.ToString() + "|";
    
        PlayerPrefs.SetString("GameState", gameState);
        Debug.Log("Game state saved.");
    }

    public void LoadGameState(Scene scene, LoadSceneMode mode) {
        if (!PlayerPrefs.HasKey("GameState"))
            return;

        string[] gameStateData = PlayerPrefs.GetString("GameState").Split('|');
        
        if (SceneManager.GetActiveScene().name == gameStateData[0])
            player.transform.position = new Vector3(float.Parse(gameStateData[1]), float.Parse(gameStateData[2]), 0f);

        UpdateHealthBar(player.hitPoint, player.hitPointMax);
        UpdateBlockBar(player.blockPoint, player.blockPointMax);
        Debug.Log("Game state loaded.");
    }

    public void SetUpPlayer() {
        if (newGame) {
            equippedSwordID = player.GetEquipment(EquipType.sword).equipID;
            equippedBowID = player.GetEquipment(EquipType.bow).equipID;
            equippedShieldID = player.GetEquipment(EquipType.shield).equipID;

            for(int i = 0; i < inventoryManager.equipDatabase.Count; i++) {
                if (equippedSwordID == inventoryManager.equipDatabase[i].itemID)
                    AddItem(inventoryManager.equipDatabase[i].itemID, inventoryManager.equipDatabase[i].itemName, inventoryManager.equipDatabase[i].itemQuantity,
                            inventoryManager.equipDatabase[i].itemSprite, inventoryManager.equipDatabase[i].itemDescription, inventoryManager.equipDatabase[i].itemType);
                else if (equippedBowID == inventoryManager.equipDatabase[i].itemID)
                    AddItem(inventoryManager.equipDatabase[i].itemID, inventoryManager.equipDatabase[i].itemName, inventoryManager.equipDatabase[i].itemQuantity,
                            inventoryManager.equipDatabase[i].itemSprite, inventoryManager.equipDatabase[i].itemDescription, inventoryManager.equipDatabase[i].itemType);
                else if (equippedShieldID == inventoryManager.equipDatabase[i].itemID)
                    AddItem(inventoryManager.equipDatabase[i].itemID, inventoryManager.equipDatabase[i].itemName, inventoryManager.equipDatabase[i].itemQuantity,
                            inventoryManager.equipDatabase[i].itemSprite, inventoryManager.equipDatabase[i].itemDescription, inventoryManager.equipDatabase[i].itemType);
            }
        } else {
            player.hitPoint = hitPoint;
            player.hitPointMax = hitPointMax;
            player.blockPoint = blockPoint;
            player.blockPointMax = blockPointMax;
            player.baseDamage = baseDamage;
            player.baseDefense = baseDefense;
            player.gold = gold;
            player.transform.position = new Vector3(positionX, positionY, 0f);
            persistenceManager.interactableNames = interactableNames;
            persistenceManager.interactableStates = interactableStates;
            persistenceManager.npcNames = npcNames;
            persistenceManager.npcSequences = npcSequences;

            if (File.Exists(filePath)) {
                for (int i = 0; i < inventoryManager.itemSlots.Length; i++) {
                    inventoryManager.itemSlots[i].EmptySlot();
                }

                for (int i = 0; i < inventoryManager.equipSlots.Length; i++) {
                    inventoryManager.equipSlots[i].EmptySlot();
                }

                // Player Inventory
                for (int i = 0; i < inventoryIDs.Length; i++) {
                    for (int j = 0; j < inventoryManager.itemDatabase.Count; j++) {
                        if (inventoryIDs[i] == inventoryManager.itemDatabase[j].itemID) {
                            AddItem(inventoryManager.itemDatabase[j].itemID, inventoryManager.itemDatabase[j].itemName, inventoryQuantities[i],
                                inventoryManager.itemDatabase[j].itemSprite, inventoryManager.itemDatabase[j].itemDescription, inventoryManager.itemDatabase[j].itemType);
                            break;
                        }
                    }
                }

                // Player Equipment
                for (int i = 0; i < equipmentIDs.Length; i++) {
                    for (int j = 0; j < inventoryManager.equipDatabase.Count; j++) {
                        if (equipmentIDs[i] == inventoryManager.equipDatabase[j].itemID) {
                            AddItem(inventoryManager.equipDatabase[j].itemID, inventoryManager.equipDatabase[j].itemName, inventoryManager.equipDatabase[j].itemQuantity,
                                inventoryManager.equipDatabase[j].itemSprite, inventoryManager.equipDatabase[j].itemDescription, inventoryManager.equipDatabase[j].itemType);
                            break;
                        }
                    }
                }
            }
        }

        // Player Equipped Weapons
        for (int i = 0; i < inventoryManager.scriptableEquips.Count; i++) {
            if (inventoryManager.scriptableEquips[i].equipID == equippedSwordID) {
                player.SetEquipment(inventoryManager.scriptableEquips[i]);
                for (int j = 0; j < inventoryManager.equipSlots.Length; j++) {
                    if (inventoryManager.equipSlots[j].itemID == equippedSwordID) {
                        inventoryManager.equipSlots[j].quantityText.text = "E";
                        inventoryManager.equipSlots[j].quantityText.enabled = true;
                        break;
                    }
                }
            } else if (inventoryManager.scriptableEquips[i].equipID == equippedBowID) {
                player.SetEquipment(inventoryManager.scriptableEquips[i]);
                for (int j = 0; j < inventoryManager.equipSlots.Length; j++) {
                    if (inventoryManager.equipSlots[j].itemID == equippedBowID) {
                        inventoryManager.equipSlots[j].quantityText.text = "E";
                        inventoryManager.equipSlots[j].quantityText.enabled = true;
                        break;
                    }
                }
            } else if (inventoryManager.scriptableEquips[i].equipID == equippedShieldID) {
                player.SetEquipment(inventoryManager.scriptableEquips[i]);
                for (int j = 0; j < inventoryManager.equipSlots.Length; j++) {
                    if (inventoryManager.equipSlots[j].itemID == equippedShieldID) {
                        inventoryManager.equipSlots[j].quantityText.text = "E";
                        inventoryManager.equipSlots[j].quantityText.enabled = true;
                        break;
                    }
                }
            }
        }

        UpdateHealthBar(player.hitPoint, player.hitPointMax);
        UpdateBlockBar(player.blockPoint, player.blockPointMax);
        
    }

    // Floating Text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration) {
        floatingTextManager.ShowText(msg, fontSize, color, position, motion, duration);
    }

    // Debug UI
    public void UpdateDebugUI(string txt) {
        hudManager.UpdateDebugUI(txt);
    }

    // Health Bar
    public void UpdateHealthBar(float hitPoint, float hitPointMax) {
        hudManager.UpdateHealthBar(hitPoint, hitPointMax);
    }

    // Block Bar
    public void UpdateBlockBar(float blockPoint, float blockPointMax) {
        hudManager.UpdateBlockBar(blockPoint, blockPointMax);
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

    public Item SearchDatabase(ItemType itemType, int itemID) {
        return inventoryManager.SearchDatabase(itemType, itemID);
    }

    public void RemoveItem(int itemID) {
        inventoryManager.RemoveItem(itemID);
    }

    public void UpdateStatsUI(float hitPoint, float hitPointMax, float blockPoint, float blockPointMax, int swordAtk, int bowAtk, int shieldDef) {
        inventoryManager.UpdateStatsUI(hitPoint, hitPointMax, blockPoint, blockPointMax, swordAtk, bowAtk, shieldDef);
    }

    public void UpdateSingleStat(string statName, float statValue, float statMax) {
        inventoryManager.UpdateSingleStat(statName, statValue, statMax);
    }

    public void InitiateDialogue(DialogueSO conversation, NPC npc) {
        dialogueManager.InitiateDialogue(conversation, npc);
    }

    public void FindInteractables(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name != "MainMenuScene")
            persistenceManager.FindInteractables(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void SetInteractableState(string name, bool state) {
        persistenceManager.SetInteractableState(name, state);
    }

    public void FindNPCs(Scene scene, LoadSceneMode mode) {
        if (SceneManager.GetActiveScene().name != "MainMenuScene")
            persistenceManager.FindNPCs(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    public void SetDialogueSequence(string name, int sequence) {
        persistenceManager.SetDialogueSequence(name, sequence);
    }

    public void Shop(string shopType) {
        inventoryManager.Shop(shopType);
    }

    public void RestoreHealth(int amount) {
        player.RestoreHealth(amount);
    }

    public int GetHitPoint() {
        return player.GetHitPoint();
    }

    public int GetHitPointMax() {
        return player.GetHitPointMax();
    }

    public float GetBlockPoint() {
        return player.GetBlockPoint();
    }

    public float GetBlockPointMax() {
        return player.GetBlockPointMax();
    }

    public int GetMeleeDamage() {
        return player.GetMeleeDamage();
    }

    public int GetRangedDamage() {
        return player.GetRangedDamage();
    }

    public int GetBlockFactor() {
        return player.GetBlockFactor();
    }

    public SpriteRenderer GetDisplaySprite() {
        return player.GetDisplaySprite();
    }

    public TMP_Text GetNoItemText() {
        return player.GetNoItemText();
    }

    public bool GetGameState() {
        return newGame;
    }

    public void PauseMenu() {
        uiManager.PauseMenu();
    }

    public bool IsUIOpen() {
        return inventoryManager.open;
    }

    public bool IsDialogueActive() {
        return dialogueManager.dialogueActive;
    }

    public void SetNewGame(bool newGame) {
        this.newGame = newGame;
    }
}

