
using System.Collections;
using System.Collections.Generic;
using DocoptNet;

namespace ProjReferenceAnalyzer.Console
{
    // Generated class for Main.usage.txt
	public class MainArgs
	{
		public const string USAGE = @"Example usage for T4 Docopt.NET

Usage:
  projrefs find <path> [-o <output-path>] [--json] [--include-assembly-references]

  <path> can be a folder, a solution file or a project file.

Options:
 -o           Short switch.
 -s=<arg>     Short option with arg.
 --long=ARG   Long option with arg.
 --switch     Long switch.
";
	    private readonly IDictionary<string, ValueObject> _args;
		public MainArgs(ICollection<string> argv, bool help = true,
                                                      object version = null, bool optionsFirst = false, bool exit = false)
		{
			_args = new Docopt().Apply(USAGE, argv, help, version, optionsFirst, exit);
		}

        public IDictionary<string, ValueObject> Args
        {
            get { return _args; }
        }

		public bool CmdFind { get { return _args["find"].IsTrue; } }
		public string ArgPath  { get { return null == _args["<path>"] ? null : _args["<path>"].ToString(); } }
		public bool OptO { get { return _args["-o"].IsTrue; } }
		public string ArgOutputPath  { get { return null == _args["<output-path>"] ? null : _args["<output-path>"].ToString(); } }
		public bool OptJson { get { return _args["--json"].IsTrue; } }
		public bool OptIncludeAssemblyReferences { get { return _args["--include-assembly-references"].IsTrue; } }
	
	}

	
}

