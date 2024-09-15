using System;
using Foundation;
using UIKit;
using Walkman.iOS.Utils;

namespace Walkman.iOS.ViewControllers
{
    public partial class MoreTableViewController : UITableViewController
    {
        public MoreTableViewController(IntPtr handle) : base(handle) { }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(indexPath.Row.ToString());

            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            TableView.BackgroundColor = ColorUtils.GetInterfaceStyle(TableView.Frame.Width, TableView.Frame.Height, TableView.Frame.Size);
            TableView.Layer.CornerRadius = 10;

            return cell;
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return 3;
        }
    }
}


