using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Web.Script.Serialization;
using System.Web.Services;

public class GreenFootprintService: WebService
{
    string filePath;

    public GreenFootprintService()
    {    // go through the user database as needed
        filePath = Context.Server.MapPath("~/App_Data/users.json");
    }

    public class User // keep track of individuals
    {
        public string username { get; set; }
        public int score { get; set; }
        public string passwordHash{ get; set; }//Required Hashing DLL
    }

    public class UserList // keep track of total users in a list
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
        // get the list of the users
    }

    private void SaveData(UserList data)
    {
        JavaScriptSerializer js = new JavaScriptSerializer();
        string json = js.Serialize(data);
        File.WriteAllText(filePath, json);
        // update the data to the database
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
        //intialize the user into the database
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
        // could not find the user, return -1
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
                if (u.score <= 1)
                {
                    // no negative scores allowed
                    u.score = 0;
                    return u.score;
                }
                else
                {
                    u.score -= 1;
                    SaveData(data);
                    return u.score;
                }
            }
        }
        // could not find the user, return -1
        return -1;
    }

    [WebMethod]
    public int GetFootprintScore(string username)
    {
        var data = LoadData();

        foreach (var u in data.users)
        {
            if (u.username == username)
            {
                return u.score;
            }
        }
        // could not find the user, return -1
        return -1;
    }

    [WebMethod]
    public string GetGreenState(string username)
    {
        int score = GetFootprintScore(username);

        if (score < 0) return "User not found";
        else if (score > 0 && score <= 25) return "Sapling";
        else if (score > 25 && score <= 50) return "Sprout";
        else if (score > 50 && score <= 75) return "Plant";
        else return "Tree";
    }
}
