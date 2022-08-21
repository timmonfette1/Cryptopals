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

        private readonly Regex regex = new(@"Challenge[\d]+$");
        private readonly IDictionary<int, string> SetMapper = new Dictionary<int, string>
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

        private readonly IEnumerable<string> NamespaceFilter;

        public ChallengeRunner() : this(null)
        {

        }

        public ChallengeRunner(int? set)
        {
            if (!set.HasValue)
            {
                NamespaceFilter = new string[8]
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
                NamespaceFilter = new string[1] { SetMapper[set.Value] };
            }
        }

        public void Execute()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var query = from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.IsClass && NamespaceFilter.Contains(t.Namespace) && regex.Match(t.Name).Success
                        select t;

            for (var i = 1; i <= query.Count(); i++)
            {
                var c = query.ElementAt(i - 1);
                var type = assembly.GetType(c.FullName);
                var instance = Activator.CreateInstance(type, i) as BaseChallenge;
                instance.Execute();
                Console.WriteLine();
            }
        }
    }
}
