using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WireWorld
{
    public static class MathExtensions
    {
        public static Vector2 GetTopLeft(this RectangleF rectangle)
        {
            return new Vector2(rectangle.Left, rectangle.Top);
        }

        public static Vector2 GetTopRight(this RectangleF rectangle)
        {
            return new Vector2(rectangle.Right, rectangle.Top);
        }

        public static Vector2 GetBottomLeft(this RectangleF rectangle)
        {
            return new Vector2(rectangle.Left, rectangle.Bottom);
        }

        public static Vector2 GetBottomRight(this RectangleF rectangle)
        {
            return new Vector2(rectangle.Right, rectangle.Bottom);
        }

        public static Vector2 GetTopLeft(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Left, rectangle.Top);
        }

        public static Vector2 GetTopRight(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Right, rectangle.Top);
        }

        public static Vector2 GetBottomLeft(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Left, rectangle.Bottom);
        }

        public static Vector2 GetBottomRight(this Rectangle rectangle)
        {
            return new Vector2(rectangle.Right, rectangle.Bottom);
        }

        public static RectangleF Translated(this RectangleF rectangle, Vector2 offset)
        {
            RectangleF newRectangle = rectangle;

            newRectangle.X += offset.X;
            newRectangle.Y += offset.Y;

            return newRectangle;
        }

        public static Rectangle Translated(this Rectangle rectangle, Vector2 offset)
        {
            Rectangle newRectangle = rectangle;

            newRectangle.X += (int)offset.X;
            newRectangle.Y += (int)offset.Y;

            return newRectangle;
        }

        public static RectangleF Scaled(this RectangleF rectangle, float scale)
        {
            RectangleF newRectangle = rectangle;

            newRectangle.X *= scale;
            newRectangle.Y *= scale;
            newRectangle.Width *= scale;
            newRectangle.Height *= scale;

            return newRectangle;
        }

        public static Rectangle Scaled(this Rectangle rectangle, int scale)
        {
            Rectangle newRectangle = rectangle;

            newRectangle.X *= scale;
            newRectangle.Y *= scale;
            newRectangle.Width *= scale;
            newRectangle.Height *= scale;

            return newRectangle;
        }

        public static RectangleF Inflated(this RectangleF rectangle, float x, float y)
        {
            RectangleF newRectangle = rectangle;

            newRectangle.X -= x;
            newRectangle.Y -= y;
            newRectangle.Width += x * 2;
            newRectangle.Height += y * 2;

            return newRectangle;
        }

        public static RectangleF Inflated(this RectangleF rectangle, float left, float top, float right, float bottom)
        {
            RectangleF newRectangle = rectangle;

            newRectangle.X -= left;
            newRectangle.Y -= right;
            newRectangle.Width += left + right;
            newRectangle.Height += top + bottom;

            return newRectangle;
        }

        public static Rectangle Inflated(this Rectangle rectangle, int x, int y)
        {
            Rectangle newRectangle = rectangle;

            newRectangle.X -= x;
            newRectangle.Y -= y;
            newRectangle.Width += x * 2;
            newRectangle.Height += y * 2;

            return newRectangle;
        }

        public static Rectangle Inflated(this Rectangle rectangle, int left, int top, int right, int bottom)
        {
            Rectangle newRectangle = rectangle;

            newRectangle.X -= left;
            newRectangle.Y -= right;
            newRectangle.Width += left + right;
            newRectangle.Height += top + bottom;

            return newRectangle;
        }

        public static RectangleF Joined(this RectangleF rectangle, RectangleF other)
        {
            float x1 = Math.Min(rectangle.X, other.X);
            float x2 = Math.Max(rectangle.X + rectangle.Width, other.X + other.Width);
            float y1 = Math.Min(rectangle.Y, other.Y);
            float y2 = Math.Max(rectangle.Y + rectangle.Height, other.Y + other.Height);

            return new RectangleF(x1, y1, x2 - x1, y2 - y1);
        }

        public static Rectangle Joined(this Rectangle rectangle, Rectangle other)
        {
            return Rectangle.Union(rectangle, other);
        }

        public static bool Contains(this Rectangle rectangle, Vector2 point)
        {
            return rectangle.Contains((int)point.X, (int)point.Y);
        }

        public static bool Contains(this RectangleF rectangle, Vector2 point)
        {
            return point.X > rectangle.Left && point.X <= rectangle.Right && point.Y > rectangle.Top && point.Y < rectangle.Bottom;
        }

        public static Vector3 Round(this Vector3 vec)
        {
            return new Vector3((float)Math.Round(vec.X), (float)Math.Round(vec.Y), (float)Math.Round(vec.Z));
        }

        public static Vector3 Round(this Vector3 vec, int decimals)
        {
            return new Vector3((float)Math.Round(vec.X, decimals), (float)Math.Round(vec.Y, decimals), (float)Math.Round(vec.Z, decimals));
        }

        public static Vector3 Floor(this Vector3 vec)
        {
            return new Vector3((float)Math.Floor(vec.X), (float)Math.Floor(vec.Y), (float)Math.Floor(vec.Z));
        }

        public static Vector3 Ceiling(this Vector3 vec)
        {
            return new Vector3((float)Math.Ceiling(vec.X), (float)Math.Ceiling(vec.Y), (float)Math.Ceiling(vec.Z));
        }

        public static Vector3 Abs(this Vector3 vec)
        {
            return new Vector3(Math.Abs(vec.X), Math.Abs(vec.Y), Math.Abs(vec.Z));
        }

        public static Vector2 Round(this Vector2 vec)
        {
            return new Vector2((float)Math.Round(vec.X), (float)Math.Round(vec.Y));
        }

        public static Vector2 Round(this Vector2 vec, int decimals)
        {
            return new Vector2((float)Math.Round(vec.X, decimals), (float)Math.Round(vec.Y, decimals));
        }

        public static Vector2 Floor(this Vector2 vec)
        {
            return new Vector2((float)Math.Floor(vec.X), (float)Math.Floor(vec.Y));
        }

        public static Vector2 Ceiling(this Vector2 vec)
        {
            return new Vector2((float)Math.Ceiling(vec.X), (float)Math.Ceiling(vec.Y));
        }

        public static Vector2 Abs(this Vector2 vec)
        {
            return new Vector2(Math.Abs(vec.X), Math.Abs(vec.Y));
        }

        public static Vector4 ToVector(this Color color)
        {
            return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        public static Vector2 ToVector(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Vector2 ToVector(this PointF point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Vector2 ToVector(this Size size)
        {
            return new Vector2(size.Width, size.Height);
        }

        public static Vector2 ToVector(this SizeF size)
        {
            return new Vector2(size.Width, size.Height);
        }

        public static Vector4 ToVector(this Rectangle rectangle)
        {
            return new Vector4(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static Vector4 ToVector(this RectangleF rectangle)
        {
            return new Vector4(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
        }

        public static Point ToPoint(this Vector2 vector)
        {
            return new Point((int)vector.X, (int)vector.Y);
        }

        public static PointF ToPointF(this Vector2 vector)
        {
            return new PointF(vector.X, vector.Y);
        }

        public static Size ToSize(this Vector2 vector)
        {
            return new Size((int)vector.X, (int)vector.Y);
        }

        public static SizeF ToSizeF(this Vector2 vector)
        {
            return new SizeF(vector.X, vector.Y);
        }

        public static Rectangle ToRectangle(this Size size)
        {
            return new Rectangle(0, 0, size.Width, size.Height);
        }
    }
}
