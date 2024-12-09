using System;

namespace AOC2024
{
    public class DiskSpace
    {
        public int Index { get; set; }
        public int Size { get; set; }
        public int ID { get; set; }

        public DiskSpace(int index, int size, int id = 0)
        {
            Index = index;
            Size = size;
            ID = id;
        }
    }
    public class Day9
    {
        public static void Solve()
        {
            int[] diskMap = PuzzleInput.Text.Select(c => int.Parse(c.ToString())).ToArray();
            SolvePart2(diskMap);
        }

        private static void SolvePart1(int[] diskMap)
        {
            var fileSystem = new List<int?>();

            for (int i = 0; i < diskMap.Length; i += 2)
            {
                fileSystem.AddRange(Enumerable.Repeat((int?)i/2, diskMap[i]));
                if (i+1 < diskMap.Length)
                    fileSystem.AddRange(Enumerable.Repeat((int?)null, diskMap[i+1]));
            }

            var lastNumber = fileSystem.FindLastIndex(f => f.HasValue);
            var firstEmpty = fileSystem.FindIndex(f => !f.HasValue);
            while (lastNumber > firstEmpty)
            {
                fileSystem[firstEmpty] = fileSystem[lastNumber];
                fileSystem[lastNumber] = null;
                lastNumber = fileSystem.FindLastIndex(f => f.HasValue);
                firstEmpty = fileSystem.FindIndex(f => !f.HasValue);
            }

            var checkSum = 0L;
            for (int i = 0; fileSystem[i].HasValue; i++)
                checkSum += i * fileSystem[i].Value;
            
            Console.WriteLine(checkSum);
        }

        private static void SolvePart2(int[] diskMap)
        {
            var allFiles = new List<DiskSpace>();
            var allEmpties = new List<DiskSpace>();
            var index = 0;

            for (int i = 0; i < diskMap.Length; i += 2)
            {
                allFiles.Add(new (index, diskMap[i], i/2));
                index += diskMap[i];
                if (i+1 < diskMap.Length)
                {
                    allEmpties.Add(new (index, diskMap[i+1]));
                    index += diskMap[i+1];
                }
            }

            for (var i = allFiles.Count - 1; i > 0; i--)
            {
                var lastNumber = allFiles[i];
                var firstEmpty = allEmpties.FirstOrDefault(e => e.Size >= lastNumber.Size);
                if (firstEmpty == null || firstEmpty.Index > lastNumber.Index)
                    continue;

                var lastIndex = lastNumber.Index;
                lastNumber.Index = firstEmpty.Index;
                firstEmpty.Index = lastIndex;
                var leftover = firstEmpty.Size - lastNumber.Size;
                if (leftover > 0)
                {
                    firstEmpty.Size -= leftover;
                    allEmpties.Add(new (lastNumber.Index + lastNumber.Size, leftover));
                }
                allEmpties = allEmpties.OrderBy(e => e.Index).ToList();
            }
            allFiles = allFiles.OrderBy(f => f.Index).ToList();

            var checkSum = 0L;
            for (int i = 0; i < allFiles.Count; i++)
                for (int j = 0; j < allFiles[i].Size; j++)
                    checkSum += (allFiles[i].Index + j) * allFiles[i].ID;
            
            Console.WriteLine(checkSum);
        }
    }
}
