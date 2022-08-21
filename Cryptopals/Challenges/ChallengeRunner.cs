using System.Reflection;
using System.Text.RegularExpressions;

namespace Cryptopals.Challenges
{
    public class ChallengeRunner
    {
        private const string SET_1_NAMESPACE = "Cryptopals.Challenges.Set1";
        private const string SET_2_NAMESPACE = "Cryptopals.Challenges.Set2";
        private const string SET_3_NAMESPACE = "Cryptopals.Challenges.Set3";
        private const string SET_4_NAMESPACE = "Cryptopals.Challenges.Set4";
        private const string SET_5_NAMESPACE = "Cryptopals.Challenges.Set5";
        private const string SET_6_NAMESPACE = "Cryptopals.Challenges.Set6";
        private const string SET_7_NAMESPACE = "Cryptopals.Challenges.Set7";
        private const string SET_8_NAMESPACE = "Cryptopals.Challenges.Set8";

        private readonly Regex _challengeRegex = new(@"Challenge[\d]+$");
        private readonly Regex _numberRegex = new(@"\d+");
        private readonly IDictionary<int, string> _setMapper = new Dictionary<int, string>
        {
            { 1, SET_1_NAMESPACE },
            { 2, SET_2_NAMESPACE },
            { 3, SET_3_NAMESPACE },
            { 4, SET_4_NAMESPACE },
            { 5, SET_5_NAMESPACE },
            { 6, SET_6_NAMESPACE },
            { 7, SET_7_NAMESPACE },
            { 8, SET_8_NAMESPACE },
        };

        private readonly IEnumerable<string> _namespaceFilter;

        public ChallengeRunner() : this(null)
        {

        }

        public ChallengeRunner(int? set)
        {
            if (!set.HasValue)
            {
                _namespaceFilter = new string[8]
                {
                    SET_1_NAMESPACE,
                    SET_2_NAMESPACE,
                    SET_3_NAMESPACE,
                    SET_4_NAMESPACE,
                    SET_5_NAMESPACE,
                    SET_6_NAMESPACE,
                    SET_7_NAMESPACE,
                    SET_8_NAMESPACE,
                };
            }
            else
            {
                _namespaceFilter = new string[1] { _setMapper[set.Value] };
            }
        }

        public void Execute()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var query = from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.IsClass && _namespaceFilter.Contains(t.Namespace) && _challengeRegex.Match(t.Name).Success
                        select new
                        {
                            Type = t,
                            ChallengeNumber = Convert.ToInt32(string.Join("", _numberRegex.Matches(t.Name)))
                        };

            foreach (var type in query.OrderBy(x => x.ChallengeNumber))
            {
                var instance = Activator.CreateInstance(type.Type, type.ChallengeNumber) as BaseChallenge;
                instance.Execute();
                Console.WriteLine();
            }
        }
    }
}
