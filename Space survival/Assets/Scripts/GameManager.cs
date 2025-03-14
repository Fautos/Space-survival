using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public ScoreData Leaderboard;
    public bool musicOn = true;

    // ENCAPSULATION:
    // Before setting the user's name, we check if it is in the banned list
    private List<string> bannedNames = new List<string>{"Fox", "Samus", "Pit"};
    private string _userName; 
    public string userName { 
                            get
                            {
                                return _userName;
                            }
                            set
                            {
                                if (bannedNames.Contains(value))
                                {
                                    Debug.LogWarning("Invalid name, please change it");
                                    _userName = "Pilot";
                                }
                                else
                                {
                                    _userName = value;
                                }
                            } }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }   

        Instance = this;
        DontDestroyOnLoad(gameObject);

        initScoreBoard();
    }

    #region Leaderboard
    //  Class to store user/level/points
    [System.Serializable]
    public class ScoreEntry
    {
        public string userName;
        public int level, score;

        public ScoreEntry(string userName, int level, int score)
        {
            this.userName = userName;
            this.level = level;
            this.score = score;
        }
    }

    // Class to create and manage score tables
    [System.Serializable]
    public class ScoreData
    {
        public List<ScoreEntry> topScores = new List<ScoreEntry>();

        public void AddScore(string userName, int level, int score)
        {
            topScores.Add(new ScoreEntry(userName, level, score));
            topScores.Sort((x, y) => 
            {
                // First we compare the scores (from highest to lowest)
                int scoreComparison = y.score.CompareTo(x.score);

                // If the scores are the same we compare the level (from highest to lowest)
                if (scoreComparison == 0)
                {
                    return y.level.CompareTo(x.level);
                }

                return scoreComparison;
            });

            // Lastly we only keep the top 5
            if (topScores.Count > 5)
            {
                topScores.RemoveAt(topScores.Count - 1);
            }

        }

        public string GetElement(string element)
        {
            string outputstring="";

            switch (element)
            {
                case "names":
                    foreach (ScoreEntry entry in topScores)
                    {
                        outputstring+=entry.userName + "\n";   
                    }                            
                    return outputstring;
                
                case "levels":
                    foreach (ScoreEntry entry in topScores)
                    {
                        outputstring+=entry.level + "\n";   
                    }                            
                    return outputstring;
                
                case "scores":
                    foreach (ScoreEntry entry in topScores)
                    {
                        outputstring+=entry.score + "\n";   
                    }                            
                    return outputstring;

                default:
                    return "";

            }

        }
    }

    private void initScoreBoard()
    {
        // First we add some random characters
        Leaderboard.AddScore("Fox", 20, 300);
        Leaderboard.AddScore("Samus", 10, 125);
        Leaderboard.AddScore("Pit", 5, 50);

        // The rest of the leaderboard will be filled without new pilots
        for (int i=0; i<2; i++)
        {
            Leaderboard.AddScore("---", 1, 0);
        }
    }

    #endregion
}
