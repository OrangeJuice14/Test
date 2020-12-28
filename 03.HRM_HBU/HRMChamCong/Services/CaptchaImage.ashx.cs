using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing.Imaging;
using System.Web.SessionState;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using HRMChamCong.Helper;

namespace HRMChamCong.Services
{
    public class Captcha
    {
        //Default Constructor 
        public Captcha() { }
        //property
        public string Text { get; set; }
        public Bitmap Image { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        //Private variable
        private Random random = new Random();
        //Methods declaration
        public Captcha(string s, int Width, int Height)
        {
            this.Text = s;
            this.SetDimensions(Width, Height);
            this.GenerateImage();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                this.Image.Dispose();
        }
        private void SetDimensions(int Width, int Height)
        {
            if (Width <= 0)
                throw new ArgumentOutOfRangeException("Width", Width,
                    "Argument out of range, must be greater than zero.");
            if (Height <= 0)
                throw new ArgumentOutOfRangeException("Height", Height,
                    "Argument out of range, must be greater than zero.");
            this.Width = Width;
            this.Height = Height;
        }
        private void GenerateImage()
        {
            Bitmap bitmap = new Bitmap
              (this.Width, this.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            HatchBrush hatchBrush = new HatchBrush(HatchStyle.SmallConfetti,
                Color.LightGray, Color.White);
            g.FillRectangle(hatchBrush, rect);
            SizeF size;
            float fontSize = rect.Height + 1;
            Font font;

            do
            {
                fontSize--;
                font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold);
                size = g.MeasureString(this.Text, font);
            } while (size.Width > rect.Width);
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
            GraphicsPath path = new GraphicsPath();
            path.AddString(this.Text, font.FontFamily, (int)font.Style, Height - 10, rect, format);
            float v = 4F;
            PointF[] points =
            {
                new PointF(this.random.Next(rect.Width) / v, this.random.Next(rect.Height) / v),
                new PointF(rect.Width - this.random.Next(rect.Width) / v, 
                    this.random.Next(rect.Height) / v),
                new PointF(this.random.Next(rect.Width) / v, 
                    rect.Height - this.random.Next(rect.Height) / v),
                new PointF(rect.Width - this.random.Next(rect.Width) / v,
                    rect.Height - this.random.Next(rect.Height) / v)
            };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);
            hatchBrush = new HatchBrush(HatchStyle.Percent10, System.Drawing.ColorTranslator.FromHtml("#bf3f00"), System.Drawing.ColorTranslator.FromHtml("#bf3f00"));
            g.FillPath(hatchBrush, path);

            //g.DrawString(this.Text, font, Brushes.OrangeRed, rect);

            int m = Math.Max(rect.Width, rect.Height);
            for (int i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = this.random.Next(rect.Width);
                int y = this.random.Next(rect.Height);
                int w = this.random.Next(m / 50);
                int h = this.random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }
            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();
            this.Image = bitmap;
        }
    }
    public class CaptchaImage : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            // Create a random code and store it in the Session object.
            SessionHelper.Data(SessionKey.CaptchaImage,GenerateRandomCode());
            // Create a CAPTCHA image using the text stored in the Session object.
            Captcha captcha = new Captcha(HttpContext.Current.Session[SessionKey.CaptchaImage.ToString()].ToString(), 150, 50);
            // Change the response headers to output a JPEG image.
            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";
            // Write the image to the response stream in JPEG format.
            captcha.Image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
            // Dispose of the CAPTCHA image object.
            captcha.Dispose();


        }

        private string GenerateRandomCode()
        {
            Random r = new Random();
            string s = "";
            for (int j = 0; j < 4; j++)
            {
                int i = r.Next(2);
                int ch;
                switch (i)
                {
                    case 1:
                        ch = r.Next(0, 9);
                        s = s + ch.ToString();
                        break;
                    case 2:
                        ch = r.Next(65, 90);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    default:
                        ch = r.Next(65, 90);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                }
                r.NextDouble();
                r.Next(100, 1999);
            }
            return s;
        }


        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}