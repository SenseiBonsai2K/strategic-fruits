// using UnityEngine;
// using InfluxDB.Client;
// using InfluxDB.Client.Api.Domain;
// using InfluxDB.Client.Writes;
// using System;
//
// public class InfluxDBManager
// {
//     private static InfluxDBClient _client;
//
//     public InfluxDBManager()
//     {
//         var options = InfluxDBClientOptions.Builder
//             .CreateNew()
//             .Url("https://us-east-1-1.aws.cloud2.influxdata.com/")
//             .AuthenticateToken("INFLUXDB_TOKEN".ToCharArray())
//             .Org("Cris-and-Matt")
//             .Bucket("StrategicFruitsData")
//             .Build();
//
//         _client = InfluxDBClientFactory.Create(options);
//     }
//
//     public static void WritePointOnDatabase(int phase, int round, string matchup, string suit, int rank, float thinkingTime)
//     {
//         var point = PointData.Measurement("Video Game")
//             .Tag("Phase", phase.ToString())
//             .Tag("Round", round.ToString())
//             .Tag("Suit", suit)
//             .Field("Matchup", matchup)
//             .Field("Rank", rank)
//             .Field("Thinking Time", thinkingTime)
//             .Timestamp(DateTime.UtcNow, WritePrecision.Ns);
//
//         using var writeApi = _client.GetWriteApi();
//         writeApi.WritePoint(point);
//         Debug.Log($"Data saved: Phase {phase}, Round {round}, Matchup {matchup}, Suit {suit}, Rank {rank}, Thinking Time {thinkingTime}");
//     }
// }