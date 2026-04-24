using System.IO;
using System.Web.Services;
using System.Xml.Linq;

public class GreenFootprintService : WebService
{
    string filePath;

    public GreenFootprintService()
    {    // go through the user database as needed
        filePath = Context.Server.MapPath("~/App_Data/Member.xml");
    }

    private XDocument LoadData()
    {
        if (!File.Exists(filePath))
        {
            // create a new XML file with root element if it doesn't exist
            XDocument newDoc = new XDocument(new XElement("members"));
            newDoc.Save(filePath);
            return newDoc;
        }
        // get the list of the users
        return XDocument.Load(filePath);
    }

    private void SaveData(XDocument data)
    {
        // update the data to the database
        data.Save(filePath);
    }

    [WebMethod]
    public bool RegisterUser(string username, string password)
    {
        var data = LoadData();
        foreach (var user in data.Root.Elements("member"))
        {
            if (user.Element("username").Value == username)
            {
                return false;
            }
        }
        string hash = HashGen.MakeHash(password); //Required Hashing DLL
        data.Root.Add(new XElement("member",
            new XElement("username", username),
            new XElement("passwordHash", hash),
            new XElement("score", 0),
            new XElement("state", "Sapling")
            )
        );
        SaveData(data);
        return true;
    }

    [WebMethod]
    public bool ValidateUser(string username, string password)
    {
        var data = LoadData();
        string hash = HashGen.MakeHash(password); //Required Hashing DLL
        foreach (var user in data.Root.Elements("member"))
        {
            if (user.Element("username").Value == username && user.Element("passwordHash").Value == hash)
            {
                return true;
            }
        }
        return false;
    }

    [WebMethod]
    public int LogGreenAction(string username, string action)
    {
        var data = LoadData();

        foreach (var user in data.Root.Elements("member"))
        {
            if ((string)user.Element("username") == username)
            {
                int score = (int)user.Element("score");
                score += 1; // each green action adds 1 point
                user.Element("score").Value = score.ToString();
                user.Element("state").Value = GetGreenStateFromScore(score); // update the state based on the new score
                SaveData(data);
                return score;
            }
        }
        // could not find the user, return -1
        return -1;
    }

    [WebMethod]
    public int LogNonGreenAction(string username, string action)
    {
        var data = LoadData();

        foreach (var user in data.Root.Elements("member"))
        {
            if ((string)user.Element("username") == username)
            {
                int score = (int)user.Element("score");
                if (score <= 1)
                {
                    score = 0; // prevent negative scores
                }
                else
                {
                    score -= 1; // each non-green action subtracts 1 point
                }
                user.Element("score").Value = score.ToString();
                user.Element("state").Value = GetGreenStateFromScore(score); // update the state based on the new score
                SaveData(data);
                return score;
            }
        }
        // could not find the user, return -1
        return -1;
    }

    [WebMethod]
    public int GetFootprintScore(string username)
    {
        var data = LoadData();

        foreach (var user in data.Root.Elements("member"))
        {
            if ((string)user.Element("username") == username)
            {
                return (int)user.Element("score");
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
        else
        {
            return GetGreenStateFromScore(score);
        }
    }
    
    private string GetGreenStateFromScore(int score)
    {
        if (score >= 0 && score <= 25) return "Sapling";
        else if (score > 25 && score <= 50) return "Sprout";
        else if (score > 50 && score <= 75) return "Plant";
        else return "Tree";
    }
}
