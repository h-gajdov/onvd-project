using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Dan.Main;

public class LeaderboardUser
{
    private string position;
    private float score;
    private string name;

    private const string goldHex = "#FFD700";
    private const string silverHex = "#C0C0C0";
    private const string bronzeHex = "#CD7F32"; 
    
    private static Sprite[] medals = new Sprite[3];

    public float Score
    {
        get
        {
            return score;
        }
    }
    
    public LeaderboardUser(string position, float score, string name)
    {
        this.position = position;
        this.score = score;
        this.name = name;
    }

    public void SetInfo(GameObject userPrefab)
    {
        TextMeshProUGUI[] texts = userPrefab.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = position;
        texts[1].text = score.ToString().Replace(',', '.');
        texts[2].text = name;

        string colorHex = "#000000";
        switch (position)
        {
            case "1ST":
                colorHex = goldHex;
                userPrefab.transform.GetChild(3).gameObject.SetActive(true);
                userPrefab.transform.GetChild(3).GetComponent<Image>().sprite = medals[0];
                break;
            case "2ND":
                colorHex = silverHex;
                userPrefab.transform.GetChild(3).gameObject.SetActive(true);
                userPrefab.transform.GetChild(3).GetComponent<Image>().sprite = medals[1];
                break;
            case "3RD":
                colorHex = bronzeHex;
                userPrefab.transform.GetChild(3).gameObject.SetActive(true);
                userPrefab.transform.GetChild(3).GetComponent<Image>().sprite = medals[2];
                break;
        }

        Color textColor;
        if (ColorUtility.TryParseHtmlString(colorHex, out textColor))
        {
            texts[0].color = textColor;   
            texts[1].color = textColor;   
            texts[2].color = textColor;   
        }
    }

    public static void SetSprites(Sprite goldMedal, Sprite silverMedal, Sprite bronzeMedal)
    {
        medals[0] = goldMedal;
        medals[1] = silverMedal;
        medals[2] = bronzeMedal;
    }
    
    public void Print()
    {
        Debug.Log(position + " " + score.ToString() + " " + name);
    }
}

public class LeaderboardManager : MonoBehaviour
{
    private static string publicKey = "682feb9e45b00d9b3b0c98af19f759634433c5fc786dcb8cac13c1468b417192";
    public static List<LeaderboardUser> users = new List<LeaderboardUser>();
    public static bool hasLeaderboard = false;

    public static void GetLeaderboard(GameObject leaderboardUserPrefab, Transform scrollViewPort)
    {
        LeaderboardCreator.GetLeaderboard(publicKey, ((msg) =>
        {
            for (int i = 0; i < msg.Length; i++)
            {
                var user = msg[i];
                string position;
                switch (user.Rank)
                {
                    case 1:
                        position = "1ST";
                        break;
                    case 2:
                        position = "2ND";
                        break;
                    case 3:
                        position = "3RD";
                        break;
                    default:
                        position = user.Rank.ToString() + "TH";
                        break;
                }

                LeaderboardUser uiUser = new LeaderboardUser(position, user.Score / 10f, user.Username);
                users.Add(uiUser);
                
                if (i >= 100) continue;
                
                GameObject userPrefab = Instantiate(leaderboardUserPrefab);
                userPrefab.transform.parent = scrollViewPort;
                uiUser.SetInfo(userPrefab);
                hasLeaderboard = true;
            }
            Debug.Log(hasLeaderboard);
        }), (errCallback) => { hasLeaderboard = false; });
    }

    public static void SetLeaderboardEntry(string username, float score)
    {
        float mult = (float)Math.Round(score, 1);
        LeaderboardCreator.UploadNewEntry(publicKey, username, (int)(mult*10), (msg) =>
        {
            Debug.Log("New entry added!");
        });
    }

    public static int GetRankOfScore(float score)
    {
        if (!hasLeaderboard) return -1;

        int left = 0;
        int right = users.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;

            if (users[mid].Score == score)
            {
                return mid + 1;
            }
            else if (score > users[mid].Score)
            {
                right = mid - 1;
            }
            else
            {
                left = mid + 1;
            }
        }
        Debug.Log(users.Count);
        return (left + 1 >= users.Count && users.Count > 0) ? users.Count : left + 1;
    }
}
