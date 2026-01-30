using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] ingredientPrefabs;    // 具材のプレハブ
    [SerializeField] private GameObject topBunPrefab;           // 上のバンズ
    [SerializeField] private float spawnInterval = 1.5f;        // 具材が降る間隔
    [SerializeField] private float gameTime = 60f;              // 制限時間
    [SerializeField] private Transform spawnPoint;              // 生成位置
    [SerializeField] private float spawnRangeX = 5f;            // 生成する横幅
    [SerializeField] private TextMeshProUGUI timerText;         // タイマー表示用テキスト
    [SerializeField] private GameObject resultCanvas;           // ResultCanvas
    [SerializeField] private TextMeshProUGUI resultText;        // ResultText
    [SerializeField] private PlayerController playerController; // プレイヤーコントローラー

    private float timer;
    private bool isGameActive = true;
    private bool isEnding = false;

    private void Start()
    {
        timer = gameTime;
        resultCanvas.SetActive(false);
        InvokeRepeating("SpawnIngredient", 1.0f, spawnInterval);
    }

    private void Update()
    {
        if (!isGameActive) return;

        timer -= Time.deltaTime;
        timerText.text = Mathf.Ceil(timer).ToString();

        if (timer <= 0)
        {
            EndGame();
        }
    }

    private void SpawnIngredient()
    {
        if (!isGameActive) return;

        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPos = new Vector3(randomX, spawnPoint.position.y, 0);
        int randomIndex = Random.Range(0, ingredientPrefabs.Length);

        Instantiate(ingredientPrefabs[randomIndex], spawnPos, Quaternion.identity);
    }

    private void EndGame()
    {
        isEnding = true;
        isGameActive = false;
        CancelInvoke("SpawnIngredient");

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null) player.DisableControl();

        float playerX = (player != null) ? player.transform.position.x : 0;
        Vector3 topBunPos = new Vector3(playerX, 10f, 0);
        Instantiate(topBunPrefab, topBunPos, Quaternion.identity);

        Invoke("CalculateScore", 3.0f);
    }

    private void CalculateScore()
    {
        GameObject bottomBun = GameObject.FindWithTag("Player");
        GameObject topBun = GameObject.FindWithTag("TopBun");

        if (bottomBun == null || topBun == null) return;

        Vector2 cornerA = new Vector2(bottomBun.transform.position.x - 2f, bottomBun.transform.position.y);
        Vector2 cornerB = new Vector2(topBun.transform.position.x + 2f, topBun.transform.position.y);

        Collider2D[] results = Physics2D.OverlapAreaAll(cornerA, cornerB);

        int count = 0;
        foreach (var col in results)
        {
            if (col.CompareTag("Ingredient"))
            {
                count++;
            }
        }
        ShowResult(count);
    }
    void ShowResult(int score)
    {
        resultCanvas.SetActive(true);
        resultText.text = score.ToString();
    }
}