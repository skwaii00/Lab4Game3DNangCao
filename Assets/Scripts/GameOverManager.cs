using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TextMeshProUGUI gameOverText;

    void Start()
    {
        gameOverText.enabled = false;
    }

    public void GameOver()
    {
        gameOverText.enabled = true;  
    }
}
