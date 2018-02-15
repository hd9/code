public class MyController : Controller
{

  public void DownloadReport(string id)
  {
      var rptLines = new List<CsvLine>();
      var count = 0;

      // load your data from the db...
      // example: using RavenDB

      using (var session = store.OpenSession())
      {
          var results = session.Query<BatchRow>("BatchIndex").ToList();
          rptLines = results.ConvertAll(bl => new CsvLine(bl.RefId, bl.Name, bl.Description /*, etc */ ));
      }

      // init StringWriter, StringBuilder
      var sb = new StringBuilder();
      using (var sw = new StringWriter(sb))
      {
          // init CsvWriter
          var csv = new CsvWriter(sw);

          // write all rptLines records to my StringBuilder
          csv.WriteRecords(rptLines);
      }

      // respond with data
      Response.ContentType = "application/csv";
      Response.AddHeader("content-disposition", @"attachment;filename=""export.csv""");   //necessary to return a 'filename' to the user
      Response.Write(sb.ToString());
      Response.End();
  }
 
}
