using System;
using BizTalkComponents.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winterdom.BizTalk.PipelineTesting;
using Microsoft.BizTalk.Component;
using System.Net.Mail;
using BizTalkComponents.PipelineComponents.SetTypedProperty;
using System.Text;
using System.IO;
using System.Xml;

namespace BizTalkComponents.PipelineComponents.SetTypedProperty.Tests.UnitTests
{
    [TestClass]
    public class SetTypedPropertyTests
    {
        [TestMethod]
        public void TestSetTypedProperty_Int()
        {
            var pipeline = PipelineFactory.CreateEmptySendPipeline();
            var component = new SetTypedProperty {
                PropertyPath ="http://unit.test#prop1",
                PromoteProperty=true,
                Value="10",
                ValueType="Int32"
            };
            pipeline.AddComponent(component, PipelineStage.Assemble);       
            var message = MessageHelper.CreateFromString("<Message />");
            var output = pipeline.Execute(message);
            object value = null;
            output.Context.TryRead(new ContextProperty("http://unit.test#prop1"), out value);
            Assert.IsTrue(value is int & ((int)value) == 10);
        }

        [TestMethod]
        public void TestSetTypedProperty_Bool()
        {
            var pipeline = PipelineFactory.CreateEmptySendPipeline();
            var component = new SetTypedProperty
            {
                PropertyPath = "http://unit.test#prop1",
                PromoteProperty = true,
                Value = "true",
                ValueType = "Boolean"
            };
            pipeline.AddComponent(component, PipelineStage.Assemble);
            var message = MessageHelper.CreateFromString("<Message />");
            var output = pipeline.Execute(message);
            object value = null;
            output.Context.TryRead(new ContextProperty("http://unit.test#prop1"), out value);
            Assert.IsTrue((bool)value);
        }

        [TestMethod]
        public void TestSetTypedProperty_DateTime()
        {
            var pipeline = PipelineFactory.CreateEmptySendPipeline();
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var component = new SetTypedProperty
            {
                PropertyPath = "http://unit.test#prop1",
                PromoteProperty = true,
                Value = now.ToString("yyyy-MM-ddTHH:mm:ss"),
                ValueType = "DateTime"
            };
            pipeline.AddComponent(component, PipelineStage.Assemble);
            var message = MessageHelper.CreateFromString("<Message />");
            var output = pipeline.Execute(message);
            object value = null;
            output.Context.TryRead(new ContextProperty("http://unit.test#prop1"), out value);
            Assert.IsTrue(value is DateTime & ((DateTime)value) == now);
        }
        [TestMethod]
        [ExpectedException(typeof(FormatException))]

        public void TestSetTypedProperty_WrongType()
        {
            var pipeline = PipelineFactory.CreateEmptySendPipeline();
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var component = new SetTypedProperty
            {
                PropertyPath = "http://unit.test#prop1",
                PromoteProperty = true,
                Value = now.ToString("yyyy-MM-ddTHH:mm:ss"),
                ValueType = "Int32"
            };
            pipeline.AddComponent(component, PipelineStage.Assemble);
            var message = MessageHelper.CreateFromString("<Message />");
            var output = pipeline.Execute(message);
            object value = null;
            output.Context.TryRead(new ContextProperty("http://unit.test#prop1"), out value);
            Assert.IsFalse(value is int);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSetTypedProperty_InvalidValueType()
        {
            var pipeline = PipelineFactory.CreateEmptySendPipeline();
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var component = new SetTypedProperty
            {
                PropertyPath = "http://unit.test#prop1",
                PromoteProperty = true,
                Value = now.ToString("yyyy-MM-ddTHH:mm:ss"),
                ValueType = "some type"
            };
            pipeline.AddComponent(component, PipelineStage.Assemble);
            var message = MessageHelper.CreateFromString("<Message />");
            var output = pipeline.Execute(message);
            object value = null;
            output.Context.TryRead(new ContextProperty("http://unit.test#prop1"), out value);
            Assert.IsFalse(value is int);
        }

        [TestMethod]
        [ExpectedException(typeof(OverflowException))]
        public void TestSetTypedProperty_Overflow()
        {
            var pipeline = PipelineFactory.CreateEmptySendPipeline();
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var component = new SetTypedProperty
            {
                PropertyPath = "http://unit.test#prop1",
                PromoteProperty = true,
                Value = "256",
                ValueType = "byte"
            };
            pipeline.AddComponent(component, PipelineStage.Assemble);
            var message = MessageHelper.CreateFromString("<Message />");
            var output = pipeline.Execute(message);
            object value = null;
            output.Context.TryRead(new ContextProperty("http://unit.test#prop1"), out value);
            Assert.IsFalse(value is byte);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void TestSetTypedProperty_InvalidCast()
        {
            var pipeline = PipelineFactory.CreateEmptySendPipeline();
            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            var component = new SetTypedProperty
            {
                PropertyPath = "http://unit.test#prop1",
                PromoteProperty = true,
                Value = "-1",
                ValueType = "empty"
            };
            pipeline.AddComponent(component, PipelineStage.Assemble);
            var message = MessageHelper.CreateFromString("<Message />");
            var output = pipeline.Execute(message);
            object value = null;
            output.Context.TryRead(new ContextProperty("http://unit.test#prop1"), out value);
            Assert.IsFalse(value is byte);
        }
    }
}
