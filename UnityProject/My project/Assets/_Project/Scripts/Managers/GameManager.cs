using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Pour le texte UI

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool hasMicro = false;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI winText; // assigner dans l’inspecteur

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (winText != null)
            winText.gameObject.SetActive(false);
    }

    public void PlayerDied()
    {
        Debug.Log("Game Over");
        Invoke(nameof(RestartLevel), 2f);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Appelée quand le joueur récupère l'argent
    public void WinGame()
    {
        Debug.Log("Partie gagnée !");
        if (winText != null)
        {
            winText.text = "Partie gagnée !";
            winText.gameObject.SetActive(true);
        }

        // Optionnel : bloquer les contrôles du joueur, téléporteur, etc.
    }
}
