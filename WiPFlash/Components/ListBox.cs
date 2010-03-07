﻿#region

using System.Collections.Generic;
using System.Windows.Automation;
using WiPFlash.Framework.Events;

#endregion

namespace WiPFlash.Components
{
    public class ListBox : AutomationElementWrapper<ListBox>
    {
        public ListBox(AutomationElement element, string name) : base(element, name)
        {
        }

        public string[] Selection
        {
            get
            {
                var selectedItems = GetSelectedElements();
                var result = new List<string>();
                foreach (var item in selectedItems)
                {
                    result.Add(item.GetCurrentPropertyValue(AutomationElement.NameProperty).ToString());
                }
                return result.ToArray();
            }
        }

        public string[] Items
        {
            get
            {
                var result = new List<string>();
                foreach (AutomationElement element in AllItemElements())
                {
                    result.Add(element.GetCurrentPropertyValue(AutomationElement.NameProperty).ToString());
                }
                return result.ToArray();
            }
        }

        private AutomationElement[] GetSelectedElements()
        {
            return ((SelectionPattern) Element.GetCurrentPattern(SelectionPattern.Pattern))
                .Current.GetSelection();
        }

        public void Select(params string[] selections)
        {
            var selectionList = new List<string>(selections);
            foreach (AutomationElement listItem in AllItemElements())
            {
                if (selectionList.Contains(listItem.GetCurrentPropertyValue(AutomationElement.NameProperty).ToString()))
                {
                    ((SelectionItemPattern)listItem.GetCurrentPattern(SelectionItemPattern.Pattern)).AddToSelection();
                }
            }
        }

        private AutomationElementCollection AllItemElements()
        {
            return Element.FindAll(TreeScope.Children,
                                   new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.ListItem));
        }

        public void ClearSelection()
        {
            var selection = GetSelectedElements();
            foreach (AutomationElement element in selection)
            {
                ((SelectionItemPattern)element.GetCurrentPattern(SelectionItemPattern.Pattern))
                    .RemoveFromSelection();
            }
        }

        protected override IEnumerable<AutomationEventWrapper> SensibleEventsToWaitFor
        {
            get 
            {
                return new AutomationEventWrapper[]
                   {
                       new StructureChangeEvent(TreeScope.Element),
                       new OrdinaryEvent(SelectionItemPattern.ElementAddedToSelectionEvent, TreeScope.Descendants),
                       new OrdinaryEvent(SelectionItemPattern.ElementSelectedEvent, TreeScope.Descendants),
                       new OrdinaryEvent(SelectionItemPattern.ElementRemovedFromSelectionEvent, TreeScope.Descendants)
                   };
            }
        }
    }
}