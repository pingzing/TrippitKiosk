using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace TrippitKiosk.Extensions
{
    public static class VisualTreeExtensions
    {
        public static T FindDescendant<T>(this DependencyObject thisElement) where T : DependencyObject
        {
            T retValue = null;
            var childrenCount = VisualTreeHelper.GetChildrenCount(thisElement);

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(thisElement, i);
                T castChild = child as T;
                if (castChild != null)
                {
                    retValue = castChild;
                    break;
                }

                retValue = FindDescendant<T>(child);

                if (retValue != null)
                {
                    break;
                }
            }

            return retValue;
        }
    }
}
