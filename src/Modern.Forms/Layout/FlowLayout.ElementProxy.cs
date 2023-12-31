﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Drawing;

namespace Modern.Forms.Layout;

internal partial class FlowLayout
{
    /// <summary>
    ///  ElementProxy inserts a level of indirection between the LayoutEngine
    ///  and the IArrangedElement that allows us to use the same code path
    ///  for Vertical and Horizontal flow layout. (see VerticalElementProxy)
    /// </summary>
    private class ElementProxy
    {
        private IArrangedElement _element = null!;

        public virtual AnchorStyles AnchorStyles {
            get {
                var anchorStyles = LayoutUtils.GetUnifiedAnchor (Element);
                var isStretch = (anchorStyles & LayoutUtils.VerticalAnchorStyles) == LayoutUtils.VerticalAnchorStyles; //whether the control stretches to fill in the whole space
                var isTop = (anchorStyles & AnchorStyles.Top) != 0;   //whether the control anchors to top and does not stretch;
                var isBottom = (anchorStyles & AnchorStyles.Bottom) != 0;  //whether the control anchors to bottom and does not stretch;

                //the element stretches to fill in the whole row. Equivalent to AnchorStyles.Top|AnchorStyles.Bottom
                if (isStretch)
                    return LayoutUtils.VerticalAnchorStyles;

                //the element anchors to top and doesn't stretch
                if (isTop)
                    return AnchorStyles.Top;

                //the element anchors to bottom and doesn't stretch
                if (isBottom)
                    return AnchorStyles.Bottom;

                return AnchorStyles.None;
            }
        }

        public bool AutoSize => CommonProperties.GetAutoSize (_element);

        public virtual Rectangle Bounds {
            set => _element.SetBounds (value, BoundsSpecified.None);
        }

        public IArrangedElement Element {
            get => _element;
            set {
                _element = value;
                Debug.Assert (Element == value, "Element should be the same as we set it to");
            }
        }

        public virtual Padding Margin => CommonProperties.GetMargin (Element);

        public virtual Size MinimumSize => CommonProperties.GetMinimumSize (Element, Size.Empty);

        public bool ParticipatesInLayout => _element.ParticipatesInLayout;

        public virtual Size SpecifiedSize => CommonProperties.GetSpecifiedBounds (_element).Size;

        public bool Stretches {
            get {
                var styles = AnchorStyles;
                return (LayoutUtils.VerticalAnchorStyles & styles) == LayoutUtils.VerticalAnchorStyles;
            }
        }

        public virtual Size GetPreferredSize (Size proposedSize)
        {
            return _element.GetPreferredSize (proposedSize);
        }
    }
}
