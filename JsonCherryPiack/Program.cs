using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonCherryPiack
{
    public static class JsonHelper
    {
        public static bool IsNullOrEmpty(this JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //var input = Input;
            //string[] fields = Fields;
            //    var jsonInput = (JObject) JsonConvert.DeserializeObject(input);
            var jtokenInput = JToken.Parse(Input);

            Traverse(jtokenInput, Fields);
            Console.WriteLine($"{Environment.NewLine}FinalJSON: {jtokenInput}");
            //GetFields(input, fields);
        }

        private static void removeFields(JToken token, string[] fields)
        {
            JContainer container = token as JContainer;
            if (container == null) return;

            List<JToken> removeList = new List<JToken>();
            foreach (JToken el in container.Children())
            {
                JProperty p = el as JProperty;
                if (p != null && !fields.Contains(p.Name))
                {
                    removeList.Add(el);
                }

                removeFields(el, fields);
            }

            foreach (JToken el in removeList)
            {
                Console.WriteLine($"Removing: {el}");
                el.Remove();
            }
        }

        public static void Traverse(JToken token, string[] fields)
        {
            JContainer container = token as JContainer;
            if (container == null)
            {
                return;
            }

            foreach (JToken el in container.Children().ToArray())
            {
                Traverse(el, fields);

                var prop = el as JProperty;
                if (prop != null)
                {
                    var name = prop.Name;

                    if (!fields.Contains(prop.Name))
                    {
                        if (!prop.Value.HasValues)
                        {
                            prop.Remove();
                        }

                        //Remove arrays with empty objects
                        //if (prop.Value.Type == JTokenType.Array && prop.Parent != null)
                        //{
                        //    if (prop.Value.All(x => !x.HasValues))
                        //    {
                        //        prop.Remove();
                        //    }
                        //}
                    }
                    else
                    {
                        if (prop.Value.IsNullOrEmpty())
                        {
                            prop.Remove();
                        }
                    }
                }
                //Delete array items that end up being empty objects
                //else if (el.Parent.Type == JTokenType.Array && !el.HasValues)
                //{
                //    el.Remove();
                //}
            }
        }

        [Obsolete]
        private static void GetFields(string json, string[] fieldsToKeep, string root = "DwellingData")
        {
            var jsonObject = JObject.Parse(json);

            //DwellingData is the root object so take it with jsonObject[root].First.Children()
            //Now removing the root DwellingData object to work on generic JSON
            foreach (var child in jsonObject[root].Children())
            {
                var childProperties = (JProperty) child;
                if (childProperties != null && !fieldsToKeep.Contains(childProperties.Name))
                {
                    Console.WriteLine($"Removing field: {child}");
                    child.Remove();
                }
            }
        }

        static string[] Fields => new []{
            "DwellingDataId",
            "DwellingAddress",
            "UValue",
            "RenewPrimaryEnergy3"
        };

        static string Input2 => @"
        {
          ""fort"": {
            ""vowel"": {
              ""particular"": ""clean"",
              ""yellow"": ""definition"",
              ""consonant"": ""elephant"",
              ""command"": {
                ""particles"": 1242932292,
                ""article"": true,
                ""social"": ""hearing"",
                ""general"": 584112402.4074564,
                ""drew"": ""start"",
                ""heavy"": false
              },
              ""wheat"": 1776728927,
              ""careful"": ""mouth""
            },
            ""arm"": ""beautiful"",
            ""widely"": ""passage"",
            ""measure"": 1096561128.5130153,
            ""rabbit"": ""minute"",
            ""structure"": -354922457.58943415
          },
          ""funny"": ""salt"",
          ""copy"": 791908287,
          ""joined"": -2089955502,
          ""hope"": ""where"",
          ""making"": 824952109.1728516
        }
        ";

        static string Input => @"
            {
              ""DwellingData"": {
                ""DwellingDataId"": 123132,
                ""DEAPVersion"": ""3.2.1"",
                ""ResultId"": 10020,
                ""TypeOfRating"": ""E "",
                ""BERSubmitted"": true,
                ""IsExtension"": false,
                ""PurposeOfRatingID"": 4,
                ""PurposeOfRatingDescription"": null,
                ""ElectricityPEFactor"": 2.42,
                ""ElectricityCO2Factor"": 0.524,
                ""ThermalBridgingFactor"": 0.15,
                ""LowEnergyFixedLighting"": 100,
                ""ThermalMassCategory"": 4,
                ""SummerEffACH"": 0,
                ""SummerMeanExtTemp"": 15,
                ""Mandatory"": true,
                ""Published"": false,
                ""ProjectData"": {
                  ""DwellingTypeID"": 5,
                  ""MPRNReference"": ""123123"",
                  ""MultiDwellingMPRN"": ""False"",
                  ""PlanningRef"": null,
                  ""BERNumber"": ""101007474"",
                  ""YourRef"": ""91"",
                 ""DateOfPlans"": ""5/15/2013 12:00:00 AM"",
                  ""DateOfConstruction"": ""5/15/2004 12:00:00 AM"",
                  ""DateOfAssessment"": ""5/15/2013 12:00:00 AM"",
                  ""DateOfIssue"": ""6/19/2013 3:17:00 PM"",
                  ""DateValidUntil"": ""6/19/2023 12:00:00 AM"",
                  ""ProjectComment"": null,
                  ""ClientName"": ""Mark Warner"",
                  ""ClientPhone"": null,
                  ""ClientEmail"": null,
                  ""AssessorName"": null,
                  ""AssessorRegNo"": ""100025"",
                  ""DevelopmentName"": null,
                  ""DeveloperName"": null,
                  ""TGDLEdition"": ""0"",
                  ""SharedBERNumber"": null,
                  ""DwellingAddress"": {
                    ""AddressLine1"": ""APT 4 GREGG COURT"",
                    ""AddressLine2"": ""KILMEAGUE"",
                    ""AddressLine3"": ""NAAS"",
                    ""County"": ""Co. Kildare"",
                    ""PostCode"": null
                  },
                  ""ClientAddress"": {
                    ""AddressLine1"": ""Apt 4 GREGG COURT"",
                    ""AddressLine2"": ""KILMEAGUE"",
                    ""AddressLine3"": ""NAAS"",
                    ""County"": ""34"",
                    ""PostCode"": null
                  }
                },
                ""DimensionDetails"": {
                  ""GroundFloorArea"": 37.44,
                  ""GroundFloorHeight"": 2.87,
                  ""FirstFloorArea"": 0,
                  ""FirstFloorHeight"": 0,
                  ""SecondFloorArea"": 0,
                  ""SecondFloorHeight"": 0,
                  ""ThirdFloorArea"": 0,
                  ""ThirdFloorHeight"": 0,
                  ""RoomInRoofArea"": 0,
                  ""RoomInRoofHeight"": 0,
                  ""NoOfStoreys"": 1,
                  ""LivingAreaPercent"": 49.011752136752,
                  ""LivingArea"": 18.35
                },
                ""VentilationDetails"": {
                  ""NoOfChimneys"": 0,
                  ""NoOfOpenFlues"": 0,
                  ""NoOfFansAndVents"": 2,
                  ""NoOfFluelessCombustionRoomHeaters"": 0,
                  ""DraftLobby"": true,
                  ""VentilationMethod"": 1,
                  ""FanPowerManuDeclaredValue"": 0,
                  ""HeatExchangerEff"": 0,
                  ""VentManufacturer"": null,
                  ""VentModel"": null,
                  ""NoOfWetRoomsComment"": null,
                  ""PermeabilityTest"": false,
                  ""StructureType"": 5,
                  ""SuspendedWoodenFloor"": 1,
                  ""PercentageDraughtStripped"": 100,
                  ""PermeabilityTestResult"": 0,
                  ""NoOfSidesSheltered"": 3
                },
                ""StructuralDetails"": {
                  ""FloorTypes"": [],
                  ""RoofTypes"": [
                    {
                      ""ID"": 105473,
                      ""Description"": null,
                      ""UValue"": 0.13,
                      ""Area"": 37.44,
                      ""AgeBandId"": ""9"",
                      ""DefaultUValue"": ""0.13"",
                      ""InsulationThicknessId"": 10,
                      ""RoofTypeId"": 2
                    }
                  ],
                  ""WallTypes"": [
                    {
                      ""ID"": 170597,
                      ""Description"": ""400mm thick of unknown build"",
                      ""UValue"": 0.55,
                      ""Area"": 41.43,
                      ""AgeBandId"": ""9"",
                      ""DefaultUValue"": ""0.55"",
                      ""IsSemiExposed"": false,
                      ""WallTypeId"": 10,
                      ""IsRelevantForTGDLComplyCheck"": false
                    },
                    {
                      ""ID"": 170598,
                      ""Description"": null,
                      ""UValue"": 0.451,
                      ""Area"": 19.03,
                      ""AgeBandId"": ""9"",
                      ""DefaultUValue"": ""0.45082"",
                      ""IsSemiExposed"": true,
                      ""WallTypeId"": 10,
                      ""IsRelevantForTGDLComplyCheck"": false
                    }
                  ],
                  ""DoorTypes"": [
                    {
                      ""ID"": 95996,
                      ""Description"": ""D1"",
                      ""UValue"": 3,
                      ""Area"": 1.8,
                      ""AgeBandId"": ""9"",
                      ""DefaultUValue"": ""3"",
                      ""NumberOfDoors"": 1
                    }
                  ],
                  ""WindowData"": [
                    {
                      ""ID"": 0,
                      ""WindowRef"": 1,
                      ""Description"": ""W1"",
                      ""OrientationID"": 8,
                      ""Area"": 1.03,
                      ""Uvalue"": 2.2,
                      ""AdjustedUvalue"": 2.022,
                      ""GlazingTypeID"": 4,
                      ""SolarTransmittance"": 0.72,
                      ""LightTransmittance"": 0,
                      ""ManufacturerModel"": null,
                      ""EffColectingArea"": ""0.359750160000"",
                      ""FrameFactorID"": 1,
                      ""GapID"": 2,
                      ""OvershadingFactorID"": 2,
                      ""ShadingFactorIDBlinds"": 1,
                      ""ShadingFactorIDOverhangs"": 999,
                      ""OverhangType"": ""NA"",
                      ""RoofWindow"": false,
                      ""UserDefinedUvalue"": false,
                      ""ShadingFactorBlinds"": 0,
                      ""UserDefinedFrameFactor"": 0.7
                    },
                    {
                      ""ID"": 0,
                      ""WindowRef"": 2,
                      ""Description"": ""W2"",
                      ""OrientationID"": 8,
                      ""Area"": 1.03,
                      ""Uvalue"": 2.2,
                      ""AdjustedUvalue"": 2.022,
                      ""GlazingTypeID"": 4,
                      ""SolarTransmittance"": 0.72,
                      ""LightTransmittance"": 0,
                      ""ManufacturerModel"": null,
                      ""EffColectingArea"": ""0.359750160000"",
                      ""FrameFactorID"": 1,
                      ""GapID"": 2,
                      ""OvershadingFactorID"": 2,
                      ""ShadingFactorIDBlinds"": 1,
                      ""ShadingFactorIDOverhangs"": 999,
                      ""OverhangType"": ""NA"",
                      ""RoofWindow"": false,
                      ""UserDefinedUvalue"": false,
                      ""ShadingFactorBlinds"": 0,
                      ""UserDefinedFrameFactor"": 0.7
                    },
                    {
                      ""ID"": 0,
                      ""WindowRef"": 3,
                      ""Description"": ""W3"",
                      ""OrientationID"": 4,
                      ""Area"": 1,
                      ""Uvalue"": 2.2,
                      ""AdjustedUvalue"": 2.022,
                      ""GlazingTypeID"": 4,
                      ""SolarTransmittance"": 0.72,
                      ""LightTransmittance"": 0,
                      ""ManufacturerModel"": null,
                      ""EffColectingArea"": ""0.349272000000"",
                      ""FrameFactorID"": 1,
                      ""GapID"": 2,
                      ""OvershadingFactorID"": 2,
                      ""ShadingFactorIDBlinds"": 1,
                      ""ShadingFactorIDOverhangs"": 999,
                      ""OverhangType"": ""NA"",
                      ""RoofWindow"": false,
                      ""UserDefinedUvalue"": false,
                      ""ShadingFactorBlinds"": 0,
                      ""UserDefinedFrameFactor"": 0.7
                    },
                    {
                      ""ID"": 0,
                      ""WindowRef"": 4,
                      ""Description"": ""W4"",
                      ""OrientationID"": 4,
                      ""Area"": 1.02,
                      ""Uvalue"": 2.2,
                      ""AdjustedUvalue"": 2.022,
                      ""GlazingTypeID"": 4,
                      ""SolarTransmittance"": 0.72,
                      ""LightTransmittance"": 0,
                      ""ManufacturerModel"": null,
                      ""EffColectingArea"": ""0.356257440000"",
                      ""FrameFactorID"": 1,
                      ""GapID"": 2,
                      ""OvershadingFactorID"": 2,
                      ""ShadingFactorIDBlinds"": 1,
                      ""ShadingFactorIDOverhangs"": 999,
                      ""OverhangType"": ""NA"",
                      ""RoofWindow"": false,
                      ""UserDefinedUvalue"": false,
                      ""ShadingFactorBlinds"": 0,
                      ""UserDefinedFrameFactor"": 0.7
                    }
                  ]
                },
                ""WaterHeatingDetails"": {
                  ""DistributionLosses"": true,
                  ""StorageLosses"": true,
                  ""ManuLossFactorAvail"": false,
                  ""SolarHotWaterHeating"": false,
                  ""ElecImmersionInSummer"": false,
                  ""CombiBoiler"": false,
                  ""CombiBoilerTypeID"": 1,
                  ""KeepHotFacilityTypeID"": 1,
                  ""WaterStorageVolume"": 84,
                  ""WaterManuNameModel"": null,
                  ""DeclaredLossFactor"": 0,
                  ""TempFactorUnadj"": 0.6,
                  ""TempFactorMultiplier"": 1,
                  ""LookupTable2Values"": ""Type of water storage: Cylinder, immersion main water heater&#xA;"",
                  ""IsHotWaterStorageIndoors"": true,
                  ""InsulationType"": 1,
                  ""InsulationThickness"": 30,
                  ""PrimaryCircuitLossID"": 2,
                  ""CombiBoilerAddLoss"": 0,
                  ""ElecConsumpKeepHot"": 0,
                  ""SWHDetails"": null
                },
                ""SpaceHeatingDetails"": {
                  ""TempAdjustment"": 0,
                  ""HeatSystemControlCat"": 3,
                  ""HeatSystemResponseCat"": 2,
                  ""NoCentralHeatingPumps"": 0,
                  ""CHBoilerThermostatControlled"": false,
                  ""NoOilBoilerHeatingPumps"": 0,
                  ""OBBoilerThermostatControlled"": false,
                  ""OBPumpInsideDwelling"": false,
                  ""CentHeatPumpInsideDwelling"": true,
                  ""TotalElecConsumption"": 0,
                  ""NoGasBoilerHeatingPumps"": 0,
                  ""WarmAirHeatingSystem"": false,
                  ""UndergroundHeating"": false,
                  ""GroundFloorUValue"": 0
                },
                ""HeatingSchemeDetails"": {
                  ""IndividualSchemeDetails"": {
                    ""HSMainSystemEfficiency"": 100,
                    ""HSManuName"": null,
                    ""HSManuModel"": null,
                    ""HSSupplManuName"": null,
                    ""HSSupplManuModel"": null,
                    ""WaterMainManuName"": null,
                    ""WaterMainManuModel"": null,
                    ""HSEffAdjFactor"": 1,
                    ""HSSupplHeatFraction"": 0.1,
                    ""HSSupplSystemEff"": 100,
                    ""WHMainSystemEff"": 100,
                    ""WHEffAdjFactor"": 1,
                    ""MainSHFuel"": 28,
                    ""SupplSHFuel"": 28,
                    ""MainWHFuel"": 28,
                    ""SupplWHFuel"": 1,
                    ""CHPMainHeatFraction"": 0,
                    ""CHPThermalEff"": 0,
                    ""CHPElectricalEff"": 0,
                    ""CHPFuelTypeID"": 1,
                    ""SHRenewableResources"": 2,
                    ""WHRenewableResources"": 2,
                    ""RenewableTechnology"": [
                      {
                        ""ID"": 232683,
                        ""EnergyTypeId"": 1,
                        ""EnerProdComment"": null,
                        ""EnerProdDelivered"": 0,
                        ""PartLTotalContribution"": 0,
                        ""EnerProdConvFactor"": 0,
                        ""EnerProdEmissionFactor"": 0,
                        ""EnerConsumedComment"": null,
                        ""EnerConsumedDelivered"": 0,
                        ""EnerConsumedConvFactor"": 0,
                        ""EnerConsumedEmissionFactor"": 0
                      },
                      {
                        ""ID"": 232684,
                        ""EnergyTypeId"": 1,
                        ""EnerProdComment"": null,
                        ""EnerProdDelivered"": 0,
                        ""PartLTotalContribution"": 0,
                        ""EnerProdConvFactor"": 0,
                        ""EnerProdEmissionFactor"": 0,
                        ""EnerConsumedComment"": null,
                        ""EnerConsumedDelivered"": 0,
                        ""EnerConsumedConvFactor"": 0,
                        ""EnerConsumedEmissionFactor"": 0
                      },
                      {
                        ""ID"": 232685,
                        ""EnergyTypeId"": 1,
                        ""EnerProdComment"": null,
                        ""EnerProdDelivered"": 0,
                        ""PartLTotalContribution"": 0,
                        ""EnerProdConvFactor"": 0,
                        ""EnerProdEmissionFactor"": 0,
                        ""EnerConsumedComment"": null,
                        ""EnerConsumedDelivered"": 0,
                        ""EnerConsumedConvFactor"": 0,
                        ""EnerConsumedEmissionFactor"": 0
                      }
                    ]
                  },
                  ""GroupSchemeDetails"": null
                },
                ""ResultsDetails"": {
                  ""EnergyRating"": ""E2"",
                  ""EnergyValue"": 341.38,
                  ""CDERValue"": 73.92,
                  ""MPCDERValue"": 87.36,
                  ""EnergyDetailsData"": [
                    {
                      ""SHMainPrimaryEnergy"": 7406.56,
                      ""SHSecPrimaryEnergy"": 822.95,
                      ""WHMainPrimaryEnergy"": 4093.26,
                      ""WHSecPrimaryEnergy"": 0,
                      ""PumpsFansPrimaryEnergy"": 0,
                      ""LightingPrimaryEnergy"": 458.54,
                      ""CHPInputPrimaryEnergy"": 0,
                      ""CHPElecOutputPrimaryEnergy"": 0,
                      ""RenewPrimaryEnergy1"": 0,
                      ""RenewPrimaryEnergy2"": 0,
                      ""RenewPrimaryEnergy3"": 0
                    }
                  ]
                }
              }
            }
        ";
    }

}
