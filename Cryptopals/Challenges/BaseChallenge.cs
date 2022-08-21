namespace Cryptopals.Challenges
{
    public abstract class BaseChallenge
    {
        private readonly int _index;

        public BaseChallenge(int index)
        {
            _index = index;
        }

        public abstract void Execute();

        protected void OutputResult(string answer, string result, bool skipAnswerPrint = true)
        {
            Console.WriteLine($"===== Challenge {_index} =====");

            if (!skipAnswerPrint)
            {
                Console.WriteLine($"Answer: {answer.Trim()}");
                Console.WriteLine($"Result: {result.Trim()}");
            }

            Console.WriteLine($"Challenge Passed: {string.Equals(answer, result, StringComparison.OrdinalIgnoreCase)}");
        }

        protected void OutputResult(string[] answers, string[] results, bool skipAnswerPrint = true)
        {
            if (answers.Length != results.Length)
            {
                throw new ArgumentException("Different number of answers and results provided.");
            }

            bool success = true;

            Console.WriteLine($"===== Challenge {_index} =====");
            for (int i = 0; i < answers.Length; i++)
            {
                var answer = answers[i];
                var result = results[i];

                if (!skipAnswerPrint)
                {
                    Console.WriteLine($"Answer: {answer.Trim()}");
                    Console.WriteLine($"Result: {result.Trim()}");
                }

                success = success && string.Equals(answer, result, StringComparison.OrdinalIgnoreCase);
            }

            Console.WriteLine($"Challenge Passed: {success}");
        }
    }
}
