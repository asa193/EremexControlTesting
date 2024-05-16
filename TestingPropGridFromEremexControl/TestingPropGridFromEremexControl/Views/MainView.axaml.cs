using Avalonia.Controls;
using Eremex.AvaloniaUI.Controls.PropertyGrid;

namespace TestingPropGridFromEremexControl.Views {
    public partial class MainView : UserControl {
        public MainView() {
            InitializeComponent();
        }

        private void PropertyGridControl_PropertyChanged(object? sender, Avalonia.AvaloniaPropertyChangedEventArgs e) {
            if(e.Property == PropertyGridControl.RowsSourceProperty) {
                propertyGrid.FocusedRow = null;
            }
        }
    }
}