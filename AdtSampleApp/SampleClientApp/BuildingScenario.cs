using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Azure;

namespace SampleClientApp
{
    public class BuildingScenario
    {
        private CommandLoop cl;
        public BuildingScenario(CommandLoop cl_)
        {
            cl = cl_;
        }

        public async Task InitBuilding()
        {
            //Log.Alert($"Deleting all twins...");
            //await cl.DeleteAllTwinsAsync();
            //Log.Out($"Creating 1 floor, 1 room and 1 thermostat...");
            await CreateModels();
            await initializeSpaceGraph();
            await initializeDevicesGraph();
            await initializeSensorGraphs();
        }


        private async Task CreateModels()
        {
            string[] models_to_upload = new string[2] { "CreateModels", "Space" };
            Log.Out($"Uploading {string.Join(", ", models_to_upload)} models");

            await cl.CommandCreateModels(models_to_upload);

            models_to_upload = new string[5] { "CreateModels",  "Region", "Building", "Floor", "Room" };
            Log.Out($"Uploading {string.Join(", ", models_to_upload)} models");

            await cl.CommandCreateModels(models_to_upload);

            models_to_upload = new string[6] { "CreateModels", "Device", "VirtualGatewayDevice", "Sensor", "PersonCountSensor", "Motion" };
            Log.Out($"Uploading {string.Join(", ", models_to_upload)} models");

            await cl.CommandCreateModels(models_to_upload);
        }
        private async Task initializeDevicesGraph()
        {
            Log.Out($"Creating Devices");
            await cl.CommandCreateDigitalTwin(new string[12]
            {
                "CreateTwin", "dtmi:microsoft:Device:VirtualGatewayDevice;4", "VergeSense_MilleniumF_Floor1",
                "status", "string", "provisioned",
                "id", "string", "VergeSense_MilleniumF_Floor1",
                "name", "string", "VergeSense_MilleniumF_Floor1",
            });
            await cl.CommandCreateRelationship(new string[5]
         {
                "CreateEdge", "MilleniumF_Floor1", "hasDevices", "VergeSense_MilleniumF_Floor1", Guid.NewGuid().ToString()
         });
            await cl.CommandCreateRelationship(new string[5]
           {
                "CreateEdge", "VergeSense_MilleniumF_Floor1", "isInSpace", "MilleniumF_Floor1", Guid.NewGuid().ToString()
           });

            await cl.CommandCreateDigitalTwin(new string[12]
           {
                "CreateTwin", "dtmi:microsoft:Device:VirtualGatewayDevice;4", "Schnieder_MilleniumF_Floor1",
                "status", "string", "provisioned",
                "id", "string", "Schnieder_MilleniumF_Floor1",
                "name", "string", "Schnieder_MilleniumF_Floor1",
           });
            await cl.CommandCreateRelationship(new string[5]
          {
                "CreateEdge", "MilleniumF_Floor1", "hasDevices", "Schnieder_MilleniumF_Floor1", Guid.NewGuid().ToString()
          });
            await cl.CommandCreateRelationship(new string[5]
           {
                "CreateEdge", "Schnieder_MilleniumF_Floor1", "isInSpace", "MilleniumF_Floor1", Guid.NewGuid().ToString()
           });

        }

