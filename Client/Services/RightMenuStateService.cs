using System;
using Quanda.Client.Shared.RightMenu;

namespace Quanda.Client.Services
{
    public class RightMenuStateService
    {
        private RightMenuType _activeMenu;

        public Action OnAction;

        public RightMenuType RightMenuType
        {
            get => _activeMenu;
            set
            {
                _activeMenu = value;
                NotifyStateChange();
            }
        }

        public void NotifyStateChange()
        {
            OnAction?.Invoke();
        }
    }
}
