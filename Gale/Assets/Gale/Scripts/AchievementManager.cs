using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class AchievementManager : MonoBehaviour {

    public int score;
    public Image AchievmentImage;

    [System.Serializable]
    public class Achievement
    {
        public string Names;
        public string Description;
        public int score;
        public bool earned;

        public Achievement(string a_name, int a_score, string a_desc)
        {
            Names = a_name;
            Description = a_desc;
            score = a_score;
        }
    }

    public List<Achievement> achievements;
    public List<Achievement> AchievementsEarned;
    // Use this for initialization
    void Start () {
        AddAchievement("First Destroy", 10, "Destory an Object");
        AddAchievement("Win", 100, "Win The Game");
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    void AddAchievement(string name, int score, string Desc)
    {
       achievements.Add(new Achievement(name, score, Desc));
    }

    public void EarnAchievment(string Name)
    {
        for(int i = 0; i < achievements.Count; i++)
        {
            if(achievements[i].Names == Name && achievements[i].earned == false)
            {
                AchievementsEarned.Add((achievements[i]));
                score += achievements[i].score;
                achievements[i].earned = true;
            }
        }
    
       
    }
}
