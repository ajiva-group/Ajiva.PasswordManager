// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using StoreManager;

const string name = "test123";
const string file = "test.tar";

Console.WriteLine("Hello, World!");
var store = new Store();
await store.AddEntryAsync(new StoreEntry(Guid.NewGuid(), name), CancellationToken.None);
var stream = await store.GetStreamAsync(CancellationToken.None);
stream.Position = 0;
//write to file
var fileStream = File.Create(file);
await stream.CopyToAsync(fileStream);
fileStream.Close();
Console.WriteLine("Done");

store = new Store();
fileStream = File.OpenRead(file);
await store.InitializeAsync(fileStream, CancellationToken.None);
fileStream.Close();

var entry = await store.GetEntryByNameAsync(name, CancellationToken.None);
Console.WriteLine(JsonSerializer.Serialize(entry));
