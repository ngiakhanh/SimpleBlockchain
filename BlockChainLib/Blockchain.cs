namespace BlockChainLib;

public class Blockchain
{
    public List<Block> Chain { get; private set; }

    public int Difficulty { get; } = 1;

    public List<Transaction> PendingTransactions { get; private set; }

    public int Reward { get; private set; } = 10;

    public Blockchain(int difficulty, int reward)
    {
        Difficulty = difficulty;
        Reward = reward;
        AddGenesisBlock();
    }

    public Blockchain()
    {
        AddGenesisBlock();
    }


    private Block CreateGenesisBlock()
    {
        return new Block(DateTime.Now, null, PendingTransactions);
    }

    private void AddGenesisBlock()
    {
        Chain = new();
        PendingTransactions = new();
        Chain.Add(CreateGenesisBlock());
    }

    private Block GetLatestBlock()
    {
        return Chain[^1];
    }

    public void AddBlock(Block block)
    {
        Block latestBlock = GetLatestBlock();
        block.Index = latestBlock.Index + 1;
        block.PreviousHash = latestBlock.Hash;
        block.Mine(Difficulty);
        Chain.Add(block);
    }
    public bool IsValid()
    {
        if (Chain.Count == 1)
        {
            if (Chain[0].Hash != Chain[0].CalculateHash())
            {
                return false;
            }

            if (Chain[0].PreviousHash != null)
            {
                return false;
            }
            return true;
        }
        for (int i = 1; i < Chain.Count; i++)
        {
            Block currentBlock = Chain[i];
            Block previousBlock = Chain[i - 1];

            if (currentBlock.Hash != currentBlock.CalculateHash())
            {
                return false;
            }

            if (currentBlock.PreviousHash != previousBlock.Hash)
            {
                return false;
            }
        }
        return true;
    }

    public void CreateTransactions(IList<Transaction> transactions)
    {
        PendingTransactions.AddRange(transactions);
    }

    public void CreateTransaction(Transaction transaction)
    {
        PendingTransactions.Add(transaction);
    }

    public void ProcessPendingTransactions(string minerAddress)
    {
        Block block = new Block(DateTime.Now, GetLatestBlock().Hash, PendingTransactions);
        AddBlock(block);

        PendingTransactions = new List<Transaction>();
        CreateTransaction(new Transaction(null, minerAddress, Reward));
    }
}