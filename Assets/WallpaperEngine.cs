using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallpaperEngine : MonoBehaviour
{
//    public Text Label;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public delegate void ChangeSchemeColor(Color Value);
    public ChangeSchemeColor ChangeSchemeColorHandler;
    public void OnSchemeColor( int Value )
    {
        ChangeSchemeColorHandler(new Color(((Value >> 16) & 0xFF) / 256f, ((Value >> 8) & 0xFF) / 256f, (Value & 0xFF) / 256f));
//        Label.text = "OnSchemeColor: " + Value.ToString("x6");
//        Label.color = new Color( ((Value >> 16) & 0xFF) / 256f, ((Value >> 8)&0xFF) / 256f, (Value & 0xFF) / 256f );
    }

    public delegate void ChangeCustomColor(Color Value);
    public ChangeCustomColor ChangeCustomColorHandler;
    public void OnCustomColor(int Value)
    {
        ChangeCustomColorHandler(new Color(((Value >> 16) & 0xFF) / 256f, ((Value >> 8) & 0xFF) / 256f, (Value & 0xFF) / 256f));
        //        Label.text = "OnCustomColor: " + Value.ToString("x6");
        //        Label.color = new Color(((Value >> 16) & 0xFF) / 256f, ((Value >> 8) & 0xFF) / 256f, (Value & 0xFF) / 256f);
    }

    public void OnCustomBool(int Value)
    {
//        Label.text = "OnCustomBool: " + Value;
    }

    public void OnCustomInt(int Value)
    {
//        Label.text = "OnCustomInt: " + Value;
    }

    public void OnCustomCombo(int Value)
    {
//        Label.text = "OnCustomCombo: " + Value;
    }
}
