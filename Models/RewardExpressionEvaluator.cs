using System.Text;
using System.Text.RegularExpressions;

namespace ArchnemesisRecipies.Models
{
    public interface IExpressionEvaluator
    {
        EffectExpression GetExpression(string expression);
        static string MatchText { get; }
        string GetMatchText();
        Priority Priority { get; }
    }

    public interface IRewardsEvaluator : IExpressionEvaluator
    {
    }

    public interface IRarityEvaluator : IExpressionEvaluator
    {
    }

    public delegate string EffectExpression(string expression);

    public abstract class ExpressionEvaluator
    {
        protected Regex GetRegex(string matchText) => new(matchText, RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }

    public class DefaultExpressionEvaluator : IExpressionEvaluator
    {
        public Priority Priority => Priority.Low;

        public EffectExpression GetExpression(string expression)
        {
            return (s) => s;
        }

        public string GetMatchText() => string.Empty;
    }

    public class AllRewardsAreThisExpressionEvaluator : ExpressionEvaluator, IExpressionEvaluator, IRewardsEvaluator
    {
        public static string MatchText => @"all reward types are (.*)";
        public Priority Priority => Priority.Low;
        public string GetMatchText() => MatchText;
        public EffectExpression GetExpression(string expression)
        {
            var m = GetRegex(MatchText).Match(expression);
            if (m.Success)
            {
                return (s) => Regex.Replace(s, @"(\w+)", $"{m.Groups[1].Value.Replace(" ", "")}");
            }

            return null;
        }
    }

    public class RewardsAreDoubledExpressionEvaluator : ExpressionEvaluator, IExpressionEvaluator, IRewardsEvaluator
    {
        public Priority Priority => Priority.High;
        public static string MatchText => @"rewards are doubled";
        public string GetMatchText() => MatchText;
        public EffectExpression GetExpression(string expression)
        {
            var m = GetRegex(MatchText).Match(expression);
            if (m.Success)
            {
                return (s) => string.Join(" ", s, s);
            }

            return (s) => s;
        }
    }

    public class AdditionalRewardForEachRewardTypeExpressionEvaluator : ExpressionEvaluator, IExpressionEvaluator, IRewardsEvaluator
    {
        public static string MatchText => @"all reward types have an additional reward";
        public Priority Priority => Priority.High;
        public string GetMatchText() => MatchText;
        public EffectExpression GetExpression(string expression)
        {
            var m = GetRegex(MatchText).Match(expression);
            if (m.Success)
            {
                return (s) =>
                {
                    var groups = s.Split(" ").GroupBy(x => x);
                    var result = new List<string>();
                    foreach (var group in groups)
                    {
                        foreach (var str in Enumerable.Repeat(group.Key, group.Count() + 1))
                        {
                            result.Add(str);
                        }
                    }
                    return string.Join(" ", result);
                };
            }

            return (s) => s;
        }
    }

    public class RewardsRolledAdditionalTimesExpressionEvaluator : ExpressionEvaluator, IExpressionEvaluator, IRarityEvaluator
    {
        public Priority Priority => Priority.Low;
        public static string MatchText => @"Rewards are rolled (\d+) additional time(?:s)?, choosing the rarest result";
        public string GetMatchText() => MatchText;
        public EffectExpression GetExpression(string expression)
        {
            var r = GetRegex(MatchText);
            var m = r.Match(expression);
            if (m.Success)
            {
                return (s) =>
                {
                    var count = r.Match(s) is { Success: true } t ? Convert.ToInt32(t.Groups[1].Value) : 0;
                    count += Convert.ToInt32(m.Groups[1].Value);
                    return $"Rewards are rolled {count} additional time{(count > 1 ? "s" : "")}, choosing the rarest result";
                };
            }

            return (s) => s;
        }
    }

    public enum Priority
    {
        Low,
        High
    }

    public static class RewardExpressionEvaluators
    {
        public static Dictionary<string, IExpressionEvaluator> Evaluators = new()
        {
            { AllRewardsAreThisExpressionEvaluator.MatchText, new AllRewardsAreThisExpressionEvaluator() },
            { RewardsAreDoubledExpressionEvaluator.MatchText, new RewardsAreDoubledExpressionEvaluator() },
            { AdditionalRewardForEachRewardTypeExpressionEvaluator.MatchText, new AdditionalRewardForEachRewardTypeExpressionEvaluator() },
            { RewardsRolledAdditionalTimesExpressionEvaluator.MatchText, new RewardsRolledAdditionalTimesExpressionEvaluator() },
        };

        public static IExpressionEvaluator GetEvaluator(string effect)
        {
            foreach (var evaluator in Evaluators)
            {
                var r = new Regex(evaluator.Key, RegexOptions.IgnoreCase);
                if (r.IsMatch(effect))
                    return evaluator.Value;
            }
            return null;
        }
    }
}
