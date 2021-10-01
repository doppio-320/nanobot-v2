using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Net;
using System.IO;

namespace NanoBot_V2.Services
{
    public static class GraphicsGenerator
    {
        /*public static Bitmap GenerateUserCard(string _avatarURL, string _name, int _rank, int _xp)
        {            
            Bitmap bmp = new Bitmap(1200, 860);

            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                Pen debugLines = new Pen(Color.Magenta, 2);
                debugLines.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;

                //White BG
                gfx.FillRectangle(Brushes.White, new RectangleF(0, 0, bmp.Width, bmp.Height));

                //Upper red box
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), new RectangleF(0, 0, bmp.Width, 320));

                //Upper red box
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), new RectangleF(0, 790, bmp.Width, 70));

                //TEXT: HELLO
                string text1Content = "HELLO";
                var text1Font = new Font("Helvetica", 125, FontStyle.Bold);
                var text1Size = gfx.MeasureString(text1Content, text1Font);
                var text1Rect = new RectangleF((bmp.Width / 2) - (text1Size.Width / 2), 0, text1Size.Width, text1Size.Height);
                gfx.DrawString(text1Content, text1Font, Brushes.White, text1Rect);
                //Text debug wireframe
                //gfx.DrawRectangle(debugLines, Rectangle.Round(text1Rect));

                //TEXT: my name is
                string text2Content = "my name is";
                var text2Font = new Font("Helvetica", 45);
                var text2Size = gfx.MeasureString(text2Content, text2Font);
                var text2Rect = new RectangleF((bmp.Width / 2) - (text2Size.Width / 2), text1Size.Height, text2Size.Width, text2Size.Height);
                gfx.DrawString(text2Content, text2Font, Brushes.White, text2Rect);
                //Text debug wireframe
                //gfx.DrawRectangle(debugLines, Rectangle.Round(text2Rect));

                //PFP                
                var pfpBgRect = new RectangleF(20, 375, 370, 370);
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), pfpBgRect); //BG
                using(WebClient webCl = new WebClient())
                {
                    Stream stream = webCl.OpenRead(_avatarURL);
                    Bitmap pfpBmp = new Bitmap(stream);

                    gfx.DrawImage(pfpBmp, new RectangleF(30, 385, 350, 350));
                }

                //TEXT: USERNAME
                var text3InitFontSize = 100;
                string text3Content = _name.ToUpper();
                var text3Font = new Font("Helvetica", text3InitFontSize, FontStyle.Bold);
                var text3Size = gfx.MeasureString(text3Content, text3Font);
                var text3Rect = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f, 375, text3Size.Width, text3Size.Height);
                while(text3Size.Width > bmp.Width - (pfpBgRect.Width + pfpBgRect.X + 10f) - 20f)
                {
                    text3InitFontSize--;
                    text3Font = new Font("Helvetica", text3InitFontSize, FontStyle.Bold);
                    text3Size = gfx.MeasureString(text3Content, text3Font);
                    text3Rect = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f, 375, text3Size.Width, text3Size.Height);
                }
                gfx.DrawString(text3Content, text3Font, Brushes.Black, text3Rect);
                //Text debug wireframe
                //gfx.DrawRectangle(debugLines, Rectangle.Round(text3Rect));

                //TEXT: Rank 69
                string text4Content = $"Rank {_rank}";
                var text4Font = new Font("Helvetica", 50);
                var text4Size = gfx.MeasureString(text4Content, text4Font);
                var text4Rect = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f, text3Rect.Height + text3Rect.Y, text4Size.Width, text4Size.Height);
                gfx.DrawString(text4Content, text4Font, Brushes.Black, text4Rect);
                //Text debug wireframe
                //gfx.DrawRectangle(debugLines, Rectangle.Round(text4Rect));

                //Rank bar bg                                                
                var rankBarBgRec = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f, text4Rect.Height + text4Rect.Y, bmp.Width - (pfpBgRect.Width + pfpBgRect.X + 10f) - 20f, 80f);
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(30, 30, 30)), rankBarBgRec);
                //Rank bar fill
                var fillMargin = 10f;
                var rankBarFillRec = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f + fillMargin, rankBarBgRec.Y + fillMargin, (_xp / (float)UserRank.RequiredXP(_rank))*(bmp.Width - (pfpBgRect.Width + pfpBgRect.X + 10f + fillMargin) - (20f + fillMargin)), rankBarBgRec.Height - (fillMargin * 2f));
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), rankBarFillRec);

                //TEXT: 6900xp remaining
                string text5Content = $"{UserRank.RequiredXP(_rank) - _xp}xp left until Rank {_rank + 1}";
                var text5Font = new Font("Helvetica", 20);
                var text5Size = gfx.MeasureString(text5Content, text5Font);                
                var text5Rect = new RectangleF(rankBarBgRec.X + ((rankBarBgRec.Width / 2) - (text5Size.Width / 2)), rankBarBgRec.Y + ((rankBarBgRec.Height / 2) - (text5Size.Height/2)), text5Size.Width, text5Size.Height);
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), text5Rect);
                gfx.DrawString(text5Content, text5Font, Brushes.White, text5Rect);                
                //Red border                
                Pen redBorder = new Pen(Color.FromArgb(30, 30, 30), 3);
                redBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                gfx.DrawRectangle(redBorder, Rectangle.Round(text5Rect));
                //Text debug wireframe
                //gfx.DrawRectangle(debugLines, Rectangle.Round(text5Rect));
            }
            return bmp;
        }*/

