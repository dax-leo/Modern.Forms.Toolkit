// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they begin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
// 
// - Sun Tsu,
// "The Art of War"

using Modern.Forms;
using System;
using System.Collections.Generic;
using TheArtOfDev.HtmlRenderer.Adapters;
using TheArtOfDev.HtmlRenderer.Adapters.Entities;
using TheArtOfDev.HtmlRenderer.Avalonia.Utilities;
using TheArtOfDev.HtmlRenderer.Core.Utils;

namespace TheArtOfDev.HtmlRenderer.Avalonia.Adapters
{
    /// <summary>
    /// Adapter for Modern.Forms context menu for core.
    /// </summary>
    internal sealed class ContextMenuAdapter : RContextMenu
    {
        #region Fields and Consts

        /// <summary>
        /// the underline Avalonia context menu
        /// </summary>
        private readonly ContextMenu _contextMenu;

        #endregion


        /// <summary>
        /// Init.
        /// </summary>
        public ContextMenuAdapter()
        {
            _contextMenu = new ContextMenu();
        }

        private MenuItemCollection Items => ((MenuItemCollection)_contextMenu.Items);
        
        public override int ItemsCount
        {
            get { return Items.Count; }
        }

        public override void AddDivider()
        {
            Items.Add(new MenuSeparatorItem());
        }

        public override void AddItem(string text, bool enabled, EventHandler onClick)
        {
            ArgChecker.AssertArgNotNullOrEmpty(text, "text");
            ArgChecker.AssertArgNotNull(onClick, "onClick");

            var item = new MenuItem();
            item.Text = text;
            item.Enabled = enabled;
            item.Click += (sender, args) => onClick(sender, args); 
            Items.Add(item);
        }

        public override void RemoveLastDivider()
        {
            if (Items[Items.Count - 1].GetType() == typeof(MenuSeparatorItem))
                Items.RemoveAt(Items.Count - 1);
        }

        public override void Show(RControl parent, RPoint location)
        {            
            _contextMenu.Show(((ControlAdapter)parent).Control, new System.Drawing.Point((int)location.X, (int)location.Y));
            //_contextMenu.ShowAt(((ControlAdapter)parent).Control, true);
        }

        public override void Dispose()
        {
            _contextMenu.Hide();
            Items.Clear();
        }
    }
}