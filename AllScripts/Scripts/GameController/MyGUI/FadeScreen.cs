using UnityEngine;

public class FadeScreen : MonoBehaviour {
    private Texture black;
    public float fadeSpeed = 0.04f;
    private int drawDepth = -1000;
    public float alpha = 1;
    public float fadeDir = 1;
    private GameController gameController;

    void Awake ()
    {

        GameObject gc = GameObject.FindGameObjectWithTag("GameController");
        
        if (gc.GetComponent<GameController>())
        {
            gameController = gc.GetComponent<GameController>();
        }
        
    }

    void Start ()
    {
        black = (Texture)(Resources.Load ("Textures/Black", typeof(Texture)));

        if (GetComponent<GameController>())
        {

            gameController.inDialogue = true;
        }
        
    }


    void OnGUI ()
    {
   //     Debug.Log(alpha);
        Color tmpColor = GUI.color;
        alpha += fadeSpeed * Time.deltaTime * fadeDir;
        alpha = Mathf.Clamp01(alpha);
        GUI.color = new Color(1, 1, 1, alpha);
   //     GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), black);

        if (alpha <= 0 && fadeDir == -1)
        {
            this.enabled = false;
        }
    }
}
