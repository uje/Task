using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WY.Tasks.Controls {
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WY.Tasks.Controls"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WY.Tasks.Controls;assembly=WY.Tasks.Controls"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误:
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:notificationMenu/>
    ///
    /// </summary>
    class NotificationMenu : Control {
        System.Windows.Forms.NotifyIcon notifyIcon;
        System.Windows.Forms.ContextMenu contextMenu;
        static NotificationMenu() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationMenu), new FrameworkPropertyMetadata(typeof(NotificationMenu)));
        }

        public NotificationMenu() {
            notifyIcon = new System.Windows.Forms.NotifyIcon();
            contextMenu = new System.Windows.Forms.ContextMenu();
            notifyIcon.Visible = true;
            notifyIcon.ContextMenu = contextMenu;
        }

        public System.Drawing.Icon Icon {
            get { return notifyIcon.Icon; }
            set { notifyIcon.Icon = value; }
        }
        public EventHandler DBClick {
            set { notifyIcon.DoubleClick += value; }
        }

        public System.Windows.Forms.Menu.MenuItemCollection Items {
            get { return contextMenu.MenuItems; }
        }

        public void AddMenu(string title,EventHandler click,string name) {
            contextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem(title, click) {
                Name = name
            });
        }
    }

}
