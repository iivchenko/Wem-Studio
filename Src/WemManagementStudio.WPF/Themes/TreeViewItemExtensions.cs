using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace DarkBlendTheme
{
    public static class TreeViewItemExtensions
    {
        public static int GetDepth(this TreeViewItem item)
        {
            TreeViewItem parent;
            while ((parent = GetParent(item)) != null)
            {
                return GetDepth(parent) + 1;
            }
            return 0;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Justification = "Third party")]
        private static TreeViewItem GetParent(TreeViewItem item)
        {
            var parent = VisualTreeHelper.GetParent(item);
				
            while (!(parent is TreeViewItem || parent is TreeView))
            {
				if (parent == null) return null;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as TreeViewItem;
        }
    }
}
