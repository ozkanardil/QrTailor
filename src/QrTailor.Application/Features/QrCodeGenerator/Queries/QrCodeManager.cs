﻿using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QrTailor.Application.Features.QrCodeGenerator.Queries
{
    internal class QrCodeManager : AbstractQRCode, IDisposable
    {
        public QrCodeManager()
        {
        }

        public QrCodeManager(QRCodeData data)
            : base(data)
        {
        }

        public Bitmap GetGraphic(int pixelsPerModule)
        {
            return GetGraphic(pixelsPerModule, Color.Black, Color.White, drawQuietZones: true);
        }

        public Bitmap GetGraphic(int pixelsPerModule, string darkColorHtmlHex, string lightColorHtmlHex, bool drawQuietZones = true)
        {
            return GetGraphic(pixelsPerModule, ColorTranslator.FromHtml(darkColorHtmlHex), ColorTranslator.FromHtml(lightColorHtmlHex), drawQuietZones);
        }

        public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, bool drawQuietZones = true)
        {
            int num = (base.QrCodeData.ModuleMatrix.Count - (!drawQuietZones ? 8 : 0)) * pixelsPerModule;
            int num2 = !drawQuietZones ? 4 * pixelsPerModule : 0;
            Bitmap bitmap = new Bitmap(num, num);
            using Graphics graphics = Graphics.FromImage(bitmap);
            using SolidBrush brush2 = new SolidBrush(lightColor);
            using SolidBrush brush = new SolidBrush(darkColor);
            for (int i = 0; i < num + num2; i += pixelsPerModule)
            {
                for (int j = 0; j < num + num2; j += pixelsPerModule)
                {
                    if (base.QrCodeData.ModuleMatrix[(j + pixelsPerModule) / pixelsPerModule - 1][(i + pixelsPerModule) / pixelsPerModule - 1])
                    {
                        graphics.FillRectangle(brush, new Rectangle(i - num2, j - num2, pixelsPerModule, pixelsPerModule));
                    }
                    else
                    {
                        graphics.FillRectangle(brush2, new Rectangle(i - num2, j - num2, pixelsPerModule, pixelsPerModule));
                    }
                }
            }

            graphics.Save();
            return bitmap;
        }

        public Bitmap GetGraphic(int pixelsPerModule, Color darkColor, Color lightColor, Bitmap icon = null, int iconSizePercent = 15, int iconBorderWidth = 0, bool drawQuietZones = true, Color? iconBackgroundColor = null)
        {
            int num = (base.QrCodeData.ModuleMatrix.Count - (!drawQuietZones ? 8 : 0)) * pixelsPerModule;
            int num2 = !drawQuietZones ? 4 * pixelsPerModule : 0;
            Bitmap bitmap = new Bitmap(num, num, PixelFormat.Format32bppArgb);
            using Graphics graphics = Graphics.FromImage(bitmap);
            using SolidBrush solidBrush2 = new SolidBrush(lightColor);
            using SolidBrush solidBrush = new SolidBrush(darkColor);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.Clear(lightColor);
            bool flag = icon != null && iconSizePercent > 0 && iconSizePercent <= 100;
            for (int i = 0; i < num + num2; i += pixelsPerModule)
            {
                for (int j = 0; j < num + num2; j += pixelsPerModule)
                {
                    SolidBrush brush = base.QrCodeData.ModuleMatrix[(j + pixelsPerModule) / pixelsPerModule - 1][(i + pixelsPerModule) / pixelsPerModule - 1] ? solidBrush : solidBrush2;
                    graphics.FillRectangle(brush, new Rectangle(i - num2, j - num2, pixelsPerModule, pixelsPerModule));
                }
            }

            if (flag)
            {
                float num3 = (float)(iconSizePercent * bitmap.Width) / 100f;
                float num4 = flag ? num3 * (float)icon.Height / (float)icon.Width : 0f;
                float num5 = ((float)bitmap.Width - num3) / 2f;
                float num6 = ((float)bitmap.Height - num4) / 2f;
                RectangleF rect = new RectangleF(num5 - iconBorderWidth, num6 - iconBorderWidth, num3 + iconBorderWidth * 2, num4 + iconBorderWidth * 2);
                RectangleF destRect = new RectangleF(num5, num6, num3, num4);
                SolidBrush brush2 = iconBackgroundColor.HasValue ? new SolidBrush(iconBackgroundColor.Value) : solidBrush2;
                if (iconBorderWidth > 0)
                {
                    using GraphicsPath path = CreateRoundedRectanglePath(rect, iconBorderWidth * 2);
                    graphics.FillPath(brush2, path);
                }

                graphics.DrawImage(icon, destRect, new RectangleF(0f, 0f, icon.Width, icon.Height), GraphicsUnit.Pixel);
            }

            graphics.Save();
            return bitmap;
        }

        internal GraphicsPath CreateRoundedRectanglePath(RectangleF rect, int cornerRadius)
        {
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180f, 90f);
            graphicsPath.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            graphicsPath.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270f, 90f);
            graphicsPath.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            graphicsPath.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0f, 90f);
            graphicsPath.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            graphicsPath.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90f, 90f);
            graphicsPath.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }
    }
}
