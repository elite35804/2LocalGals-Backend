public class DownloadHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        string fileUrl = context.Request.QueryString["file"];
        if (!string.IsNullOrEmpty(fileUrl))
        {
            string fileName = System.IO.Path.GetFileName(fileUrl);
            string filePath = context.Server.MapPath(fileUrl);

            if (System.IO.File.Exists(filePath))
            {
                context.Response.ContentType = "image/jpeg";
                context.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
                context.Response.TransmitFile(filePath);
                context.Response.End();
            }
            else
            {
                context.Response.Write("<script>alert('File not found.');</script>");
            }
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}