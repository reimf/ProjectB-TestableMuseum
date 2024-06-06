namespace Depot;

using System.Diagnostics;

public class FakeWorld : IWorld
{
    private DateTime? _now = null;
    private int _linesRead = 0;
    private readonly Dictionary<string, int> _filesTimesRead = new();
    private readonly Dictionary<string, List<string>> _previousFileVersions = new();

    // You can override these in the code block after the constructor
    public DateTime Now
    {
        get
        {
            if (_now is null)
                WriteDebugInfoToDebugConsole();
            return _now ?? throw new NullReferenceException();
        }
        set => _now = value;
    }
    public List<string> LinesWritten { get; set; } = null;
    public List<string> LinesToRead { private get; set; } = null;
    public Dictionary<string, string> Files { get; set; } = null;
    public bool IncludeLinesReadInLinesWritten { get; set; } = false;

    public void WriteLine(string line)
    {
        if (LinesWritten is null)
            WriteDebugInfoToDebugConsole();
        LinesWritten.Add(line);
    }

    public string ReadLine()
    {
        if (LinesToRead is null || _linesRead >= LinesToRead.Count)
            WriteDebugInfoToDebugConsole();
        string line = LinesToRead.ElementAt(_linesRead);
        _linesRead++;
        if (IncludeLinesReadInLinesWritten)
            WriteLine(line);
        return line;
    }

    public string ReadAllText(string path)
    {
        if (!Exists(path))
            WriteDebugInfoToDebugConsole();
        _filesTimesRead[path] = _filesTimesRead.GetValueOrDefault(path, 0) + 1;
        return Files[path];
    }

    public bool Exists(string path)
    {
        if (Files is null)
            WriteDebugInfoToDebugConsole();
        return Files.ContainsKey(path);
    }

    public void WriteAllText(string path, string content)
    {
        if (Files is null)
            WriteDebugInfoToDebugConsole();
        if (!_previousFileVersions.ContainsKey(path))
            _previousFileVersions[path] = new();
        _previousFileVersions[path].Add(Files[path]);
        Files[path] = content;
    }

    private void WriteDebugInfoAboutNowToDebugConsole()
    {
        Debug.WriteLine($"--- Now: {_now?.ToString("O") ?? "null"}");
    }

    private void WriteDebugInfoAboutLinesToReadToDebugConsole()
    {
        Debug.WriteLine($"--- LinesToRead ({_linesRead}/{LinesToRead.Count} lines read)");
        Debug.WriteLine(string.Join("\n", LinesToRead));
    }

    private void WriteDebugInfoAboutIncludeLinesReadInLinesWrittenToDebugConsole()
    {
        Debug.WriteLine($"--- IncludeLinesReadInLinesWritten: {IncludeLinesReadInLinesWritten}");
    }

    private void WriteDebugInfoAboutLinesWrittenToDebugConsole()
    {
        Debug.WriteLine($"--- LinesWritten ({LinesWritten.Count} lines)");
        Debug.WriteLine(string.Join("\n", LinesWritten));
    }

    private void WriteDebugInfoAboutFilesToDebugConsole()
    {
        if (Files.Count == 0)
        {
            Debug.WriteLine("--- Files (0 files)");
            return;
        }
        IEnumerable<(int, string, string)> numberedFiles = Files
            .Select((item, index) => (index, item.Key, item.Value));
        foreach ((int number, string path, string currentContent) in numberedFiles)
        {
            List<string> contents = _previousFileVersions.GetValueOrDefault(path, new());
            int timesRead = _filesTimesRead.GetValueOrDefault(path, 0);
            int timesWritten = contents.Count;
            contents.Add(currentContent);
            IEnumerable<(int, string)> versionedContents = contents.Select((item, index) => (index, item));
            foreach ((int version, string content) in versionedContents)
            {
                Debug.WriteLine($"--- Files[{path}] (file {number}/{Files.Count}, {timesRead}x read, {timesWritten}x written, version {version}/{timesWritten + 1})");
                Debug.WriteLine(content);
            }
        }
    }

    public void WriteDebugInfoToDebugConsole()
    {
        WriteDebugInfoAboutNowToDebugConsole();
        WriteDebugInfoAboutLinesToReadToDebugConsole();
        WriteDebugInfoAboutIncludeLinesReadInLinesWrittenToDebugConsole();
        WriteDebugInfoAboutLinesWrittenToDebugConsole();
        WriteDebugInfoAboutFilesToDebugConsole();
    }
}
