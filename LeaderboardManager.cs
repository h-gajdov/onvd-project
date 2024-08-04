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
        texts[1].text = score.ToString();
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
    private static string publicKey = "891189cd306fc2c810a6d2bbb56d7d6ea00ea764c9bf9f7c341c42520fad503b";

    public static void GetLeaderboard(GameObject leaderboardUserPrefab, Transform scrollViewPort)
    {
        LeaderboardCreator.GetLeaderboard(publicKey, ((msg) =>
        {
            foreach (var user in msg)
            {
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
                GameObject userPrefab = Instantiate(leaderboardUserPrefab);
                userPrefab.transform.parent = scrollViewPort;
                uiUser.SetInfo(userPrefab);
                uiUser.Print();
            }
        }));
    }

    public static void SetLeaderboardEntry(string username, float score)
    {
        LeaderboardCreator.UploadNewEntry(publicKey, username, (int)(score*100), (msg) =>
        {
            Debug.Log("New entry added!");
        });
    }
}
