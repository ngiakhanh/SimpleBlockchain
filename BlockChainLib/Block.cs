﻿using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BlockChainLib;

public class Block
{
    public int Index { get; set; }
    public DateTime TimeStamp { get; set; }
    public string PreviousHash { get; set; }
    public string Hash { get; set; }
    public int Nonce { get; set; }
    public IList<Transaction> Transactions { get; set; }

    public Block(DateTime timeStamp, string previousHash, IEnumerable<Transaction> transactions)
    {
        Index = 0;
        TimeStamp = timeStamp;
        PreviousHash = previousHash;
        Transactions = transactions.ToList();
        Hash = CalculateHash();
    }
    public string CalculateHash()
    {
        SHA256 sha256 = SHA256.Create();

        byte[] inputBytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonSerializer.Serialize(Transactions)}-{Nonce}");
        byte[] outputBytes = sha256.ComputeHash(inputBytes);

        return Convert.ToBase64String(outputBytes);
    }
    public void Mine(int difficulty)
    {
        var leadingZeros = new string('0', difficulty);
        while (Hash == null || Hash[..difficulty] != leadingZeros)
        {
            Nonce++;
            Hash = CalculateHash();
        }
    }
}