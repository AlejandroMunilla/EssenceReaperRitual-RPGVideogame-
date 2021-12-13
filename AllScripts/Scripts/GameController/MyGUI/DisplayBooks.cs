using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class DisplayBooks : MonoBehaviour {


	private string value;
	private TextAsset textAsset;
	private string [] arrayString;
	private string [] arrayBooks;
	private bool readBook = false;
	private bool nullBookList = false;
	private Rect bookListRect;
	private Rect bookRect;
	private Vector2 bookRectSlider;
	private int posX;
	private int posY;
    private int bookWidth;
    private int bookHeight;
    private Texture library;
	private int buttonHeight = 25;

	void Start () 
	{
		bookRect = new Rect(0, 0, Screen.width * 0.90f, 1000);
		posX = (int) (Screen.width * 0.05f);
		posY = (int) (Screen.height * 0.20f);
        bookWidth = (int)(Screen.width * 0.90f);
        bookHeight = (int)(Screen.height *0.68f);

        library = (Texture)(Resources.Load ("Books/Library", typeof(Texture)));
		readBook = false;
		GetLibrary();
	}


	public void GetLibrary ()
	{
		string books = DialogueLua.GetVariable ("Books").AsString;

		if (books == null || books == "")
		{
			nullBookList = true;
		}
		else
		{
			arrayBooks = books.Split (new string [] {"*"}, System.StringSplitOptions.None);
			nullBookList = false;
		}

	}

	public void Display ()
	{
		if (readBook == true)
		{			
			if (GUI.Button (new Rect(Screen.width * 0.05f, Screen.height *0.11f, Screen.width * 0.11f , Screen.height *0.06f), "CLOSE" ))
			{
				readBook = false;
			}

			bookRectSlider = GUI.BeginScrollView (new Rect (posX, posY, bookWidth, Screen.height * 0.8f), bookRectSlider,
			                                              bookRect);
            GUIContent content = new GUIContent(value);
            GUIStyle style = GUI.skin.label;
            float textDimensions = GUI.skin.label.CalcHeight(new GUIContent(value), Screen.width * 0.7f);

            GUI.Label(new Rect(Screen.width * 0.02f, Screen.height * 0.01f, Screen.width * 0.7f, Screen.height * 1.5f), value);
            bookRect = new Rect(0, 0, Screen.width * 1.4f, textDimensions + posY);
            /*
			for (int cnt = 0; cnt < arrayString.Length ; cnt++)
			{
				GUI.Label (new Rect(1, 20 + (cnt*25), 600 , buttonHeight), arrayString[cnt]); 
			}*/
            GUI.EndScrollView();
		}
		else
		{
			if (nullBookList == false)
			{
                if (arrayBooks == null)
                {
                    return;
                }
				for (int cnt = 0; cnt < arrayBooks.Length ; cnt++)
				{
					if (GUI.Button (new Rect(20, 250 + (cnt*25), 250 , 25), arrayBooks[cnt]) )
					{
						SetPath (arrayBooks[cnt]);
                        Debug.Log(arrayBooks[cnt]);
						readBook = true;
					}
				}
			}


     //       GUI.Label(new Rect(450, 200, 400, 400), library);
            GUI.Label (new Rect(Screen.width * 0.55f, Screen.height * 0.18f, Screen.width * 0.45f , Screen.height *0.6f), library  );
		}
	}

	private void SetPath (string pathResources)
	{ 
		textAsset =  (TextAsset)(Resources.Load ("Books/"+pathResources, typeof(TextAsset)));

        
        //     Debug.Log(pathResources);
        value = textAsset.text;
    //    Debug.Log(value);
   //    	Chop (value);
    }

    public void Chop(string value)
    {
        arrayString = value.Split(new string[] { "*" }, System.StringSplitOptions.None);
        int lines = arrayString.Length + 2;

        bookRect = new Rect(0, 0, Screen.width * 1.4f, (lines * buttonHeight));
    }

    public void AddBook (string bookName)
    {
        string books = DialogueLua.GetVariable("Books").AsString;

        if (books == "" || books == null)
        {

            string booksAdd = bookName;
            DialogueLua.SetVariable("Books", booksAdd);
        }
        else
        {
            string booksAdd = books + "*" + bookName;
            DialogueLua.SetVariable("Books", booksAdd);
        }
        GetLibrary();
        
        DialogueManager.ShowAlert(bookName + " added to books");
        GetComponent<ExpController>().FinalExp(1000);
    }
}