        public static Bitmap GenerateUserCard(ulong _guildID, ulong _userID)
        {
            Bitmap bmp = new Bitmap(1200, 860);

            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                Pen debugLines = new Pen(Color.Magenta, 2);
                debugLines.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;

                //White BG
                gfx.FillRectangle(Brushes.White, new RectangleF(0, 0, bmp.Width, bmp.Height));

                //Upper red box
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), new RectangleF(0, 0, bmp.Width, 320));

                //Upper red box
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), new RectangleF(0, 790, bmp.Width, 70));

                //TEXT: HELLO
                string text1Content = "HELLO";
                var text1Font = new Font("Helvetica", 125, FontStyle.Bold);
                var text1Size = gfx.MeasureString(text1Content, text1Font);
                var text1Rect = new RectangleF((bmp.Width / 2) - (text1Size.Width / 2), 0, text1Size.Width, text1Size.Height);
                gfx.DrawString(text1Content, text1Font, Brushes.White, text1Rect);
                //Text debug wireframe
                //gfx.DrawRectangle(debugLines, Rectangle.Round(text1Rect));

                //TEXT: my name is
                string text2Content = "my name is";
                var text2Font = new Font("Helvetica", 45);
                var text2Size = gfx.MeasureString(text2Content, text2Font);
                var text2Rect = new RectangleF((bmp.Width / 2) - (text2Size.Width / 2), text1Size.Height, text2Size.Width, text2Size.Height);
                gfx.DrawString(text2Content, text2Font, Brushes.White, text2Rect);
                //Text debug wireframe
                //gfx.DrawRectangle(debugLines, Rectangle.Round(text2Rect));

                //PFP                
                var pfpBgRect = new RectangleF(20, 375, 370, 370);
                gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), pfpBgRect); //BG
                using (WebClient webCl = new WebClient())
                {
                    Stream stream = webCl.OpenRead(UserCacheServices.GetUserCache(_userID).pfpURL);
                    Bitmap pfpBmp = new Bitmap(stream);

                    gfx.DrawImage(pfpBmp, new RectangleF(30, 385, 350, 350));
                }

                //TEXT: USERNAME
                var text3InitFontSize = 100;
                string text3Content = UserCacheServices.GetUserCache(_userID).userName.ToUpper();
                var text3Font = new Font("Helvetica", text3InitFontSize, FontStyle.Bold);
                var text3Size = gfx.MeasureString(text3Content, text3Font);
                var text3Rect = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f, 375, text3Size.Width, text3Size.Height);
                while (text3Size.Width > bmp.Width - (pfpBgRect.Width + pfpBgRect.X + 10f) - 20f)
                {
                    text3InitFontSize--;
                    text3Font = new Font("Helvetica", text3InitFontSize, FontStyle.Bold);
                    text3Size = gfx.MeasureString(text3Content, text3Font);
                    text3Rect = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f, 375, text3Size.Width, text3Size.Height);
                }
                gfx.DrawString(text3Content, text3Font, Brushes.Black, text3Rect);
                //Text debug wireframe
                //gfx.DrawRectangle(debugLines, Rectangle.Round(text3Rect));

                var userRank = GuildServices.GetGuildData(_guildID).GetUserData(_userID).rank;

                if (GuildServices.GetGuildData(_guildID).GetUserData(_userID).miscData.hasCustomCardQuote)
                {
                    //TEXT: "QUOTE"
                    string text6Content = $"\"{GuildServices.GetGuildData(_guildID).GetUserData(_userID).miscData.customCardQuote}\"";
                    var text6Font = new Font("Helvetica", 25);
                    var text6Size = gfx.MeasureString(text6Content, text6Font);
                    var text6Rect = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f, text3Rect.Height + text3Rect.Y, text6Size.Width, text6Size.Height);
                    gfx.DrawString(text6Content, text6Font, Brushes.Black, text6Rect);
                    //Text debug wireframe
                    //gfx.DrawRectangle(debugLines, Rectangle.Round(text6Rect));

                    //TEXT: Rank 69
                    string text4Content = $"Rank {userRank.currentRank}";
                    var text4Font = new Font("Helvetica", 50);
                    var text4Size = gfx.MeasureString(text4Content, text4Font);
                    var text4Rect = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f, text6Rect.Height + text6Rect.Y, text4Size.Width, text4Size.Height);
                    gfx.DrawString(text4Content, text4Font, Brushes.Black, text4Rect);
                    //Text debug wireframe
                    //gfx.DrawRectangle(debugLines, Rectangle.Round(text4Rect));

                    //Rank bar bg                                                
                    var rankBarBgRec = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f, text4Rect.Height + text4Rect.Y, bmp.Width - (pfpBgRect.Width + pfpBgRect.X + 10f) - 20f, 80f);
                    gfx.FillRectangle(new SolidBrush(Color.FromArgb(30, 30, 30)), rankBarBgRec);
                    //Rank bar fill
                    var fillMargin = 10f;
                    var rankBarFillRec = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f + fillMargin, rankBarBgRec.Y + fillMargin, (userRank.currentXP / (float)UserRank.RequiredXP(userRank.currentRank)) * (bmp.Width - (pfpBgRect.Width + pfpBgRect.X + 10f + fillMargin) - (20f + fillMargin)), rankBarBgRec.Height - (fillMargin * 2f));
                    gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), rankBarFillRec);

                    //TEXT: 6900xp remaining
                    string text5Content = $"{UserRank.RequiredXP(userRank.currentRank) - userRank.currentXP}xp left until Rank {userRank.currentRank + 1}";
                    var text5Font = new Font("Helvetica", 20);
                    var text5Size = gfx.MeasureString(text5Content, text5Font);
                    var text5Rect = new RectangleF(rankBarBgRec.X + ((rankBarBgRec.Width / 2) - (text5Size.Width / 2)), rankBarBgRec.Y + ((rankBarBgRec.Height / 2) - (text5Size.Height / 2)), text5Size.Width, text5Size.Height);
                    gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), text5Rect);
                    gfx.DrawString(text5Content, text5Font, Brushes.White, text5Rect);
                    //Red border                
                    Pen redBorder = new Pen(Color.FromArgb(30, 30, 30), 3);
                    redBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                    gfx.DrawRectangle(redBorder, Rectangle.Round(text5Rect));
                    //Text debug wireframe
                    //gfx.DrawRectangle(debugLines, Rectangle.Round(text5Rect));
                }
                else
                {
                    //TEXT: Rank 69
                    string text4Content = $"Rank {userRank.currentRank}";
                    var text4Font = new Font("Helvetica", 50);
                    var text4Size = gfx.MeasureString(text4Content, text4Font);
                    var text4Rect = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f, text3Rect.Height + text3Rect.Y, text4Size.Width, text4Size.Height);
                    gfx.DrawString(text4Content, text4Font, Brushes.Black, text4Rect);
                    //Text debug wireframe
                    //gfx.DrawRectangle(debugLines, Rectangle.Round(text4Rect));

                    //Rank bar bg                                                
                    var rankBarBgRec = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f, text4Rect.Height + text4Rect.Y, bmp.Width - (pfpBgRect.Width + pfpBgRect.X + 10f) - 20f, 80f);
                    gfx.FillRectangle(new SolidBrush(Color.FromArgb(30, 30, 30)), rankBarBgRec);
                    //Rank bar fill
                    var fillMargin = 10f;
                    var rankBarFillRec = new RectangleF(pfpBgRect.Width + pfpBgRect.X + 10f + fillMargin, rankBarBgRec.Y + fillMargin, (userRank.currentXP / (float)UserRank.RequiredXP(userRank.currentRank)) * (bmp.Width - (pfpBgRect.Width + pfpBgRect.X + 10f + fillMargin) - (20f + fillMargin)), rankBarBgRec.Height - (fillMargin * 2f));
                    gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), rankBarFillRec);

                    //TEXT: 6900xp remaining
                    string text5Content = $"{UserRank.RequiredXP(userRank.currentRank) - userRank.currentXP}xp left until Rank {userRank.currentRank + 1}";
                    var text5Font = new Font("Helvetica", 20);
                    var text5Size = gfx.MeasureString(text5Content, text5Font);
                    var text5Rect = new RectangleF(rankBarBgRec.X + ((rankBarBgRec.Width / 2) - (text5Size.Width / 2)), rankBarBgRec.Y + ((rankBarBgRec.Height / 2) - (text5Size.Height / 2)), text5Size.Width, text5Size.Height);
                    gfx.FillRectangle(new SolidBrush(Color.FromArgb(239, 61, 51)), text5Rect);
                    gfx.DrawString(text5Content, text5Font, Brushes.White, text5Rect);
                    //Red border                
                    Pen redBorder = new Pen(Color.FromArgb(30, 30, 30), 3);
                    redBorder.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                    gfx.DrawRectangle(redBorder, Rectangle.Round(text5Rect));
                    //Text debug wireframe
                    //gfx.DrawRectangle(debugLines, Rectangle.Round(text5Rect));
                }
            }
            return bmp;
        }
    }
}
