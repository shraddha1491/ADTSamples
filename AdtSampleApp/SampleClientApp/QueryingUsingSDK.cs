using Azure;
using Azure.DigitalTwins.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VDS.RDF;

namespace SampleClientApp
{
    public class QueryingUsingSDK
    {

        private DigitalTwinsClient client ;
        public QueryingUsingSDK(DigitalTwinsClient dtclient)
        {
            client = dtclient;
        }

        public async Task InitQuery()
        {
            await UsingQuerys();
            await CreateProperty();
            await UpdateProperty();

        }
        public async Task UsingQuerys()
        {
            Console.WriteLine("Get All Buildings");
            List<string> buildings = await Query("select * from DIGITALTWINS T WHERE IS_OF_MODEL(T,'dtmi:microsoft:Space:Building;4') and status = 'active'");

            Console.WriteLine("Get All Floor of a Building");
            List<string> floors = await Query("select Floor from DIGITALTWINS BD JOIN Floor RELATED BD.hasChildren WHERE IS_OF_MODEL(Floor,'dtmi:microsoft:Space:Floor;4') and BD.$dtId = 'MilleniumF' ");

            Console.WriteLine("Get Few Devices Type in a floor includes sensors");
            List<string> devices = await Query("select Device from DIGITALTWINS Floor JOIN Device RELATED Floor.hasDevices WHERE IS_OF_MODEL(Device,'dtmi:microsoft:Device:VirtualGatewayDevice;4') and Floor.$dtId = 'MilleniumF_Floor1' ");
            List<string> sensors = await Query("select Sensor from DIGITALTWINS Device JOIN Sensor RELATED Device.hasSensors WHERE Device.$dtId = 'VergeSense_MilleniumF_Floor1' ");

            Console.WriteLine("Get spaces attached to sensors");
            List<string> sensorSPaces = await Query("select Space from DIGITALTWINS Sensor JOIN Space RELATED Sensor.isInSpace WHERE Sensor.$dtId in [ 'MilleniumF_Floor1'] ");

        }

        public async Task UpdateProperty()
        {
            List<object> twinData = new List<object>();
            twinData.Add(new Dictionary<string, object>() {
                    { "op", "replace"},
                    { "path", "/pollFrequency"},
                    { "value", "00:00:40"}
                });
            string twin_id = "VergeSense_MilleniumF_Floor1";
            await client.UpdateDigitalTwinAsync(twin_id, JsonSerializer.Serialize(twinData));
            List<string> device = await Query("Select * from Digitaltwins T where T.$dtId = 'VergeSense_MilleniumF_Floor1'");

        }

        public async Task CreateProperty()
        {
        List<object> twinData = new List<object>();
        twinData.Add(new Dictionary<string, object>() {
                    { "op", "add"},
                    { "path", "/authType"},
                    { "value", "basic"}
                });
            string twin_id = "VergeSense_MilleniumF_Floor1";
        await client.UpdateDigitalTwinAsync(twin_id, JsonSerializer.Serialize(twinData));
        List<string> device = await Query("Select * from Digitaltwins T where T.$dtId = 'VergeSense_MilleniumF_Floor1'");
        }   

        private async Task<List<string>> Query(string query)
        {
            try
            {
                AsyncPageable<string> qresult = client.QueryAsync(query);
                List<string> reslist = new List<string>();
                await foreach (string item in qresult)
                    reslist.Add(item);
                Print(reslist);
                return reslist;
            }
            catch (RequestFailedException e)
            {
                Log.Error($"Error {e.Status}: {e.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Log.Error($"Error: {ex}");
                return null;
            }
        }

        private void Print(List<string> reslist)
        {
            foreach(string str in reslist)
            {
                object jsonObj = System.Text.Json.JsonSerializer.Deserialize<object>(str);
                var json =  System.Text.Json.JsonSerializer.Serialize(jsonObj, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                Console.WriteLine(json);
            }
        }


    }
}
