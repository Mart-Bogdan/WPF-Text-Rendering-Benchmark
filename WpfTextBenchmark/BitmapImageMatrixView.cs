﻿/* 
 * Copyright (C) 2010 Daniel Grunwald
 *
 * This sourcecode is licenced under The GNU Lesser General Public License
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
 * NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 * DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
 * OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Threading;

namespace WpfTextBenchmark
{
	public class BitmapImageMatrixView : MatrixView
    {
        private readonly static BitmapImage _ImageSource;
        private readonly static Size _imageSize;

        static BitmapImageMatrixView()
        {
            _ImageSource = new BitmapImage(new Uri("pack://application:,,,/Resources/ball.png"));
            _ImageSource.CacheOption = BitmapCacheOption.OnLoad;
            _imageSize = new Size(52, 52);
        }

		TextFormatter formatter = TextFormatter.Create(TextFormattingMode.Display);
		Typeface cachedTypeface;
		
		public override string ToString()
		{
            return "BitmapImage";
		}
		
		public override IText AddText(string text, double fontSize, SolidColorBrush brush)
		{
			if (cachedTypeface == null) {
				cachedTypeface = CreateTypeface();
			}
			MyText myText = new MyText { /*line = line, */parent = this };
			texts.Add(myText);
			return myText;
		}
		
		List<MyText> texts = new List<MyText>();

        protected override void OnRender(DrawingContext drawingContext)
		{
			foreach (MyText text in texts)
			{
			    drawingContext.DrawImage(_ImageSource, new Rect(text.Position, _imageSize));
			}
			Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(delegate { OnFrameRendered(EventArgs.Empty); }));
		}
		
		class MyText : IText
		{
            public BitmapImageMatrixView parent;
			public Image line;
			public Point Position { get; set; }
			
			public void Remove()
			{
                //line.Dispose();
				parent.texts.Remove(this);
			}
		}
		
	}
}
