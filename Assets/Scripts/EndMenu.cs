using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private string screenshotFolder = "Screenshots";
    [SerializeField] private Image endImage;
    [SerializeField] private RectTransform endScores;

    private Texture2D screenshot;
    
    private void Awake() {
        
    }

    private void OnEnable() {
        Debug.Log("Ended game");
        // When the menu gets enabled, means that the game is over
        screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        SaveScreenshot();
        // Create sprite
        endImage.sprite = Sprite.Create(screenshot, new Rect(0.0f,0.0f,screenshot.width,screenshot.height), new Vector2(0.5f,0.5f));
    }

    private void SaveScreenshot() {
        if (screenshot != null) {
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
        }
    }
}
