﻿using System.Text;
using Cryptopals.DataContexts;

namespace Cryptopals.Challenges.Set2
{
    public class Challenge12 : BaseChallenge
    {
        private const string Unknown = "Um9sbGluJyBpbiBteSA1LjAKV2l0aCBteSByYWctdG9wIGRvd24gc28gbXkgaGFpciBjYW4gYmxvdwpUaGUgZ2lybGllcyBvbiBzdGFuZGJ5IHdhdmluZyBqdXN0IHRvIHNheSBoaQpEaWQgeW91IHN0b3A/IE5vLCBJIGp1c3QgZHJvdmUgYnkK";

        public Challenge12(int index) : base(index)
        {

        }

        public override void Execute()
        {
            var bytes = Convert.FromBase64String(Unknown);
            var oracle = new EncryptionOracleDataContext(bytes);
            var decrypted = oracle.DecryptUnknown();
            var result = Encoding.ASCII.GetString(decrypted);

            OutputResult(Answers.CHALLENGE_12, result);
        }
    }
}
