using System.Linq;
using CommandLine;
using CommandLine.Text;

namespace NasaHGTReader
{
    public class Options
    {
        private string[] RecognizedFileFormats = new string[]
        {
            "png",
        };

        private bool setFormat = true;

        private string outputFilename;
        private string format;

        [Value(0, Required=true)]
        public string InpuFilename { get; set; }

        [Value(1, Required = true)]
        public string OutputFilename
        {
            get { return outputFilename; }
            set
            {
                var dot = value.LastIndexOf('.');
                
                if (dot == -1)
                {
                    this.outputFilename = value;
                }
                else
                {
                    string checkFormat = value.Substring(dot, value.Length - dot);

                    if (this.RecognizedFileFormats.Contains(checkFormat))
                    {
                        this.outputFilename = value;
                        this.setFormat = false;
                        this.Format = checkFormat;
                    }
                    else
                    {
                        this.outputFilename = value;
                    }
                }
            }
        }

        [Option('f', "format", DefaultValue="png", HelpText="Choose the image format", Required=false)]
        public string Format
        {
            get { return format; }
            set 
            {
                if (setFormat)
                {
                    this.format = value;
                    outputFilename = outputFilename + "." + value;
                }
            }
        }

        [Option('n', "normalize", DefaultValue="", HelpText="Normalize data", Required=false)]
        public bool Normalize { get; set; }
    }
}
