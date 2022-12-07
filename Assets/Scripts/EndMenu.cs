using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class EndMenu : MonoBehaviour
{
    [Header("Screenshot")]
    [SerializeField] private string screenshotFolder = "Screenshots";
    [SerializeField] private Camera screenshotCam;

    [Header("Display")]
    [SerializeField] private Image endImage;
    [SerializeField] private RectTransform endScores;
    [Space]
    [SerializeField] private PlayerScore playerScorePrefab;

    [Header("Actions")]
    [SerializeField] private InputActionProperty quitAction;
    [SerializeField] private InputActionProperty restartAction;


    private Texture2D screenshot;
    
    private void Awake() {
        if (quitAction.reference != null) {
            quitAction.reference.action.performed += QuitGame;
        } else quitAction.action.performed += QuitGame;
        if (restartAction.reference != null) {
            restartAction.reference.action.performed += RestartGame;
        } else restartAction.action.performed += RestartGame;

    }

    private void OnEnable() {
        DisplayEndScores();
    }

    public void DisplayEndScores() {
        if (endScores != null) {
            while (endScores.childCount > 0) {
                Destroy(endScores.GetChild(0).gameObject);
            }
            for (int i = 0; i < GameManager.instance.players.Count; i++)
            {
                PlayerScore newScore = Instantiate<PlayerScore>(playerScorePrefab, endScores);
                newScore.playerID = i;
            }
        }
    }

    public void RestartGame(InputAction.CallbackContext ctx) => RestartGame();
    public void RestartGame() {
        // Fast and ugly
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame(InputAction.CallbackContext ctx) => QuitGame();

    public void QuitGame() {
        // Fast and ugly, but doesn't crash on WebGL :)
        if (Application.platform != RuntimePlatform.WebGLPlayer)
            Application.Quit();
    }

    IEnumerator SaveScreenshot() {
        // Currently broken, don't use

        yield return new WaitForEndOfFrame();
        screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        
        // Create and save screenshot to disc
        string folderPath = Application.dataPath+"/../"+screenshotFolder;
        if (!System.IO.Directory.Exists(folderPath)) {
            System.IO.Directory.CreateDirectory(folderPath);
        }
        string filePath = "/ToTheStars_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")+".png";
        System.IO.File.WriteAllBytesAsync(folderPath + filePath, screenshot.EncodeToPNG());
        
        #if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        #endif

        // Create sprite
        endImage.sprite = Sprite.Create(screenshot, new Rect(0.0f, 0.0f, screenshot.width, screenshot.height), new Vector2(0.5f,0.5f));
    }
}