        private async Task initializeSensorGraphs()
        {
            Log.Out($"Creating Sensors");
            await cl.CommandCreateDigitalTwin(new string[09]
            {
                "CreateTwin", "dtmi:microsoft:Sensor:PersonCountSensor;4", "VS_PersonCount_MilleniumF_Floor1_1507",
                "pollRate", "integer", "3",
                "id", "string", "VS_PersonCount_MilleniumF_Floor1_1507"
            });
            await cl.CommandCreateRelationship(new string[5]
            {
                "CreateEdge", "VS_PersonCount_MilleniumF_Floor1_1507", "isInSpace", "MilleniumF_Floor1", Guid.NewGuid().ToString()
            });
            await cl.CommandCreateRelationship(new string[5]
            {
                "CreateEdge", "MilleniumF_Floor1", "hasSensors", "VS_PersonCount_MilleniumF_Floor1_1507",  Guid.NewGuid().ToString()
            });
            await cl.CommandCreateRelationship(new string[5]
            {
                "CreateEdge", "VS_PersonCount_MilleniumF_Floor1_1507", "hasParentDevices", "VergeSense_MilleniumF_Floor1", Guid.NewGuid().ToString()
            });
            await cl.CommandCreateRelationship(new string[5]
            {
                "CreateEdge", "VergeSense_MilleniumF_Floor1", "hasSensors", "VS_PersonCount_MilleniumF_Floor1_1507", Guid.NewGuid().ToString()
            });

            await cl.CommandCreateDigitalTwin(new string[09]
           {
                "CreateTwin", "dtmi:microsoft:Sensor:MotionSensor;4", "Motion_MilleniumF_Floor1_1507",
                "pollRate", "integer", "4",
                "id", "string", "Motion_MilleniumF_Floor1_1507",
           });
            await cl.CommandCreateRelationship(new string[5]
            {
                "CreateEdge", "Motion_MilleniumF_Floor1_1507", "isInSpace", "MilleniumF_Floor1",  Guid.NewGuid().ToString()
            });
            await cl.CommandCreateRelationship(new string[5]
            {
                "CreateEdge", "MilleniumF_Floor1", "hasSensors", "Motion_MilleniumF_Floor1_1507",  Guid.NewGuid().ToString()
            });
            await cl.CommandCreateRelationship(new string[5]
            {
                "CreateEdge", "Motion_MilleniumF_Floor1_1507", "hasParentDevices", "Schnieder_MilleniumF_Floor1", Guid.NewGuid().ToString()
            });
            await cl.CommandCreateRelationship(new string[5]
            {
                "CreateEdge", "Schnieder_MilleniumF_Floor1", "hasSensors", "Motion_MilleniumF_Floor1_1507",  Guid.NewGuid().ToString()
            });

         }
        private async Task initializeSpaceGraph()
        {

            Log.Out($"Creating Region, Building, Floor and ConferenceRoom");
            await cl.CommandCreateDigitalTwin(new string[18]
            {
                "CreateTwin", "dtmi:microsoft:Space:Region;4", "PugetSound",
                "roomKey", "string", "PugetSound",
                "status", "string", "active",
                "id", "string", "PugetSound",
                "name", "string", "Puget Sound",
                "friendlyName", "string",  "Puget Sound"
            });
            await cl.CommandCreateDigitalTwin(new string[18]
             {
                "CreateTwin", "dtmi:microsoft:Space:Building;4", "MilleniumF",
                "roomKey", "string", "Millenium F",
                "status", "string", "active",
                "id", "string", "Millenium F",
                "name", "string", "Millenium F",
                "friendlyName", "string",  "Millenium F"
             });
            await cl.CommandCreateDigitalTwin(new string[18]
            {
                "CreateTwin", "dtmi:microsoft:Space:Floor;4", "MilleniumF_Floor1",
                "roomKey", "string", "Millenium F/Floor 1",
                "status", "string", "active",
                "id", "string", "Millenium F/Floor 1",
                "name", "string", "Millenium F/Floor 1",
                "friendlyName", "string",  "Millenium F/Floor 1"
            });
            await cl.CommandCreateDigitalTwin(new string[21]
           {
                "CreateTwin", "dtmi:microsoft:Space:ConferenceRoom;4", "1507",
                "roomKey", "string", "1507",
                "status", "string", "active",
                "id", "string", "1507",
                "name", "string", "1507",
                "friendlyName", "string",  "1507",
                "size", "enum",  "xs"
           });

            await cl.CommandCreateDigitalTwin(new string[18]
          {
                "CreateTwin", "dtmi:microsoft:Space:Building;4", "MilleniumE",
                "roomKey", "string", "Millenium E",
                "status", "string", "active",
                "id", "string", "Millenium E",
                "name", "string", "Millenium E",
                "friendlyName", "string",  "Millenium E"
          });

            await cl.CommandCreateDigitalTwin(new string[18]
            {
                "CreateTwin", "dtmi:microsoft:Space:Floor;4", "MilleniumE_Floor1",
                "roomKey", "string", "Millenium E_Floor 1",
                "status", "string", "active",
                "id", "string", "Millenium E_Floor 1",
                "name", "string", "Millenium E_Floor 1",
                "friendlyName", "string",  "Millenium E_Floor 1"
            });
            await cl.CommandCreateDigitalTwin(new string[21]
           {
                "CreateTwin", "dtmi:microsoft:Space:ConferenceRoom;4", "MilleniumE_Floor1_1010",
                "roomKey", "string", "Millenium E_Floor 1_1010",
                "status", "string", "active",
                "id", "string", "Millenium E_Floor 1_1010",
                "name", "string", "Millenium E_Floor 1_1010",
                "friendlyName", "string",  "Millenium E_Floor 1_1010",
                "size", "enum",  "xl"
           });

            Log.Out($"Creating edges between the Floor, Room and Buildings");
            await cl.CommandCreateRelationship(new string[5]
            {
                "CreateEdge", "PugetSound", "hasChildren", "MilleniumF", "RegionToBuildingEdge"
            });
            await cl.CommandCreateRelationship(new string[5]
            {
                "CreateEdge", "PugetSound", "hasChildren", "MilleniumE", "RegionToBuildingEdge2"
            });

            await cl.CommandCreateRelationship(new string[5]
           {
                "CreateEdge", "MilleniumF", "hasChildren", "MilleniumF_Floor1", "BuildingToFloorEdge1"
           }); 
            await cl.CommandCreateRelationship(new string[5]
           {
                "CreateEdge", "MilleniumE", "hasChildren", "MilleniumE_Floor1", "BuildingToFloorgEdge2"
           });

            await cl.CommandCreateRelationship(new string[5]
           {
                "CreateEdge", "MilleniumF_Floor1", "hasChildren", "1507", "FloortoRoomEdge1"
           });
            await cl.CommandCreateRelationship(new string[5]
           {
                "CreateEdge", "MilleniumE_Floor1", "hasChildren", "MilleniumE_Floor1_1010", "FloortoRoomEdge2"
           });

            await cl.CommandCreateRelationship(new string[5]
           {
                "CreateEdge", "PugetSound", "hasDescendants", "MilleniumF_Floor1", "RegionToFloorEdge1"
           });
            await cl.CommandCreateRelationship(new string[5]
           {
                "CreateEdge", "PugetSound", "hasDescendants", "MilleniumE_Floor1", "RegionToFloorEdge2"
           });

            await cl.CommandCreateRelationship(new string[5]
           {
                "CreateEdge", "PugetSound", "hasDescendants", "1507", "RegionToRoomEdge1"
           });
            await cl.CommandCreateRelationship(new string[5]
           {
                "CreateEdge", "PugetSound", "hasDescendants", "MilleniumE_Floor1_1010", "RegionToRoomEdge2"
           });

            await cl.CommandCreateRelationship(new string[5]
         {
                "CreateEdge", "MilleniumF", "hasDescendants", "1507", "BuildinToRoomEdge1"
         });
            await cl.CommandCreateRelationship(new string[5]
           {
                "CreateEdge", "MilleniumE", "hasDescendants", "MilleniumE_Floor1_1010", "BuildinToRoomEdge2"
           });
        }

