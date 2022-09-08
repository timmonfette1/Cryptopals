namespace Cryptopals.Challenges
{
    public abstract class BaseChallenge
    {
        protected readonly int _index;

        public BaseChallenge(int index)
        {
            _index = index;
        }

        public abstract bool Execute();

        protected virtual bool OutputResult(object answer, object result, bool skipAnswerPrint = true)
        {
            Console.WriteLine($"===== Challenge {_index} =====");

            if (!skipAnswerPrint)
            {
                Console.WriteLine($"Answer: {answer.ToString().Trim()}");
                Console.WriteLine($"Result: {result.ToString().Trim()}");
            }

            var success = string.Equals(answer.ToString(), result.ToString(), StringComparison.OrdinalIgnoreCase);

            Console.WriteLine($"Challenge Passed: {success}");

            return success;
        }

        protected virtual bool OutputResult(object[] answers, object[] results, bool skipAnswerPrint = true)
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
                    Console.WriteLine($"Answer: {answer.ToString().Trim()}");
                    Console.WriteLine($"Result: {result.ToString().Trim()}");
                }

                success = success && string.Equals(answer.ToString(), result.ToString(), StringComparison.OrdinalIgnoreCase);
            }

            Console.WriteLine($"Challenge Passed: {success}");

            return success;
        }
    }
}
