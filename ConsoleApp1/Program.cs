using BlockChainLib;

var myBlockchain = new Blockchain();
var server = new P2PServer();
server.Start(2000, myBlockchain);
//var client = new P2PClient(myBlockchain);
//client.Connect("ws://127.0.0.1:1000/Blockchain");
while (true)
{

}