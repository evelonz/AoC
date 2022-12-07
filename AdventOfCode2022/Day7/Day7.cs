using AdventOfCode2022.Utility;
using FluentAssertions;
using Xunit;

namespace AdventOfCode2022.Day7;

internal class Folder
{
    public override string ToString()
    {
        return $"- {Name} (dir)";
    }

    public IList<Folder> SubFolders { get; set; }
    public string Name { get; set; }
    public Folder? Parent { get; set; }

    public Folder(string name, Folder? parent)
    {
        Name = name;
        SubFolders = new List<Folder>();
        Parent = parent;
    }

    public virtual int GetSize() => SubFolders.Sum(s => s.GetSize());
}

internal class File : Folder
{
    public File(string name, Folder parent, int size) : base(name, parent)
    {
        Size = size;
    }

    public override string ToString()
    {
        return $"- {Name} (file, size={Size})";
    }
    public int Size { get; set; }

    public override int GetSize()
    {
        return Size;
    }
}

internal static class Day7
{
    const int FolderSizeLimit = 100_000;
    const int TotalSpace = 70_000_000;
    const int SpaceNeeded = 30_000_000;

    internal static (int partOne, int partTwo) Solve(IInputResolver input)
    {
        var root = new Folder("/", null);
        var currentFolder = root;
        foreach (var item in input.AsEnumerable())
        {
            switch (item)
            {
                case string s when s.StartsWith("$ cd"):
                    var name = s[5..];
                    currentFolder = name == ".."
                        ? currentFolder.Parent
                        : name == "/" ? root
                        : currentFolder.SubFolders.First(f => f.Name == name);
                    break;
                case string s when s.StartsWith("$ ls"):
                    break;
                case string s when s.StartsWith("dir"):
                    var folderName = s[4..];
                    currentFolder.SubFolders.Add(new Folder(folderName, currentFolder));
                    break;
                default:
                    var index = item.IndexOf(' ');
                    var size = item[..index];
                    var fileName = item[(index+1)..];
                    currentFolder.SubFolders.Add(new File(fileName, currentFolder, int.Parse(size)));
                    break;
            }
        }

        var spaceLeftToFree = SpaceNeeded - (TotalSpace - root.GetSize());
        var (partOne, partTwo) = Print(root, "", spaceLeftToFree);
        return (partOne, partTwo);
    }

    private static (int canBeFreed, int smalestToDelete) Print(
        Folder folder,
        string prefix,
        int spaceNeeded)
    {
        Console.WriteLine(prefix + folder);
        var subPrefix = "  " + prefix;
        var sumOfValidSizes = 0;
        var bestOptionForDelete = int.MaxValue;
        foreach (var subfolder in folder.SubFolders)
        {
            switch (subfolder)
            {
                case File file:
                    Console.WriteLine(subPrefix + file);
                    break;
                default:
                    var (canBeFreed, smalestToDelete) =  Print(subfolder, subPrefix, spaceNeeded);
                    sumOfValidSizes += canBeFreed;
                    bestOptionForDelete = smalestToDelete < bestOptionForDelete ? smalestToDelete : bestOptionForDelete;
                    break;
            }
        }
        var a = folder.GetSize();
        var b = a <= FolderSizeLimit ? sumOfValidSizes + a : sumOfValidSizes;

        var canDelete = spaceNeeded - a < 0;
        bestOptionForDelete =
            bestOptionForDelete < a
            ? bestOptionForDelete
            : canDelete ? a : int.MaxValue;

        return (b, bestOptionForDelete);
    }
}

public class Test2022Day7
{
    [Fact]
    public void FirstProblemExamples()
    {
        Day7
            .Solve(new MockInputResolver(new string[] {
                "$ cd /",
                "$ ls",
                "dir a",
                "14848514 b.txt",
                "8504156 c.dat",
                "dir d",
                "$ cd a",
                "$ ls",
                "dir e",
                "29116 f",
                "2557 g",
                "62596 h.lst",
                "$ cd e",
                "$ ls",
                "584 i",
                "$ cd ..",
                "$ cd ..",
                "$ cd d",
                "$ ls",
                "4060174 j",
                "8033020 d.log",
                "5626152 d.ext",
                "7214296 k"
            }))
            .Should().Be((95437, 24933642));
    }

    [Fact]
    public void ProblemInput()
    {
        var (partOne, partTwo) = Day7
            .Solve(new FileInputResolver(7));

        partOne.Should().Be(1582412);
        partTwo.Should().Be(3696336);
    }
}
