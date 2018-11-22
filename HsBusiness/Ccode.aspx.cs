using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HsBusiness
{
    public partial class Ccode : System.Web.UI.Page
    {
        public static string codes="";
        protected void Page_Load(object sender, EventArgs e)
        {
            this.CreateCheckCodeImage(GetCodeNumberLetter(6));
        }
        /// <summary>
        /// 纯数字
        /// </summary>
        /// <returns></returns>
        private string RndNum()
        {
            int number;
            char code;
            string checkCode = String.Empty;
            var random = new Random();

            for (int i = 0; i < 6; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                //code = (char)('0' + (char)(number % 10));
                checkCode += code.ToString();
            }
            HttpCookie myCookie = new HttpCookie("yzm", checkCode);
            myCookie.HttpOnly = true;
            Response.Cookies.Add(myCookie);
            //codes = checkCode;
            return checkCode;
        }
        /// <summary>
        /// 获得随机数 数字＋字母
        /// </summary>
        /// <param name="CodeNumber"></param>
        /// <returns></returns>
        private  string GetCodeNumberLetter(int CodeNumber)
        {
            int number;
            char code;
            string checkCode = String.Empty;

            //System.Random random = new Random(unchecked((int)DateTime.Now.Ticks));			

            for (int i = 0; i < CodeNumber; i++)
            {
                System.Random random = new Random(GetRandomSeed());
                number = random.Next();

                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));

                checkCode += code.ToString();
            }
            Session["yzm"] = checkCode;
            //HttpCookie myCookie = new HttpCookie("yzm", checkCode);
            //myCookie.HttpOnly = true;
            //Response.Cookies.Add(myCookie);
            return checkCode;
            //return checkCode;
        }
        /// <summary>
        /// 加密随机数生成器 生成随机种子
        /// </summary>
        /// <returns></returns>

        public static int GetRandomSeed()
        {

            byte[] bytes = new byte[4];

            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();

            rng.GetBytes(bytes);

            return BitConverter.ToInt32(bytes, 0);

        }
        private void CreateCheckCodeImage(string checkCode)
        {
            if (checkCode == null || checkCode.Trim() == String.Empty)
                return;
            System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 12.5)), 20);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(image);
            try
            {
                //生成随机生成器 
                Random random = new Random();
                //清空图片背景色 
                g.Clear(Color.White);
                //画图片的背景噪音线 
                for (int i = 0; i < 5; i++)
                {
                    int x1 = random.Next(image.Width);
                    int x2 = random.Next(image.Width);
                    int y1 = random.Next(image.Height);
                    int y2 = random.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }

                Font font = new System.Drawing.Font("Arial", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(checkCode, font, brush, 2, 2);
                //画图片的前景噪音点 
                for (int i = 0; i < 30; i++)
                {
                    int x = random.Next(image.Width);
                    int y = random.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(random.Next()));
                }
                //画图片的边框线 
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                Response.ClearContent();
                Response.ContentType = "image/Gif";
                Response.BinaryWrite(ms.ToArray());
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
    }
}