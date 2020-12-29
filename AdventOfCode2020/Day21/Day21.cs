using AdventOfCode2020.Utility;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace AdventOfCode2020.Day21
{
    internal static class Day21
    {
        internal static (long partOne, string partTwo) Solve(IInputResolver input)
        {
            var data = input.AsEnumerable();
            var rows = new List<(List<string> Ing, List<string> Aller)>();
            foreach (var row in data)
            {
                var ingredients = row.Split('(')[0].Split(' ')[..^1];
                var allergens = row.Split('(')[1].Split(')')[0].Split(' ')[1..].Select(s => s.Trim(','));
                rows.Add((ingredients.ToList(), allergens.ToList()));
            }

            var distIng = new List<string>();
            foreach (var (Ing, Aller) in rows)
            {
                distIng.AddRange(Ing);
            }
            distIng = distIng.Distinct().ToList();
            var allergenIngredients = new List<Ingredient>();
            var safeIngredients = new List<string>();
            long ans1 = 0;

            foreach (var currentIngredient in distIng)
            {
                var allergensForThis = new List<string>();
                int occurrences = 0;
                foreach (var (Ing, Aller) in rows)
                {
                    if(Ing.Contains(currentIngredient))
                    {
                        allergensForThis.AddRange(Aller);
                        occurrences++;
                    }
                }
                allergensForThis = allergensForThis.Distinct().ToList();
                // There is a one to one map between an allergen and an ingredient.
                // So if we see the allergent on multiple rows, and an ingredient does not appear on all,
                // then we know that cannot be the source.

                // However, an ingredient can apear on rows with multiple allergens, and multiple rows with different allergens.
                // Since all allergens are not listed on each row, it can match one of the allergens on all rows it is present, and thus be valid.
                // From the example we have `sqjhc` are listed under (dairy, fish), (soy), and (fish).
                // Now, just because it is not listed under the next `dairy` instance, does not mean it is not possible,
                // since it is listed under all `fish` rows.
                var possibilities = allergensForThis.ToDictionary(d => d, _ => true);
                foreach (var currentAllergens in allergensForThis)
                {
                    foreach (var (rowsIngredient, rowsAllergens) in rows)
                    {
                        if (rowsAllergens.Contains(currentAllergens)
                            && !rowsIngredient.Contains(currentIngredient))
                        {
                            possibilities[currentAllergens] = false;
                            break;
                        }
                    }
                }
                if(!possibilities.Any(a => a.Value))
                {
                    ans1 += occurrences;
                    safeIngredients.Add(currentIngredient);
                }
                else
                {
                    var allergens = possibilities.Where(x => x.Value).Select(s => s.Key).ToList();
                    allergenIngredients.Add(new Ingredient(currentIngredient, allergens));
                }
            }

            // Part two
            // So too solve this, clear all known safe ingredients.
            var cleanedRows = new List<(List<string> Ing, List<string> Aller)>(rows.Count);
            foreach (var (Ing, Aller) in rows)
            {
                var ing = Ing.Where(x => !safeIngredients.Contains(x)).ToList();
                cleanedRows.Add((ing, Aller));
            }

            // We could find ingredients alone on a row, but that does not seem to be the case here (tested for it).
            // Instead we will look for ingredients that only have one possible allergen.
            var canonicalDangerousIngredients = new Dictionary<string, string>();
            while(true)
            {
                // Find them, and exclude them from the remaining results.
                var singleAllergen = allergenIngredients.First(f => f.PossibleAllergens.Count == 1);
                var allergenToRemove = singleAllergen.PossibleAllergens.Single();
                canonicalDangerousIngredients.Add(singleAllergen.Name, allergenToRemove);

                for (int i = 0; i < cleanedRows.Count;)
                {
                    var (Ing, Aller) = cleanedRows[i];
                    var cleanedIng = Ing.Where(x => x != singleAllergen.Name).ToList();
                    var cleanedAller = Aller.Where(x => x != allergenToRemove).ToList();
                    if(cleanedIng.Count == 0 || cleanedAller.Count == 0)
                    {
                        cleanedRows.RemoveAt(i);
                    }
                    else
                    {
                        cleanedRows[i] = (cleanedIng, cleanedAller);
                        i++;
                    }
                }

                // Done if no items left.
                allergenIngredients.Remove(singleAllergen);
                if (allergenIngredients.Count == 0)
                    break;

                // We must also clear other allergents list.
                foreach (var item in allergenIngredients)
                {
                    item.PossibleAllergens = item.PossibleAllergens.Where(x => x != allergenToRemove).ToList();
                }
            }
            var ans2 = string.Join(',', canonicalDangerousIngredients.OrderBy(o => o.Value).Select(s => s.Key));

            return (ans1, ans2);
        }
    }

    internal class Ingredient
    {
        public string Name { get; set; }
        public List<string> PossibleAllergens { get; set; }

        public Ingredient(string name, List<string> possibleAllergens)
        {
            Name = name;
            PossibleAllergens = possibleAllergens;
        }
    }

    public class Test2020Day21
    {
        [Theory]
        [MemberData(nameof(ExampleData))]
        public void SolveProblemExamples(string[] example, int expectedFirst, string expectedSecond)
        {
            var (partOne, partTwo) = Day21
                .Solve(new MockInputResolver(example));
            partOne.Should().Be(expectedFirst);
            partTwo.Should().Be(expectedSecond);
        }

        [Fact]
        public void SolveProblemInput()
        {
            var (partOne, partTwo) = Day21
                .Solve(new FileInputResolver(21));
            partOne.Should().Be(2098);
            partTwo.Should().Be("ppdplc,gkcplx,ktlh,msfmt,dqsbql,mvqkdj,ggsz,hbhsx");
        }

        public readonly static List<object[]> ExampleData = new List<object[]>
        {
            new object[] {
                new [] {
                    "mxmxvkd kfcds sqjhc nhms (contains dairy, fish)",
                    "trh fvjkl sbzzf mxmxvkd (contains dairy)",
                    "sqjhc fvjkl (contains soy)",
                    "sqjhc mxmxvkd sbzzf (contains fish)",
                }, 5, "mxmxvkd,sqjhc,fvjkl"
            }
        };
    }
}
