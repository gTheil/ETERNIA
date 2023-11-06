using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake() {
        if (GameManager.instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        PlayerPrefs.DeleteAll();

        SceneManager.sceneLoaded += GetReferences;
        SceneManager.sceneLoaded += LoadGameState;
        DontDestroyOnLoad(gameObject);
    }

    // References
    private Player player;
    private FloatingTextManager floatingTextManager;
    private UIManager uiManager;
    private InventoryManager inventoryManager;

    // Logic
    public float newPositionX;
    public float newPositionY;
    public string sceneToLoad;

    private void GetReferences(Scene scene, LoadSceneMode mode) {
        player = GameObject.Find("Player").GetComponent<Player>();
        floatingTextManager = GameObject.Find("FloatingTextManager").GetComponent<FloatingTextManager>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        inventoryManager = GameObject.Find("MenuCanvas").GetComponent<InventoryManager>();
    }

    public void SetNewScene(string sceneName, float posX, float posY) {
        sceneToLoad = sceneName;
        newPositionX = posX;
        newPositionY = posY;
    }

    public GameObject GetPlayer() {
        return player.gameObject;
    }

    public void SaveGameData() {
        string gameData = "";

        gameData += sceneToLoad + "|";
        gameData += newPositionX.ToString() + "|";
        gameData += newPositionY.ToString() + "|";
        gameData += player.hitPoint.ToString() + "|";
        gameData += player.hitPointMax.ToString() + "|";
        gameData += player.gold.ToString();

        PlayerPrefs.SetString("GameData", gameData);
        Debug.Log("Game data saved.");

    }

    public void LoadGameData() {
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
        SceneManager.LoadScene(gameData[0], LoadSceneMode.Single);
        Debug.Log("Game data loaded.");
    }

    // Game State
    public void SaveGameState() {
        string gameState = "";

        gameState += sceneToLoad + "|";
        gameState += newPositionX.ToString() + "|";
        gameState += newPositionY.ToString() + "|";
        gameState += player.hitPoint.ToString() + "|";
        gameState += player.hitPointMax.ToString() + "|";
        gameState += player.gold.ToString();

        PlayerPrefs.SetString("GameState", gameState);
        Debug.Log("Game state saved.");
    }

    public void LoadGameState(Scene scene, LoadSceneMode mode) {
        if (!PlayerPrefs.HasKey("GameState"))
            return;

        string[] gameStateData = PlayerPrefs.GetString("GameState").Split('|');
        
        if (SceneManager.GetActiveScene().name == gameStateData[0])
            player.transform.position = new Vector3(float.Parse(gameStateData[1]), float.Parse(gameStateData[2]), 0f);

        player.hitPoint = int.Parse(gameStateData[3]);
        player.hitPointMax = int.Parse(gameStateData[4]);
        player.gold = int.Parse(gameStateData[5]);

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
    public void AddItem(int itemID, string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType) {
        inventoryManager.AddItem(itemID, itemName, quantity, itemSprite, itemDescription, itemType);
    }

    public bool UseItem(int itemID) {
        bool usable = inventoryManager.UseItem(itemID);
        UpdateHealthBar(player.hitPoint, player.hitPointMax);
        return usable;
    }
}
