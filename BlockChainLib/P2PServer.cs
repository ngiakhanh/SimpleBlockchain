using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace BlockChainLib;

public class P2PServer : WebSocketBehavior
{
    bool chainSynced;
    WebSocketServer wss;
    private static Blockchain _blockchain;

    public void Start(int port, Blockchain blockchain)
    {
        _blockchain = blockchain;
        wss = new WebSocketServer($"ws://127.0.0.1:{port}");
        wss.AddWebSocketService<P2PServer>("/Blockchain");
        wss.Start();
        Console.WriteLine($"Started server at ws://127.0.0.1:{port}");
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        if (e.Data == "Hi Server")
        {
            Console.WriteLine(e.Data);
            Send("Hi Client");
        }
        else
        {
            Blockchain newChain = JsonSerializer.Deserialize<Blockchain>(e.Data);

            if (newChain is not null && newChain.IsValid() && newChain.Chain.Count > _blockchain.Chain.Count)
            {
                _blockchain = newChain;
            }

            if (!chainSynced)
            {
                Send(JsonSerializer.Serialize(_blockchain, new JsonSerializerOptions { WriteIndented = true }));
                chainSynced = true;
            }
        }
        Console.WriteLine(JsonSerializer.Serialize(_blockchain, new JsonSerializerOptions { WriteIndented = true }));
    }
}