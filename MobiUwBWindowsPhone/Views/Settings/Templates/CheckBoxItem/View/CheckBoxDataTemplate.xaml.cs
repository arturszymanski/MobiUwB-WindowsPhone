#region

using System.Windows.Controls;
using MobiUwB.Views.Settings.Templates.CheckBoxItem.Model;

#endregion

namespace MobiUwB.Views.Settings.Templates.CheckBoxItem.View
{
    public partial class CheckBoxDataTemplate : UserControl
    {
        
        public CheckBoxDataTemplate()
        {
            InitializeComponent();
        }

        private void Grid_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            CheckBoxTemplateModel d = (CheckBoxTemplateModel)DataContext;
            d.IsChecked = !d.IsChecked;
        }
    }
}
