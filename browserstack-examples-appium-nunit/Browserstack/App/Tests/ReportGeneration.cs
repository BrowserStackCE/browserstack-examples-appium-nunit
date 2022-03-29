using System;

using NUnit.Framework;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;
using BrowserStack.App.Pages;
using System.IO;

namespace browserstack_examples_appium_nunit.Browserstack.App.Tests
{
    public class ReportGeneration
    {
        [Test]
        public async Task ReportGenerator()
        {
            var BuildId = "";
            var Limit = 100;
            var Offset = 0;
            var userName = Environment.GetEnvironmentVariable("BROWSERSTACK_USERNAME") ?? "";
            var accessKey = Environment.GetEnvironmentVariable("BROWSERSTACK_ACCESS_KEY") ?? "";
            var buildName = Environment.GetEnvironmentVariable("BROWSERSTACK_BUILD_NAME") ?? "";
            //var buildName = "Sales Demo";


            var fileLocation = "../../../../Reports/output.html";
            string[] htmlStart =
                {
        "<!DOCTYPE html><html><head><style>table {  font-family: arial, sans-serif; border-collapse: collapse; width: 100%;}td, th{ border: 1px solid #dddddd;  text-align: left;  padding: 8px;}</style></head><body><table>",
        "<tr><th>Build Name</th><th>Project Name</th><th>Session ID</th><th>Session Name</th><th>OS Name</th><th>OS Version</th><th>Device</th><th>Status</tr>"

        };

            string[] htmlEnd = { "</table></body></html>" };

            await System.IO.File.WriteAllLinesAsync(fileLocation, htmlStart);

            var client = new RestClient("https://api.browserstack.com");
            client.Authenticator = new HttpBasicAuthenticator(userName, accessKey);

            var buildApiRequest = new RestRequest("https://api.browserstack.com/app-automate/builds.json?limit=40");
            var buildQueryResult = await client.ExecuteAsync(buildApiRequest);
            var buildJsonStr = buildQueryResult.Content ?? "";

            //GET BUILD ID FROM BUILD NAME
            if (!buildJsonStr.Equals(""))
            {
                var buildList = JArray.Parse(buildJsonStr);
                if (buildList.ToString().Length != 0)
                {

                    foreach (JObject buildObj in buildList)
                    {
                        var build = buildObj["automation_build"];
                        var bName = (string?)build["name"];
                        if (buildName.Equals(bName))
                        {
                            BuildId = build["hashed_id"].ToString();
                            break;
                        }
                    }
                }
            }

            if (BuildId.Equals(""))
            {
                Console.Write("Could not find a Build");

            }
            else
            {
                //GET SESSIONS FROM BUILD ID
                do
                {
                    var request = new RestRequest("/app-automate/builds/" + BuildId + "/sessions.json?limit=" + Limit + "&offset=" + Offset, Method.Get);
                    var queryResult = await client.ExecuteAsync(request);
                    var jsonStr = queryResult.Content ?? "";

                    //Check if Not an empty array
                    if (jsonStr.Length > 2)
                    {
                        var sessionList = JArray.Parse(jsonStr);
                        if (sessionList.ToString().Length != 0)
                        {

                            foreach (JObject sessionObj in sessionList)
                            {

                                if (sessionObj != null)
                                {
                                    var session = sessionObj["automation_session"];
                                    var browserUrl = session["browser_url"].ToString().Replace("/builds", "/dashboard/v2/builds");
                                    var sessionName = session["name"].ToString().Length == 0 ? session["name"] : session["hashed_id"];

                                    string[] textContextToAdd = { "<tr><td>" + session["build_name"].ToString() +
                    "</td><td>" + session["project_name"] +
                    "</td><td><a href=" + browserUrl + ">" + sessionName + "</a>" +
                    "</td><td>" + session["name"] +
                    "</td><td>" + session["os"] +
                    "</td><td>" + session["os_version"] +
                        "</td><td>" + session["device"] +
                    "</td><td>" + session["status"] + "</tr>" };

                                    await System.IO.File.AppendAllLinesAsync(fileLocation, textContextToAdd);
                                }
                            }
                        }
                    }
                    else
                    {
                        break;
                    }

                    Offset += Limit;
                } while (true);


                await System.IO.File.AppendAllLinesAsync(fileLocation, htmlEnd);

            }
        }
    
    }
}
