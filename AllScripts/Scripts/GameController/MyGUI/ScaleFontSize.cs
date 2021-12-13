using UnityEngine;
using PixelCrushers.DialogueSystem.UnityGUI;
using PixelCrushers.DialogueSystem;

public class ScaleFontSize : MonoBehaviour
{

    [System.Serializable]
    public class StyleScale
    {
        public string styleName = string.Empty;
        public float scaleFactor = 0.04f;

        private void Start()
        {
            //     scaleFactor = Screen.width / 800;
            scaleFactor = 5;
        }

    }

    public StyleScale[] styles = new StyleScale[2];
    

    private GUIRoot guiRoot = null;
    //   private float lastScreenHeight = 0f;
    public float rateAdjust = 0.5f;

    void Awake()
    {
        guiRoot = GetComponent<GUIRoot>();
    }

    private void Start()
    {
        //Screen height = 768 standard
        //Screen wide = 1024

        
    }

    void OnGUI()
    {
        if (guiRoot == null || guiRoot.guiSkin == null) return;
   //     if (Screen.height == lastScreenHeight) return;
   //     lastScreenHeight = Screen.height;

        foreach (var style in styles)
        {
            GUIStyle guiStyle = guiRoot.guiSkin.GetStyle(style.styleName);
            if (guiStyle != null)
            {
            //    rateAdjust = DialogueLua.GetVariable("rateAdjust").AsFloat;
       //         Debug.Log(rateAdjust);
                guiStyle.fontSize = (int)(style.scaleFactor * rateAdjust * Screen.height);

                if (style.styleName == "Panel")
                {
                    Debug.Log("Panel");
                    guiStyle.normal.textColor = Color.black;
                    
                }
                
            }
            
        }
        guiRoot.ManualRefresh();
    }
}
