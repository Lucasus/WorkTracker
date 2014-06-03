using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTracker.UI
{
    public class IconManager
    {
        private WorkStateManager workStateManager;

        public Icon CurrentIcon { get; set; }

        public event EventHandler IconChanged;
        public void OnIconChanged()
        {
            IconChanged(this, EventArgs.Empty);
        }

        public IconManager(WorkStateManager workStateManager)
        {
            this.workStateManager = workStateManager;
            this.workStateManager.StateChanged += workStateManager_StateChanged;
            updateIcon();
        }

        private void workStateManager_StateChanged(object sender, EventArgs e)
        {
            updateIcon();
            OnIconChanged();
        }

        private void updateIcon()
        {
            switch (workStateManager.CurrentState)
            {
                case WorkState.Work:
                    CurrentIcon = new Icon("/circle-green.ico");
                    break;
                case WorkState.Break:
                    CurrentIcon = new Icon("/circle-yellow.ico");
                    break;
                case WorkState.Stopped:
                    CurrentIcon = new Icon("/circle-gray.ico");
                    break;
            }
        }
    }
}
