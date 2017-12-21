using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimespanLib;

namespace timespans
{
    class Program
    {
        static void Main(string[] args)
        {            
            // display application name and version in console title
            String appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Version appVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            String appFullName = String.Format("\n{0} v{1}.{2}", appName, appVersion.Major, appVersion.Minor);
            Console.Title = appFullName;


            // variables to hold values of input args
            bool showHelp = false;
            String iFileName = "";     // text input file name with path
            String oFileName = "";    // output file name including path
            String language = "en";       // language of input data (default 'en')

            var p = new Mono.Options.OptionSet() {
                { "i|in|input=", "name of input data {FILE}", v => { if (v != null) iFileName = v.Trim(); }},
                { "o|out|output=", "name of output {FILE}", v => { if (v != null) oFileName = v.Trim(); }},
                { "l|lang|language=", "language of input data (default=en) {STRING}", v => { if (v != null) language = v.Trim().ToLower(); }},
                { "h|?|help",  "show this message and exit", v => showHelp = v != null }
            };

            // validate input args
            try
            {
                p.Parse(args);
            }
            catch (Mono.Options.OptionException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Type {0} --help' for more information", appName);
                Console.WriteLine("Hit any key to exit");
                Console.ReadKey();    
                return;
            }                              
            
            
            try
            {
                DateTime started = DateTime.Now;
            
                // read values to be matched from the input file
                Console.WriteLine("{0} started {1}", appFullName, started.ToLongTimeString());
                Console.WriteLine("Reading from input file '{0}'", iFileName);
                IList<string> outputLines = new List<string>();
               
                // do the processing
                foreach (string line in System.IO.File.ReadLines(iFileName))
                {
                    IYearSpan result = YearSpan.Parse(line, language);
                    
                    Char delimiter = '\t'; // tab delimited output                   
                    outputLines.Add(String.Format("{1}{0}{2}{0}{3}",
                        delimiter,
                        result.label.Replace(delimiter, ' '),
                        result.min,
                        result.max
                    ));
                }                
                
                // Write how long the process took
                TimeSpan elapsed = DateTime.Now.Subtract(started);
                Console.WriteLine("Processed {0} records [time taken: {1:00}:{2:00}:{3:00}.{4:000}]",
                    outputLines.Count, 
                    (int)elapsed.TotalHours, 
                    elapsed.Minutes, 
                    elapsed.Seconds, 
                    elapsed.Milliseconds
                );

                // finally write the results to the (tab delimited) output file
                if(oFileName.Trim() == "") oFileName = iFileName.Trim() + ".out.txt";
                Console.WriteLine("Writing results to output file '{0}'", oFileName);
                System.IO.File.WriteAllLines(oFileName, outputLines, Encoding.UTF8);
                Console.WriteLine("Finished");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.Message);
            }

            // don't allow console to disappear automatically
            Console.WriteLine("Hit any key to exit");
            Console.ReadKey();            
        }
    }
}
