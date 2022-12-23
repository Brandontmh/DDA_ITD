using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour{
    public string UserName;
    public string Email;
    public int Hp;
    public int TimeStamp;
    public int score;
    public int kills;
    public int deaths;
    public long updatedOn;


        public void NewScoreElement (string _username, int _kills, int _deaths, int _score, int _TimeStamp)
    {
        UserName = _username;
        kills = _kills;
        deaths = _deaths;
        score = _score;
        TimeStamp = _TimeStamp;
        
    }

}
