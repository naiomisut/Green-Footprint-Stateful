using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Web.Script.Serialization;
using System.Web.Services;

public class GreenFootprintService: WebService
{
    string filePath;

    public GreenFootprintService()
    {
        filePath = Context.Server.MapPath("~/App_Data/users.json");
    }

    public class User
    {
        public string username { get; set; }
        public int score { get; set; }
    }

    public class UserList
    {
        public List<User> users { get; set; } = new List<User>();
    }

    private UserList LoadData()
    {
        if (!File.Exists(filePath))
            return new UserList();

        string json = File.ReadAllText(filePath);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return js.Deserialize<UserList>(json);
    }

    private void SaveData(UserList data)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        string json = js.Serialize(data);
        File.WriteAllText(filePath, json);
    }

    [WebMethod]
    public bool RegisterUser(string username)
    {
        var data = LoadData();

        foreach (var u in data.users)
        {
            if (u.username == username)
                return false;
        }

        data.users.Add(new User { username = username, score = 0 });
        SaveData(data);
        return true;
    }

    [WebMethod]
    public int LogGreenAction(string username, string action)
    {
        var data = LoadData();

        foreach (var u in data.users)
        {
            if (u.username == username)
            {
                u.score += 1;
                SaveData(data);
                return u.score;
            }
        }

        return -1;
    }

    [WebMethod]
    public int LogNonGreenAction(string username, string action)
    {
        var data = LoadData();

        foreach (var u in data.users)
        {
            if (u.username == username)
            {
                u.score -= 1;
                SaveData(data);
                return u.score;
            }
        }

        return -1;
    }

    [WebMethod]
    public int GetFootprintScore(string username)
    {
        var data = LoadData();

        foreach (var u in data.users)
        {
            if (u.username == username)
                return u.score;
        }

        return -1;
    }

    [WebMethod]
    public string GetGreenState(string username)
    {
        int score = GetFootprintScore(username);

        if (score < 0) return "User not found";
        if (score <= 25) return "Sapling";
        if (score <= 50) return "Sprout";
        if (score <= 75) return "Plant";
        return "Tree";
    }
}
