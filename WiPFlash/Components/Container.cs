﻿#region

using System.Collections.Generic;
using System.Windows.Automation;
using WiPFlash.Exceptions;
using WiPFlash.Framework;
using WiPFlash.Framework.Events;

#endregion

namespace WiPFlash.Components
{
    public class Container<T> : AutomationElementWrapper<T>, IContainChildren where T : Container<T>
    {
        private readonly IFindAutomationElements _finder;
        public FailureToFindHandler HandlerForFailingToFind { get; set; }

        public Container(AutomationElement element) : this(element, string.Empty)
        {
        }

        public Container(AutomationElement element, string name) : this(element, name, new PropertyBasedFinder(new WrapperFactory()))
        {
        }

        public Container(AutomationElement element, string name, IFindAutomationElements finder) : base(element, name)
        {
            _finder = finder;
            HandlerForFailingToFind = (s) => { throw new FailureToFindException(s); };
        }

        public TC Find<TC>(string componentName) where TC : AutomationElementWrapper<TC>
        {
            return Find<TC>(FindBy.UiAutomationId(componentName));
        }

        public TC Find<TC>(PropertyCondition condition) where TC : AutomationElementWrapper<TC>
        {
            TC find = _finder.Find<TC, T>(this, condition, HandlerForFailingToFind);
            if (find is IContainChildren)
            {
                ((IContainChildren) find).HandlerForFailingToFind = HandlerForFailingToFind;
            }
            return find;
        }

        protected override IEnumerable<AutomationEventWrapper> SensibleEventsToWaitFor
        {
            get {
                return new AutomationEventWrapper[] {new FocusEvent()};
            }
        }

        public bool Contains(string name)
        {
            return Contains(FindBy.UiAutomationId(name));
        }

        public bool Contains(PropertyCondition condition)
        {
            return _finder.Contains(this, condition);
        }
    }
}
