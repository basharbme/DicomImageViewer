﻿//Copyright © 2018 studio451.ru
//Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
//3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.
//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//E-mail: info@studio451.ru
//URL: https://studio451.ru/dicomimageviewer

using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DicomImageViewer.Dicom
{
    class ArrowAnnotation : Annotation
    {
        public ArrowAnnotation()
        {
        }

        public int Width;

        public override void Draw(Graphics g, convertToDestinationDelegate convertToDestination, double PixelScale)
        {
            Point startPoint = convertToDestination(StartPoint);
            Point endPoint = convertToDestination(EndPoint);

            int lenght = (int)Math.Sqrt(Math.Pow(endPoint.X - startPoint.X,2) + Math.Pow(endPoint.Y - startPoint.Y,2));

            Width = 5;

            if (lenght > 50 && lenght < 150)
            {
                Width = lenght / 10;
            }
            else if (lenght > 100)
            {
                Width = 15;
            }

            Pen pen = new Pen(this.Color, Width);
            pen.EndCap = LineCap.ArrowAnchor;            
            
            g.DrawLine(pen, startPoint,endPoint);

            path = new GraphicsPath();
            path.StartFigure();
            path.AddLine(startPoint.X, startPoint.Y - Width, startPoint.X + lenght, startPoint.Y - Width);
            path.AddLine(startPoint.X + lenght, startPoint.Y - Width, startPoint.X + lenght, startPoint.Y + Width);
            path.AddLine(startPoint.X + lenght, startPoint.Y + Width, startPoint.X, startPoint.Y + Width);
                        
            float cos = (float)(endPoint.X - startPoint.X) / (float)lenght;
            

            float arc = (float)(Math.Acos(cos) * (float)180 / Math.PI);
            if (endPoint.Y - startPoint.Y < 0)
            {
                arc = 360 - arc;
            }
            System.Drawing.Drawing2D.Matrix matrix = new System.Drawing.Drawing2D.Matrix();
            matrix.RotateAt(arc, startPoint);
            path.Transform(matrix);
            path.CloseFigure();
        }

        public override void MouseMove(Point point, Point offset)
        {
            Point offsetEnd = new Point();
            offsetEnd.X = this.StartPoint.X - point.X - offset.X;
            offsetEnd.Y = this.StartPoint.Y - point.Y - offset.Y;

            this.StartPoint.X = point.X + offset.X;
            this.StartPoint.Y = point.Y + offset.Y;

            this.EndPoint.X -= offsetEnd.X;
            this.EndPoint.Y -= offsetEnd.Y;           
           
        }

        public override void MouseUp(Point point)
        {

        }        
    }
}
