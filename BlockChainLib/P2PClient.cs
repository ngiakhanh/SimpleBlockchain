using System.Text.Json;
using WebSocketSharp;

namespace BlockChainLib;

public class P2PClient
{
    readonly IDictionary<string, WebSocket> wsDict = new Dictionary<string, WebSocket>();
    private static Blockchain _blockchain = new();

    public P2PClient(Blockchain blockchain)
    {
        _blockchain = blockchain;
    }

    public void Connect(string url)
    {
        if (!wsDict.ContainsKey(url))
        {
            WebSocket ws = new WebSocket(url);
            ws.OnMessage += (sender, e) =>
            {
                if (e.Data == "Hi Client")
                {
                    Console.WriteLine(e.Data);
                }
                else
                {
                    Blockchain newChain = JsonSerializer.Deserialize<Blockchain>(e.Data);
                    if (newChain is not null && newChain.IsValid() && newChain.Chain.Count > _blockchain.Chain.Count)
                    {
                        _blockchain = newChain;
                    }
                }
                Console.WriteLine(JsonSerializer.Serialize(_blockchain, new JsonSerializerOptions { WriteIndented = true }));
            };
            ws.Connect();
            ws.Send("Hi Server");
            ws.Send(JsonSerializer.Serialize(_blockchain, new JsonSerializerOptions { WriteIndented = true }));
            wsDict.Add(url, ws);
        }
    }

    public void Send(string url, string data)
    {
        foreach (var item in wsDict)
        {
            if (item.Key == url)
            {
                item.Value.Send(data);
            }
        }
    }

    public void Broadcast(string data)
    {
        foreach (var item in wsDict)
        {
            item.Value.Send(data);
        }
    }

    public IList<string> GetServers()
    {
        IList<string> servers = new List<string>();
        foreach (var item in wsDict)
        {
            servers.Add(item.Key);
        }
        return servers;
    }

    public void Close()
    {
        foreach (var item in wsDict)
        {
            item.Value.Close();
        }
    }
}