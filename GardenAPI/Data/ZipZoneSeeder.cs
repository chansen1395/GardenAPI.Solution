using GardenAPI.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;


namespace GardenAPI.Data
{
  public class ZipZoneSeeder
  {
    private readonly GardenAPIContext _db;
    private readonly IWebHostEnvironment _env;
    public ZipZoneSeeder(GardenAPIContext db, IWebHostEnvironment env)
    {
      _db = db;
      _env = env;
    }

    // Define file path to zipcode data, return raw json
    private string GetDataZipZones()
    {
      string rootPath = _env.ContentRootPath;
      string filePath = Path.GetFullPath(Path.Combine(rootPath, "Data", "zip_zone.json"));
      using (var r = new StreamReader(filePath))
      {
        string json = r.ReadToEnd();
        return json;
      }
    }

     // Grab data from zip_zone.json, format into object for the db
    public void Seed()
    {
      string data = GetDataZipZones();
      var items = JsonSerializer.Deserialize<List<Dictionary<string, int>>>(data);
      foreach (var item in items)
      {
        var s = new ZipZone(item["zipcode"], item["zone"]);
        _db.ZipZones.Add(s);
      }
      _db.SaveChanges();
    }
  }

}