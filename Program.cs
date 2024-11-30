var timer = new System.Diagnostics.Stopwatch();
timer.Start();


//Day1.Solve();


timer.Stop();
if (timer.ElapsedMilliseconds > 999)
    Console.WriteLine($"Time: {timer.Elapsed.TotalSeconds}s");
else
    Console.WriteLine($"Time: {timer.ElapsedMilliseconds}ms");