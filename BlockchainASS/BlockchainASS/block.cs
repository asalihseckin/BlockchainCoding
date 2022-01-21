using Newtonsoft.Json;
using System;

using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace BlockchainASSS
{
    public class Block
    {
        /*   Blok yapımızın içinde gerekli değişkenler   */
        public int Index { get; set; } 
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public string Hash { get; set; }

        public IList<Transaction> Transactions;
       
      //public string Data { get; set; }

        public int Nonce { get; set; } = 0;
        public Block(DateTime timeStamp, string preivousHash, IList<Transaction> transactions)
        {
            Index = 0;
            TimeStamp = timeStamp;
            PreviousHash = preivousHash;
            Transactions = transactions;

        }
        public string CalculateHash()
        {
            SHA256 sha256 = SHA256.Create();
            /*  TimeStamp, PreviousHash, Data yı çekip byte çevirecek ve inbytes değişkenimizin içine atar, 
             *  Ardından outbytes değişkenine inbytes değişkeninin sha256 uygulanır.
             *  outbytes değişkeni string e çevirilip geri döndürülür.
             */
            byte[] inbytes = Encoding.ASCII.GetBytes($"{TimeStamp}-{PreviousHash ?? ""}-{JsonConvert.SerializeObject(Transactions)}--{Nonce}");
            byte[] outbytes = sha256.ComputeHash(inbytes);
            return Convert.ToBase64String(outbytes);
        }
        public void Mine(int difficulty)
        {
            var leadingZeros = new string('0', difficulty);
            while (this.Hash==null || this.Hash.Substring(0, difficulty) != leadingZeros)
            {
                this.Nonce++;
                this.Hash= this.CalculateHash();
            }
        }
    }
}
