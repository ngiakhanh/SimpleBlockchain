using BlockChainLib;

var myBlockchain = new Blockchain();
myBlockchain.CreateTransaction(new Transaction("test", "test2", 20));
myBlockchain.ProcessPendingTransactions("te");
myBlockchain.ProcessPendingTransactions("te");
//var server = new P2PServer();
//server.Start(1000, myBlockchain);
var client = new P2PClient(myBlockchain);
client.Connect("ws://127.0.0.1:2000/Blockchain");
while (true)
{

}
