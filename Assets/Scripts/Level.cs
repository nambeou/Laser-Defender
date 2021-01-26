using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    int currentLevel = 0;
    [SerializeField] TextMeshProUGUI scoreValue;
    [SerializeField] TextMeshProUGUI speedBonus;
    [SerializeField] TextMeshProUGUI damageBonus;
    public void LoadGameOver() {
        StartCoroutine(LoadSceneWithDelay(1, "GameOver"));
    }

    IEnumerator LoadSceneWithDelay(int delay, string scene) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(scene);
    }

    public void LoadMainMenu() {
        StartCoroutine(LoadSceneWithDelay(2, "MainMenu"));
    }
    public void LoadGame() {
        FindObjectOfType<GameSession>().ResetGameSession();
        StartCoroutine(LoadSceneWithDelay(1, "CoreScene"));
    }
    public void QuitGame() {
        Application.Quit();
    }

    public void LevelUp() {
        this.currentLevel ++;
        scoreValue.text = this.currentLevel.ToString();
        damageBonus.text = (1 + currentLevel*0.2).ToString();
        speedBonus.text = (1 + currentLevel*0.05).ToString();
        Player player= FindObjectOfType<Player>();
        if(player != null) {
            DamageDealer playerDamageDealer = player.GetComponent<DamageDealer>(); 
            playerDamageDealer.SetDamage(playerDamageDealer.GetDamage() + this.currentLevel*10);
        }
    }

    public int GetCurrentLevel() {
        return this.currentLevel;
    }
}
