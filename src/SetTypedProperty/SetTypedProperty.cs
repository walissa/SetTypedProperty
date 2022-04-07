using System;
using BizTalkComponents.Utils;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using IComponent = Microsoft.BizTalk.Component.Interop.IComponent;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BizTalkComponents.PipelineComponents.SetTypedProperty
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [System.Runtime.InteropServices.Guid("4e2ec3d6-7052-4703-b05f-339eee351ed7")]
    public partial class SetTypedProperty : IBaseComponent, IComponent, IComponentUI, IPersistPropertyBag
    {
        [DisplayName("Property Path")]
        [Description("The property path where the specified value will be promoted to, i.e. http://temupuri.org#MyProperty.")]
        [RegularExpression(@"^.*#.*$", ErrorMessage = "A property path should be formatted as namespace#property.")]
        [RequiredRuntime]
        public string PropertyPath { get; set; }

        [DisplayName("Value")]
        [Description("The value that should be promoted to the specified property.")]
        [RequiredRuntime]
        public string Value { get; set; }

        [DisplayName("Promote Property")]
        [Description("Specifies whether the property should be promoted or just written to the context.")]
        [RequiredRuntime]
        public bool PromoteProperty { get; set; }

        [DisplayName("Value Type")]
        [Description("The type of the value being assigned to the specified property.")]
        [RequiredRuntime]
        [DefaultValue("String")]
        public string ValueType { get; set; }



        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            if (string.IsNullOrEmpty(ValueType))
            {
                ValueType = "String";
            }
            string errorMessage;
            if (!Validate(out errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }
            TypeCode tc = (TypeCode)Enum.Parse(typeof(TypeCode), ValueType, true);
            object typedValue = Convert.ChangeType(Value, tc);
            if (PromoteProperty)
            {
                pInMsg.Context.Promote(new ContextProperty(PropertyPath), typedValue);
            }
            else
            {
                pInMsg.Context.Write(new ContextProperty(PropertyPath), typedValue);
            }

            return pInMsg;
        }
    }
}