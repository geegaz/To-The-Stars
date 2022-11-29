using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    [Header("Screenshot")]
    [SerializeField] private string screenshotFolder = "Screenshots";
    [SerializeField] private Camera screenshotCam;

    [Header("Display")]
    [SerializeField] private Image endImage;
    [SerializeField] private RectTransform endScores;

    private Texture2D screenshot;
    
    private void Awake() {
        
    }

    private void OnEnable() {
        Debug.Log("Ended game");
        // When the menu gets enabled, means that the game is over
        StartCoroutine("SaveScreenshot");
        
    }

    IEnumerator SaveScreenshot() {
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
