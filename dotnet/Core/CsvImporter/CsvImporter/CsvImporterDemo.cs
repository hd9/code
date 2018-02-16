using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsvImporter
{
    public class CsvImporterDemo
    {

        #region Attributes
        private List<CsvLine> lines = new List<CsvLine>();
        #endregion


        public void Import(string filename)
        {
            try
            {
                using (var fs = new StreamReader(filename))
                {
                    // I just need this one line to load the records from the file in my List<CsvLine>
                    lines = new CsvHelper.CsvReader(fs).GetRecords<CsvLine>().ToList();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            PrintLines();
        }

        private void PrintLines()
        {
            Console.WriteLine(" - Import done: {0} lines imported!\r\n", lines.Count);
            Console.WriteLine(" - Showing the 1st three (3) rows:");

            // I know, I'm doing an 'extra' ToList there. It's just to make it a one-liner =)
            lines.Take(3).ToList().ForEach(l => Console.WriteLine("   - {0}", l));
        }
    }
}
