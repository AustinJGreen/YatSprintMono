using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YatSprint.UI
{
    public enum DockType
    {
        UpperLeft,
        UpperRight,
        LowerLeft,
        LowerRight
    }
    public class Resize
    {
        private enum ResizeMode
        {
            Absolute,
            Relative,
            Dock
        }
        private Vector2 position;
        private Vector2 centeringDividends;
        private Vector2 offset;
        private Vector2 size;
        private Vector2 margin;

        private ResizeMode mode;
        private DockType docktype;

        public void ChangeSize(Vector2 size)
        {
            this.size = size;
        }

        public Vector2 GetPosition(Rectangle window)
        {
            switch (mode)
            {
                case ResizeMode.Absolute:
                    return position;
                case ResizeMode.Dock:
                    switch (docktype)
                    {
                        case DockType.LowerLeft:
                            return new Vector2(margin.X, window.Height - margin.Y - size.Y);
                        case DockType.LowerRight:
                            return new Vector2(window.Width - margin.X - size.X, window.Height - margin.Y - size.Y);
                        case DockType.UpperLeft:
                            return margin;
                        case DockType.UpperRight:
                            return new Vector2(window.Width - margin.X - size.X, margin.Y);
                        default:
                            throw new NullReferenceException();
                    }
                case ResizeMode.Relative:
                    Vector2 pos = new Vector2(0);
                    if (centeringDividends.X != 0)
                        pos = new Vector2(window.Width / centeringDividends.X, pos.Y);
                    if (centeringDividends.Y != 0)
                        pos = new Vector2(pos.X, window.Height / centeringDividends.Y);
                    pos.X += offset.X;
                    pos.Y += offset.Y;
                    return pos;
                default:
                    throw new NullReferenceException();
            }
            
        }

        public Resize(Vector2 position)
        {
            this.position = position;
            this.mode = ResizeMode.Absolute;
        }

        public Resize(DockType dock, Vector2 size, Vector2 margin)
        {
            this.docktype = dock;
            this.size = size;
            this.margin = margin;
            this.mode = ResizeMode.Dock;
        }

        public Resize(Vector2 centeringDividends, Vector2 offset)
        {
            this.centeringDividends = centeringDividends;
            this.offset = offset;
            this.mode = ResizeMode.Relative;
        }
    }
}
