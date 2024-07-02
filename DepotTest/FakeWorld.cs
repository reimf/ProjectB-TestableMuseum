namespace Depot;

using System.Diagnostics;

public class FakeWorld : IWorld
{
    private DateTime? _now = null;
    private int _timeTimesUsed = 0;
    private int _linesRead = 0;
    private readonly Dictionary<string, int> _filesTimesRead = new();
    private readonly Dictionary<string, List<string>> _previousFileVersions = new();

    // You can override the following properties in the code block after the constructor
    public DateTime Now
    {
        get
        {
            if (_now is null)
                WriteDebugInfoToDebugConsole();
            _timeTimesUsed++;
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

    public void WriteDebugInfoToDebugConsole()
    {
        Debug.WriteLine($"--- Now ({_timeTimesUsed} times used)");
        Debug.WriteLine(_now == null ? "null" : _now?.ToString("O"));

        Debug.WriteLine($"--- LinesToRead ({_linesRead}/{LinesToRead.Count} lines read)");
        Debug.WriteLine(LinesToRead.Glue("/n"));

        Debug.WriteLine($"--- IncludeLinesReadInLinesWritten: {IncludeLinesReadInLinesWritten}");

        Debug.WriteLine($"--- LinesWritten ({LinesWritten.Count} lines)");
        Debug.WriteLine(LinesWritten.Glue("\n"));

        if (Files.Count == 0)
            Debug.WriteLine("--- Files (0 files)");
        else
        {
            IEnumerable<(int index, KeyValuePair<string, string> item)> numberedFiles = Files
                .Select((item, index) => (index, item));
            foreach ((int number, (string path, string currentContent)) in numberedFiles)
            {
                List<string> contents = _previousFileVersions.GetValueOrDefault(path, new());
                int timesRead = _filesTimesRead.GetValueOrDefault(path, 0);
                int timesWritten = contents.Count;
                contents.Add(currentContent);
                IEnumerable<(int, string)> versionedContents = contents.Select((content, version) => (version, content));
                foreach ((int version, string content) in versionedContents)
                {
                    Debug.WriteLine($"--- Files[{path}] (file {number}/{Files.Count}, {timesRead}x read, {timesWritten}x written, version {version}/{timesWritten + 1})");
                    Debug.WriteLine(content);
                }
            }
        }
    }

    public void WriteWarningsToDebugConsole()
    {
        List<string> warnings = new();

        if (_now is not null && _timeTimesUsed == 0)
            warnings.Add("WARNING: Now is set, but never used");
        
        if (LinesToRead is not null && _linesRead < LinesToRead.Count)
        {
            string stringNotRead = LinesToRead.Skip(_linesRead).Select(line => $"\"{line}\"").Glue(" and ");
            string isOrAre = _linesRead + 1 == LinesToRead.Count ? "is" : "are";
            warnings.Add($"WARNING: LinesToRead is set, but {stringNotRead} {isOrAre} not read");
        }

        if (IncludeLinesReadInLinesWritten && _linesRead == 0)
            warnings.Add("WARNING: IncludeLinesReadInLinesWritten is set to true, but no lines were read");

        if (Files is not null)
            foreach ((string path, _) in Files)
            {
                int timesRead = _filesTimesRead.GetValueOrDefault(path, 0);
                if (timesRead == 0)
                    warnings.Add($"WARNING: File {path} is set, but was never read");
            }

        if (warnings.Count == 0)
            Debug.WriteLine("No warnings");
        else
            foreach (string warning in warnings)
                Debug.WriteLine(warnings);
    }
}
