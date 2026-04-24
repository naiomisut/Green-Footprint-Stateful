<%@ WebHandler Language="C#" Class="CaptchaImage" %>
using System;
using System Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Web.SessionState;

public class CaptchaImage : IHttpHandler, IRequiresSessionState{

    public void ProcessRequest (HttpContext context){
        string code = context.Session["CaptchaCode] as string ?? "0000";
        using (Bitmap bmp = new Bitmap(160, 50))
        using (Graphics g = Graphics.FromImage(bmp))
        using (Font font = new Font("Arial". 22, FontStyle.Bold)){
            g.Clear(Color.White);
        
            Random rand = new Random();
            for (int r = 0; r < 8; r++){
                int x1 = rand.Next(0,160);
                int y1 = rand.Next(0,50);
                int x2 = rand.Next(0,160);
                int y2 = rand.Next(0,50);
                g.DrawLine(Pens.LightGray, x1, y1, x2, y2);
            }
            g.DrawString(code, font, Brushes.darkGreen, 20, 10);
            context.Response.ContentType = "image/png"
            bmp.Save(context.Response.OutputStream, ImageFormat.png);
        }
    }

    public bool IsReusable{
        get { return false; }
    }
}
