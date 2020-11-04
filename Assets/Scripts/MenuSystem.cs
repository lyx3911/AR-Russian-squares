using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Runtime.InteropServices;

public class MenuSystem : MonoBehaviour
{
    // [DllImport("user32.dll", EntryPoint = "keybd_event")]
    // static extern void keybd_event(
    //         byte bVk,            //虚拟键值 对应按键的ascll码十进制值  
    //         byte bScan,          //0
    //         int dwFlags,         //0 为按下，1按住，2为释放 
    //         int dwExtraInfo      //0
    //     );

    public bool isleft=false, isright=false, isdown=false, isroll=false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = 0;
    }

    //开始新的游戏
    public void NewGame(){
        Application.LoadLevel("ARlevel");
    }

    public void NewGame3D(){
        Application.LoadLevel("ARlevel3D");
    }

    public void PlayAgain(){
        //载入场景level,再来一次
        Application.LoadLevel("ARlevel");
    } 

    public void Quit(){
        //退出游戏
        Application.Quit();
    }

    public void left(){
        // keybd_event(37,0,0,0); //37为leftarray键码
        // keybd_event(37,0,2,0);
        isleft = true;
    }

    public void right(){
        // keybd_event(39,0,0,0); //39为rightarray键码
        // keybd_event(39,0,2,0);
        isright = true;
    }

    public void down(){
        // keybd_event(40,0,0,0); //40为downarray键码
        // keybd_event(40,0,2,0);
        isdown = true;
    }

    public void roll(){
        // keybd_event(38,0,0,0); //38为uparray键码
        // keybd_event(38,0,2,0);
        isroll = true;
    }
}