        //private async Task initializeGraph()
        //{
        //    string[] models_to_upload = new string[3] { "CreateModels", "ThermostatModel", "SpaceModel" };
        //    Log.Out($"Uploading {string.Join(", ", models_to_upload)} models");

        //    await cl.CommandCreateModels(models_to_upload);

        //    Log.Out($"Creating SpaceModel and Thermostat...");
        //    await cl.CommandCreateDigitalTwin(new string[15]
        //    {
        //        "CreateTwin", "dtmi:contosocom:DigitalTwins:Space;1", "floor1",
        //        "DisplayName", "string", "Floor 1",
        //        "Location", "string", "Puget Sound",
        //        "Temperature", "double", "0",
        //        "ComfortIndex", "double", "0"
        //    });
        //    await cl.CommandCreateDigitalTwin(new string[15]
        //    {
        //        "CreateTwin", "dtmi:contosocom:DigitalTwins:Space;1", "room21",
        //        "DisplayName", "string", "Room 21",
        //        "Location", "string", "Puget Sound",
        //        "Temperature", "double", "0",
        //        "ComfortIndex", "double", "0"
        //    });
        //    await cl.CommandCreateDigitalTwin(new string[18]
        //    {
        //        "CreateTwin", "dtmi:contosocom:DigitalTwins:Thermostat;1", "thermostat67",
        //        "DisplayName", "string", "Thermostat 67",
        //        "Location", "string", "Puget Sound",
        //        "FirmwareVersion", "string", "1.3.9",
        //        "Temperature", "double", "0",
        //        "ComfortIndex", "double", "0"
        //    });

        //    Log.Out($"Creating edges between the Floor, Room and Thermostat");
        //    await cl.CommandCreateRelationship(new string[11]
        //    {
        //        "CreateEdge", "floor1", "contains", "room21", "floor_to_room_edge",
        //        "ownershipUser", "string", "Contoso",
        //        "ownershipDepartment", "string", "Comms Division"
        //    });
        //    await cl.CommandCreateRelationship(new string[11]
        //    {
        //        "CreateEdge", "room21", "contains", "thermostat67", "room_to_therm_edge",
        //        "ownershipUser", "string", "Contoso",
        //        "ownershipDepartment", "string", "Comms Division"
        //    });
        //}
    }
}
