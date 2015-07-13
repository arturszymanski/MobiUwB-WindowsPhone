using SharedCode.Tasks;
using SharedCode.Tasks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationsAgent.DataInitialize.Tasks.CategoriesFinder
{
    public class CategoriesFinderTask : ITask<DataInitializeTaskOutput>
    {
        public String notificationText;
        public String timeRangeText;

        public String frequencyText;

        public String fromText;
        public String toText;

        public void Execute(TaskInput input, DataInitializeTaskOutput output)
        {
            List<TemplateModel> itemModels = settingsPreferencesManager.restore(output.configXmlResult);



            notificationText = baseContextResources.getString(
                    R.string.settings_category_annoucements);
            timeRangeText = baseContextResources.getString(R.string.settings_subcategory_time_range);
            frequencyText = baseContextResources.getString(R.string.settings_subcategory_frequency);
            fromText = baseContextResources.getString(
                    R.string.settings_subsubcategory_from);
            toText = baseContextResources.getString(
                    R.string.settings_subsubcategory_to);

            try
            {
                recursiveFindModel(itemModels, output);
                int[] array = baseContextResources.getIntArray(
                        R.array.frequencies_values);
                output.interval = array[output.intervalIndex];
            }
            catch (Exception e)
            {
                output.addError(e.getMessage());
            }
        }

        private void recursiveFindModel(List<ItemModel> itemModels,
                                        DataInitializeTaskOutput output)
                throws Exception
        {
            for(ItemModel itemModel : itemModels)
            {
                if(itemModel instanceof CheckBoxItemModel)
                {
                    CheckBoxItemModel checkBoxItemModel = (CheckBoxItemModel)itemModel;

                    String key = checkBoxItemModel.getId();
                    Boolean selected = checkBoxItemModel.isChecked();
                    if(!output.categories.containsKey(key))
                    {
                        output.categories.put(key, selected);
                    }
                    else
                    {
                        throw new IllegalArgumentException("Unhandled CheckBoxItemModel");
                    }
                }
                else if(itemModel instanceof ExpandableOnOffItemModel)
                {
                    ExpandableOnOffItemModel expandableOnOffItemModel = (ExpandableOnOffItemModel)itemModel;

                    if(expandableOnOffItemModel.getText().equals(notificationText))
                    {
                        output.isNotificationActive = expandableOnOffItemModel.isExpanded();
                    }
                    else if(expandableOnOffItemModel.getText().equals(timeRangeText))
                    {
                        output.isTimeRangeActive = expandableOnOffItemModel.isExpanded();
                    }
                    else
                    {
                        throw new IllegalArgumentException("Unhandled ExpandableOnOffItemModel");
                    }
                }
                else if(itemModel instanceof ListPickerItemModel)
                {
                    ListPickerItemModel listPickerItemModel = (ListPickerItemModel)itemModel;

                    if(listPickerItemModel.getText().equals(frequencyText))
                    {
                        output.intervalIndex = listPickerItemModel.getValue();
                    }
                    else
                    {
                        throw new IllegalArgumentException("Unhandled ListPickerItemModel");
                    }
                }
                else if(itemModel instanceof TimePickerItemModel)
                {
                    TimePickerItemModel timePickerItemModel = (TimePickerItemModel)itemModel;

                    if(timePickerItemModel.getText().equals(fromText))
                    {
                        output.timeRangeFrom = timePickerItemModel.getValue();
                    }
                    else if(timePickerItemModel.getText().equals(toText))
                    {
                        output.timeRangeTo = timePickerItemModel.getValue();
                    }
                    else
                    {
                        throw new IllegalArgumentException("Unhandled TimePickerItemModel");
                    }
                }

                recursiveFindModel(itemModel.nestedModels,output);
            }
        }
    }
}
