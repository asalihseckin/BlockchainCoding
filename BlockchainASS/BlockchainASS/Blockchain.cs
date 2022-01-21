using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainASSS
{
    public class Blockchain
    {
        public IList<Transaction> PendingTransactions = new List<Transaction>();
        public IList<Block> Chain { get; set; }
        public int difficulty { get; set; } = 2;
        public int Reward { get; set; } = 1;
        public Blockchain()
        {
            InitializeChain();
            AddgenesisBlock();
        }
        public void InitializeChain()
        {
            Chain = new List<Block>();
        }
        public Block CreateGenesisBlock()
        {
            Block block = new Block(DateTime.Now, null, PendingTransactions);
            block.Mine(difficulty);
            PendingTransactions = new List<Transaction>();

            return block;
        }
        //Doğuş bloğu
        public void AddgenesisBlock()
        {
            Chain.Add(CreateGenesisBlock());
        }
        public Block GetLastestBlock()
        {           
            return Chain[Chain.Count - 1];
        }
        public void AddBlock(Block block)
        {
            Block lastestblock = GetLastestBlock();
            block.Index=lastestblock.Index +1;
            block.PreviousHash=lastestblock.Hash;
            block.Hash=block.CalculateHash();
            block.Mine(this.difficulty);
            Chain.Add(block);
        }
        public void CreateTransaction(Transaction transaction)
        {
            PendingTransactions.Add(transaction);
        }

        public void ProcessPendingTransactions(string minerAdress)
        {
            CreateTransaction(new Transaction(null, minerAdress, Reward));
            Block block = new Block(DateTime.Now, GetLastestBlock().Hash, PendingTransactions);
            AddBlock(block);
            PendingTransactions = new List<Transaction>();
        }
        public bool IsValid()
        {
            /* Şuanki blok ve bir önceki blokların hash değerlerini karşılaştırır 
             * ve bloğun geçerliliğini kontrol eder */
            for (int i=1; i<Chain.Count; i++)
            {
                Block currentBlock = Chain[i];
                Block previousBlock = Chain[i-1];
                if (currentBlock.Hash!=currentBlock.CalculateHash())
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
        public int GetBalance(string adress)
        {
            int balance = 0;
            for (int i = 1; i < Chain.Count; i++)
            {
                for(int j = 0; j < Chain[i].Transactions.Count; j++)
                {
                    var transaction = Chain[i].Transactions[j];
                    if (transaction.FromAdress == adress)
                    {
                        balance -= transaction.Amount;
                    }
                    if (transaction.ToAddress == adress)
                    {
                        balance += transaction.Amount;
                    }
                }
            }
            return balance;
        }
    }
}
