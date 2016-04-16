using CommandLine;
using CommandLine.Text;

namespace GruntWurk {
    public class CommonOptions {
        [Option("verbose", DefaultValue = false, HelpText = "Displays detailed information messages while processing.")]
        public bool Verbose { get; set; }

        [Option("debug", DefaultValue = false, HelpText = "Displays even more detailed debug messages while processing.")]
        public bool Debug { get; set; }

        [HelpOption]
        public string GetUsage() {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

    }
}
