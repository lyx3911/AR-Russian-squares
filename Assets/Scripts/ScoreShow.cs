using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreShow : MonoBehaviour
{
    public GameObject ScoreText;
    // Start is called before the first frame update
    void Start()
    {   
        ScoreText.GetComponent<TMP_Text>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        int score = FindObjectOfType<Game>().score;
        ScoreText.GetComponent<TMP_Text>().text = score.ToString();
        
        if(FindObjectOfType<Game>().isGameOver){
            // Debug.Log("Gameover");
            GetComponent<Rigidbody>().useGravity = true;
        }
        else{
            //分数增加,记分牌会上升
            Vector3 pos = transform.position;
            pos.y = (float)(2+(float)score/25.0);
            if(pos.y>22) pos.y = 22;
            GetComponent<Transform>().position = pos;
        }
        
        
    }
}
